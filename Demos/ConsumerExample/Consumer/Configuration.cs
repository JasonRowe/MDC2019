using System;
using Microsoft.Extensions.Configuration;

namespace Consumer
{
	public static class Configuration
	{
		private static readonly Lazy<IConfigurationRoot> ConfigurationRoot = new Lazy<IConfigurationRoot>(LoadConfiguration);

		public static string Host => ConfigurationRoot.Value.GetValue<string>("Messaging:RabbitMQConnetionString");

		public static string HelloWorldExchangeName => ConfigurationRoot.Value.GetValue<string>("Messaging:Consume:HelloWorldExchangeName");

		public static string HelloWorldRoutingKey => ConfigurationRoot.Value.GetValue<string>("Messaging:Consume:HelloWorldRoutingKey");

		public static string HelloWorldQueueName => ConfigurationRoot.Value.GetValue<string>("Messaging:Consume:HelloWorldQueueName");

		private static IConfigurationRoot LoadConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables();

			return builder.Build();
		}
	}
}
