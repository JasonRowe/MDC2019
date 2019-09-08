using System;
using Microsoft.Extensions.Configuration;

namespace TCCC23.Publisher
{
	public static class Configuration
	{
		private static readonly Lazy<IConfigurationRoot> ConfigurationRoot = new Lazy<IConfigurationRoot>(LoadConfiguration);

		public static string Host => ConfigurationRoot.Value.GetValue<string>("RabbitMQConnetionString");

		public static string HelloWorldExchangeName => ConfigurationRoot.Value.GetValue<string>("HelloWorldExchangeName");

		public static string HelloWorldRoutingKey => ConfigurationRoot.Value.GetValue<string>("HelloWorldRoutingKey");

		private static IConfigurationRoot LoadConfiguration()
		{
			var builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.AddEnvironmentVariables();

			return builder.Build();
		}
	}
}
