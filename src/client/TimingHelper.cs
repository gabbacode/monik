﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
using Microsoft.ServiceBus.Messaging;
using System.IO;
using System.Threading;
using Monik.Common;
using System.Runtime.CompilerServices;

namespace Monik.Client
{
  public class TimingHelper
  {
    private DateTime FFrom = DateTime.Now;
    private TimingHelper() { }

    public static TimingHelper Create() { return new TimingHelper(); }

    public void Begin() { FFrom = DateTime.Now; }

    public void EndAndLog([CallerMemberName] string aSource = "")
    {
      var _delta = DateTime.Now - FFrom;
      M.ApplicationInfo("{0} execution time: {1}ms", aSource, _delta.TotalMilliseconds);
    }
  }
}