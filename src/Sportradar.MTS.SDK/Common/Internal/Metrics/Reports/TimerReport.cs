﻿/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Linq;
using log4net;
using Metrics.MetricData;

namespace Sportradar.MTS.SDK.Common.Internal.Metrics.Reports
{
    internal class TimerReport : BaseReport<TimerValueSource>
    {
        private readonly FormatHelper _fh;

        public TimerReport(string context = null, ILog log = null, MetricsReportPrintMode printMode = MetricsReportPrintMode.Normal, int decimals = 2)
            : base(context, log, printMode, decimals)
        {
            _fh = new FormatHelper(Decimals, MetricsReportPrintMode.Compact);
        }

        public override void ReportList(IEnumerable<TimerValueSource> items)
        {
            if (items == null)
            {
                return;
            }
            var listItems = items as IList<TimerValueSource> ?? items.ToList();
            if (!listItems.Any())
            {
                return;
            }

            QueueAdd(FormatHelper.SectionName("Timers", ContextName));

            base.ReportList(listItems);
        }

        public override void Report(TimerValueSource item, bool suppressPrint = false)
        {
            if (!suppressPrint)
            {
                QueueAdd(FormatHelper.SectionName("Timer", ContextName));
                SetLog(item.Name);
                SetContextName(item.Name);
            }

            PrintSelector(item);

            if (!suppressPrint)
            {
                PrintQueue();
            }
        }

        protected override void Print(TimerValueSource item)
        {
            QueueAdd("Active sessions", $"{item.Value.ActiveSessions}");
            QueueAdd("Total time", $"{item.Value.TotalTime} {_fh.Time(item.DurationUnit)}");
            QueueAdd("Count", $"{item.Value.Rate.Count} {item.Unit}");
            QueueAdd("Mean value", $"{_fh.Dec(item.Value.Rate.MeanRate)} {_fh.U(item.Unit, item.RateUnit, false)}");
            QueueAdd("1 minute rate", $"{_fh.Dec(item.Value.Rate.OneMinuteRate)} {_fh.U(item.Unit, item.RateUnit, false)}");
            QueueAdd("5 minute rate", $"{_fh.Dec(item.Value.Rate.FiveMinuteRate)} {_fh.U(item.Unit, item.RateUnit, false)}");
            QueueAdd("15 minute rate", $"{_fh.Dec(item.Value.Rate.FifteenMinuteRate)} {_fh.U(item.Unit, item.RateUnit, false)}");
        }

        protected override void PrintFull(TimerValueSource item)
        {
            Print(item);

            PrintHistogram(item.Value.Histogram, item.Unit, item.RateUnit, _fh);
        }
    }
}
