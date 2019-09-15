using EasyNetQ;
using System;

namespace ConsumerRetryExample
{
	public class BusFactory : IDisposable
	{
		private static IBus busInstance;

		public IBus GetBus()
		{
			if (busInstance == null)
			{
				busInstance = RabbitHutch.CreateBus(Configuration.Host);

				// Setup Error exchange and queue.
				busInstance.Advanced.Conventions.ErrorExchangeNamingConvention = (info) => $"{info.Exchange}_Error_Exchange";
				busInstance.Advanced.Conventions.ErrorQueueNamingConvention = (info) => $"{info.Queue}_Error_Queue";
			}

			return busInstance;
		}

		public void Dispose()
		{
			busInstance.Dispose();
		}
	}
}
