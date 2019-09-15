using EasyNetQ.SystemMessages;
using EasyNetQ.Topology;
using System;
using System.Text;
using System.Threading;

namespace ConsumerRetryExample
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

				// Declare error queue.
				var errorQueue = bus.Advanced.QueueDeclare($"{queueName}_Error_Queue");

				// Declare delay exchange and binding
				var exchangeDelayed = bus.Advanced.ExchangeDeclare($"{exchangeName}_Delayed", ExchangeType.Topic, false, true, false,false,null, true);
				bus.Advanced.Bind(exchangeDelayed, queue, routingKey);

				// Setup error consumer handler.
				bus.Advanced.Consume<Error>(errorQueue, (error, info) =>
				{
					var retryCount = RetryLogic.GetRetryCount(error.Body.BasicProperties);

					if (retryCount > 3)
					{
						// Example area you would publish to a dead letter exchange.
					}
					else
					{
						retryCount = retryCount + 1;
						var messageProps = error.Body.BasicProperties;
						messageProps.Headers["retry"] = retryCount;
						var delay = retryCount * 1000 * 2;
						Console.WriteLine($"Retry {retryCount} with {delay / 1000} second delay");
						messageProps.Headers["x-delay"] = delay;
						bus.Advanced.Publish(exchangeDelayed, error.Body.RoutingKey, true, messageProps, Encoding.UTF8.GetBytes(error.Body.Message));
					}
				});

				// Setup consumer and onMessage handler.
				bus.Advanced.Consume(queue, (body, properties, info) =>
				{
					var retryCount = RetryLogic.GetRetryCount(properties);

					// Setup fake condition to throw an error for the demo.
					if(retryCount < 3)
					{
						Console.WriteLine($"Throwing error!");
						throw new Exception("Consumer exception thrown.");
					}
					else
					{
						var json = Encoding.UTF8.GetString(body);
						Console.WriteLine(json);
					}
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
