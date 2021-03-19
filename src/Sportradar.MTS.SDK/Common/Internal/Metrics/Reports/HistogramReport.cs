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

            var hist = item.Value;
            var u = _fh.U(item.Unit, TimeUnit.Seconds, true);
            QueueAdd("Min", $"{_fh.Dec((double) hist.Min)} {u}");
            QueueAdd("Max", $"{_fh.Dec((double) hist.Max)} {u}");
            QueueAdd("Mean", $"{_fh.Dec((double) hist.Mean)} {u}");
            QueueAdd("75%", $"{_fh.Dec((double) hist.Percentile75)} {u}");
            QueueAdd("95%", $"{_fh.Dec((double) hist.Percentile95)} {u}");
            QueueAdd("98%", $"{_fh.Dec((double) hist.Percentile98)} {u}");
            QueueAdd("99%", $"{_fh.Dec((double) hist.Percentile99)} {u}");
            QueueAdd("99.9%", $"{_fh.Dec((double) hist.Percentile999)} {u}");
            QueueAdd("StdDev", $"{_fh.Dec((double) hist.StdDev)} {u}");
            QueueAdd("Median", $"{_fh.Dec((double) hist.Median)} {u}");
            QueueAdd("Last value", $"{_fh.Dec((double) hist.LastValue)} {u}");
        }
    }
}
