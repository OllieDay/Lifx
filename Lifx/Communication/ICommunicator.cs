using System;
using System.Threading;
using System.Threading.Tasks;
using Lifx.Communication.Requests;
using Lifx.Communication.Responses;
using Lifx.Communication.Responses.Payloads;

namespace Lifx.Communication
{
	internal interface ICommunicator : IDisposable
	{
		Task CommunicateAsync(Request request, CancellationToken cancellationToken);
		Task<TResponsePayload> CommunicateAsync<TResponsePayload>(
			Request request,
			CancellationToken cancellationToken
		) where TResponsePayload : ResponsePayload;
	}
}
