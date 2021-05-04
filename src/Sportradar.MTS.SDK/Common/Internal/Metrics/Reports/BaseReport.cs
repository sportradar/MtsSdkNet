/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System;
using System.Collections.Generic;
using log4net;
using Metrics;
using Metrics.MetricData;

namespace Sportradar.MTS.SDK.Common.Internal.Metrics.Reports
{
    internal abstract class BaseReport<T>
    {
        private string _contextName;
        private string _logFormat;
        protected readonly int Decimals;
        protected readonly MetricsReportPrintMode PrintMode;

        protected ILog Log;
        protected string ContextName => _contextName;
        protected readonly Queue<KeyValuePair<string, string>> LogQueue;

        public string LogFormat => _logFormat;

        protected BaseReport(string context = null, ILog log = null, MetricsReportPrintMode printMode = MetricsReportPrintMode.Normal, int decimals = 2)
        {
            Log = log;
            PrintMode = printMode;
            Decimals = decimals;
            _contextName = context;
            LogQueue = new Queue<KeyValuePair<string, string>>();
        }

        public virtual void ReportList(IEnumerable<T> items)
        {
            if (items == null)
            {
                return;
            }
            foreach (var md in items)
            {
                Report(md, true);
            }

            PrintQueue();
        }

        public abstract void Report(T item, bool suppressPrint = false);

        protected virtual void SetContextName(string context)
        {
            _contextName = context;
        }

        protected virtual void SetFormat(string format)
        {
            _logFormat = format;
        }

        protected virtual void SetLog(string loggerName)
        {
            if (Log == null && !string.IsNullOrEmpty(loggerName))
            {
                Log = LogManager.GetLogger(loggerName);
            }
        }

        protected virtual void QueueAdd(string key, string value = null)
        {
            LogQueue.Enqueue(new KeyValuePair<string, string>(key, value));
        }

        protected virtual void PrintQueue()
        {
            lock (LogQueue)
            {
                Log.Info(string.Empty);
                while (LogQueue.Count > 0)
                {
                    var kv = LogQueue.Dequeue();
                    if (string.IsNullOrEmpty(kv.Value))
                    {
                        var space = (kv.Key.Length / 2) + 30;
                        var text = "{0," + space + "}";
                        Log.Info(string.Format(text, kv.Key));
                    }
                    else
                    {
                        Log.Info($"{kv.Key,25} = {kv.Value}");
                    }
                }
                Log.Info(string.Empty);
            }
        }

        protected virtual void PrintSelector(T item)
        {
            switch (PrintMode)
            {
                case MetricsReportPrintMode.Normal:
                    Print(item);
                    break;
                case MetricsReportPrintMode.Minimal:
                    PrintMinimal(item);
                    break;
                case MetricsReportPrintMode.Compact:
                    PrintCompact(item);
                    break;
                case MetricsReportPrintMode.Full:
                    PrintFull(item);
                    break;
                default:
                    throw new ArgumentException("Unknown MetricsReportPrintMode");
            }
        }

        protected abstract void Print(T item);

        protected virtual void PrintMinimal(T item)
        {
            Print(item);
        }

        protected virtual void PrintCompact(T item)
        {
            Print(item);
        }

        protected virtual void PrintFull(T item)
        {
            Print(item);
        }

        protected virtual void PrintHistogram(HistogramValue histogram, Unit unit, TimeUnit timeUnit, FormatHelper formatHelper)
        {
            var u = formatHelper.U(unit, timeUnit, true);
            QueueAdd("Min", $"{formatHelper.Dec(histogram.Min)} {u}");
            QueueAdd("Max", $"{formatHelper.Dec(histogram.Max)} {u}");
            QueueAdd("Mean", $"{formatHelper.Dec(histogram.Mean)} {u}");
            QueueAdd("75%", $"{formatHelper.Dec(histogram.Percentile75)} {u}");
            QueueAdd("95%", $"{formatHelper.Dec(histogram.Percentile95)} {u}");
            QueueAdd("98%", $"{formatHelper.Dec(histogram.Percentile98)} {u}");
            QueueAdd("99%", $"{formatHelper.Dec(histogram.Percentile99)} {u}");
            QueueAdd("99.9%", $"{formatHelper.Dec(histogram.Percentile999)} {u}");
            QueueAdd("StdDev", $"{formatHelper.Dec(histogram.StdDev)} {u}");
            QueueAdd("Median", $"{formatHelper.Dec(histogram.Median)} {u}");
            QueueAdd("Last value", $"{formatHelper.Dec(histogram.LastValue)} {u}");
        }
    }
}
