// Copyright (c) 2019 Proto Labs, Inc. All Rights Reserved.
using EasyNetQ;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsumerRetryExample
{
	public static class RetryLogic
	{
		public static int GetRetryCount(MessageProperties properties)
		{
			int retryCount = 0;
			if (properties.HeadersPresent && properties.Headers.ContainsKey("retry"))
			{
				var success = int.TryParse(properties.Headers["retry"].ToString(), out retryCount);
				retryCount = success ? retryCount : 0;
			}

			return retryCount;
		}
	}
}
