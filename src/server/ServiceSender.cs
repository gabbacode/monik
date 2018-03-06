﻿using Monik.Common;
using Monik.Client;
using System.Collections.Concurrent;

namespace Monik.Service
{
	public class AzureServiceSender : AzureSender
	{
		public AzureServiceSender(IServiceSettings aSettings) : base(aSettings.OutcomingConnectionString, aSettings.OutcomingQueue)
		{
		}
	} //end of class
}
