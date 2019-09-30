using System;
using Microsoft.Extensions.Configuration;

namespace Publisher
{
	public static class Configuration
	{
		private static readonly Lazy<IConfigurationRoot> ConfigurationRoot = new Lazy<IConfigurationRoot>(LoadConfiguration);

		public static string Host => ConfigurationRoot.Value.GetValue<string>("Messaging:RabbitMQConnetionString");

		public static string HelloWorldExchangeName => ConfigurationRoot.Value.GetValue<string>("Messaging:Publish:HelloWorldExchangeName");

		public static string HelloWorldRoutingKey => ConfigurationRoot.Value.GetValue<string>("Messaging:Publish:HelloWorldRoutingKey");

		private static IConfigurationRoot LoadConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables();

			return builder.Build();
		}
	}
}
