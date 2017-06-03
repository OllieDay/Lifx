using System;
using System.Threading.Tasks;
using Lifx.Communication.Requests;
using Lifx.Communication.Responses;
using Lifx.Communication.Responses.Payloads;

namespace Lifx.Communication
{
	internal interface ICommunicator : IDisposable
	{
		Task CommunicateAsync(Request request);
		Task<TResponsePayload> CommunicateAsync<TResponsePayload>(Request request)
			where TResponsePayload : ResponsePayload;
	}
}
