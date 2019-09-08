using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;

namespace TCCC23.Consumer
{
	public class BusFactory : IDisposable
	{
		private static IBus busInstance;

		public IBus GetBus()
		{
			if (busInstance == null)
			{
				busInstance = RabbitHutch.CreateBus(Configuration.Host);
				busInstance.Advanced.MessageReturned += Advanced_MessageReturned;
			}

			return busInstance;
		}

		private void Advanced_MessageReturned(object sender, MessageReturnedEventArgs e)
		{
			Console.WriteLine($"Message returned with reason {e.MessageReturnedInfo.ReturnReason}.");
		}

		public void Dispose()
		{
			busInstance.Dispose();
		}
	}
}
