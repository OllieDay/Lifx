using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Lifx.Communication.Requests;
using Lifx.Communication.Requests.Payloads;
using Lifx.Communication.Responses;
using Lifx.Communication.Responses.Payloads;

namespace Lifx.Communication
{
	internal sealed class Communicator : ICommunicator
	{
		private readonly ConcurrentDictionary<byte, Response> _responses = new ConcurrentDictionary<byte, Response>();
		private readonly UdpClient _client = new UdpClient();
		private readonly IResponseParser _responseParser;
		private readonly IPEndPoint _endPoint;
		private readonly TimeSpan _responseExpiry;

		public Communicator(IResponseParser responseParser, IPEndPoint endPoint, TimeSpan responseExpiry)
		{
			_responseParser = responseParser;
			_endPoint = endPoint;
			_responseExpiry = responseExpiry;
		}

		public async Task CommunicateAsync(Request request)
		{
			if (request.AckRequired)
			{
				// Wait for an acknowledgement - response contains an empty payload
				await CommunicateAsync<ResponsePayload>(request);
			}
			else
			{
				// Don't wait for an acknowledgement - fire and forget
				await SendRequestAsync(request);
			}
		}

		public async Task<TResponsePayload> CommunicateAsync<TResponsePayload>(Request request)
			where TResponsePayload : ResponsePayload
		{
			await SendRequestAsync(request);

			var response = await ReceiveResponseAsync(request.Sequence);

			return (TResponsePayload)response.Payload;
		}

		public void Dispose()
		{
			_client.Dispose();
		}

		private async Task SendRequestAsync(Request request)
		{
			var data = request.GetData();

			await _client.SendAsync(data, data.Length, _endPoint).ConfigureAwait(false);
		}

		private async Task<Response> ReceiveResponseAsync(byte sequence)
		{
			using (var cancellationTokenSource = new CancellationTokenSource())
			{
				cancellationTokenSource.CancelAfter(_responseExpiry);

				// Continue until the task is cancelled
				while (true)
				{
					cancellationTokenSource.Token.ThrowIfCancellationRequested();

					Response response;

					// Return response if sequence matches
					if (_responses.TryGetValue(sequence, out response))
					{
						var expiry = response.CreationDate.Add(_responseExpiry);

						if (expiry > DateTime.UtcNow)
						{
							return response;
						}
					}

					var result = await _client.ReceiveAsync()
						.WithCancellation(cancellationTokenSource.Token)
						.ConfigureAwait(false);

					// Return response if sequence matches
					if (_responseParser.TryParseResponse(result.Buffer, out response))
					{
						if (response.Sequence == sequence)
						{
							return response;
						}

						// The response isn't a match for this sequence, but could be valid for another request
						_responses.AddOrUpdate(sequence, response, (_, __) => response);
					}
				}
			}
		}
	}
}
