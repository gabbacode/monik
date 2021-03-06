﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Monik.Common;

namespace Monik.Service
{
    public class AuthToken
    {
        public string sub;
        public long exp;
    }

    public enum EventQueueType : byte
    {
        Azure = 1,
        Rabbit = 2,
        Sql = 3
    }

    public class ActiveQueueContext
    {
        public Action<string> OnError;
        public Action<Event> OnReceivedMessage;
        public Action<IEnumerable<Event>> OnReceivedMessages;
    }

    public class EventQueue
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public EventQueueType Type { get; set; }
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }

    public class Source
    {
        public short ID { get; set; }
        public DateTime Created { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public short? DefaultGroupID { get; set; }
    }

    public class Instance
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public short SourceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        private Source FSourceRef = null;

        public Source SourceRef()
        {
            return FSourceRef;
        }

        public void SourceRef(Source aSrc)
        {
            FSourceRef = aSrc;
        }

        public readonly ConcurrentDictionary<string, IMetricObject> Metrics = new ConcurrentDictionary<string, IMetricObject>();
    }

    public class Group
    {
        public short ID { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string Description { get; set; }

        public List<int> Instances { get; set; } = new List<int>();
    }

    public class Log_ : ICacheEntity
    {
        public long ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Received { get; set; }
        public byte Level { get; set; }
        public byte Severity { get; set; }
        public int InstanceID { get; set; }
        public byte Format { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
    }

    public class KeepAlive_ : ICacheEntity
    {
        public long ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Received { get; set; }
        public int InstanceID { get; set; }
    }

    public class Metric_
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int InstanceID { get; set; }
        public int Aggregation { get; set; }

        public long RangeHeadID { get; set; }
        public long RangeTailID { get; set; }

        public DateTime ActualInterval { get; set; }
        public long ActualID { get; set; }
    }

    public class Measure_
    {
        public long ID { get; set; }
        public double Value { get; set; }

        internal bool HasValue { get; set; }
    }

    public class Group_
    {
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string Description { get; set; }
    }

    public class MeasureResponse
    {
        public int MetricId { get; set; }
        public DateTime Interval { get; set; }
        public double Value { get; set; }
    }

    public class WindowResponse
    {
        public int MetricId { get; set; }
        public double Value { get; set; }
    }

    public class MetricHistoryRequestParameters
    {
        public int Amount { get; set; } = 12;
        public int Skip { get; set; } = 0;
    }

    public class MetricHistoryResponse
    {
        public int MetricId { get; set; }
        public DateTime Interval { get; set; }
        public double[] Values { get; set; }
    }

    public class KeepAliveStatus
    {
        public short SourceID { get; set; }
        public int InstanceID { get; set; }

        public string SourceName { get; set; }
        public string InstanceName { get; set; }
        public string DisplayName { get; set; }

        public DateTime Created { get; set; }
        public DateTime Received { get; set; }

        public bool StatusOK { get; set; }
    }

    public class LogRequest
    {
        public short[] Groups { get; set; } = new short[0];
        public int[] Instances { get; set; } = new int[0];

        public long? LastId { get; set; }
        public byte? SeverityCutoff { get; set; }
        public byte? Level { get; set; }
        public int? Top { get; set; }
    }

    public class KeepAliveRequest
    {
        public short[] Groups { get; set; } = new short[0];
        public int[] Instances { get; set; } = new int[0];
    }

    public class MetricRequest
    {
        public short[] Groups { get; set; } = new short[0];
        public int[] Instances { get; set; } = new int[0];
    }
}
