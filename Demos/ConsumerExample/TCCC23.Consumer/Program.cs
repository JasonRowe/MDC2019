using System;
using System.Text;
using System.Threading;

namespace TCCC23.Consumer
{
	class Program
	{
		static void Main(string[] args)
		{
			var busFactory = new BusFactory();
			using (var bus = busFactory.GetBus())
			{
				var queueName = Configuration.HelloWorldQueueName;
				var exchangeName = Configuration.HelloWorldExchangeName;
				var routingKey = Configuration.HelloWorldRoutingKey;

				// Declare exchange/queue and setup consumer for hello world message.
				var queue = bus.Advanced.QueueDeclare(queueName);
				var exchange = bus.Advanced.ExchangeDeclare(exchangeName, "topic");
				bus.Advanced.Bind(exchange, queue, routingKey);

				// Setup consumer and onMessage handler.
				bus.Advanced.Consume(queue, (body, properties, info) =>
				 {
					 var json = Encoding.UTF8.GetString(body);
					 // JSON would normally be deserialized via shared Nuget package.
					 Console.WriteLine(json);
				 });

				while (true)
				{
					Console.WriteLine($"Consumer running and connected status = {bus.Advanced.IsConnected}");
					Thread.Sleep(5000);
				}
			}
		}
	}
}
