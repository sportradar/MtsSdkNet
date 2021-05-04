/*
 * Copyright (C) Sportradar AG. See LICENSE for full license governing this code
 */
using System.Collections.Generic;
using System.Linq;
using log4net;
using Metrics;
using Metrics.MetricData;

namespace Sportradar.MTS.SDK.Common.Internal.Metrics.Reports
{
    internal class HistogramReport : BaseReport<HistogramValueSource>
    {
        private readonly FormatHelper _fh;

        public HistogramReport(string context = null, ILog log = null, MetricsReportPrintMode printMode = MetricsReportPrintMode.Normal, int decimals = 2)
            : base(context, log, printMode, decimals)
        {
            _fh = new FormatHelper(Decimals, MetricsReportPrintMode.Compact);
        }

        public override void ReportList(IEnumerable<HistogramValueSource> items)
        {
            var listItems = items as IReadOnlyList<HistogramValueSource> ?? items.ToList();
            if (!listItems.Any())
            {
                return;
            }

            QueueAdd(FormatHelper.SectionName("Histograms", ContextName));

            base.ReportList(listItems);
        }

        public override void Report(HistogramValueSource item, bool suppressPrint = false)
        {
            if (!suppressPrint)
            {
                QueueAdd(FormatHelper.SectionName("Histogram", ContextName));
                SetLog(item.Name);
                SetContextName(item.Name);
            }

            PrintSelector(item);

            if (!suppressPrint)
            {
                PrintQueue();
            }
        }

        protected override void Print(HistogramValueSource item)
        {
            QueueAdd($"{item.Name}");
            QueueAdd("Sample size", $"{item.Value.SampleSize}");
            QueueAdd("Count", $"{item.Value.Count} {item.Unit}");
        }

        protected override void PrintFull(HistogramValueSource item)
        {
            Print(item);

            PrintHistogram(item.Value, item.Unit, TimeUnit.Seconds, _fh);
        }
    }
}
