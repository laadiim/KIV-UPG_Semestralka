using static LiveChartsCore.LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.VisualElements;
using UPG_SP_2024.Interfaces;
using UPG_SP_2024.Primitives;

namespace UPG_SP_2024
{
    internal class GraphTab : TabPage
    {
        private Probe probe;
        private int index;
        private ISeries[] series;
        private CartesianChart chart;

        public GraphTab(IProbe probe, int index)
        {
            this.probe = (Probe)probe ?? throw new ArgumentNullException(nameof(probe));
            this.index = index;
            InitializeChart();
            //UpdateChart();
        }

        private void InitializeChart()
        {
            int[] times = new int[probe.values.Count];
            float[] values = new float[probe.values.Count];
            for (int i = 0; i < probe.values.Count; i++)
            {
                times[i] = probe.values[i].Item1;
                values[i] = probe.values[i].Item2;
            }

            series =
            [
                new LineSeries<float>
                {
                    Values = values,
                }
            ];

            this.chart = new CartesianChart
            {
                Series = series,
                Title = new LabelVisual
                {
                    Text = "My chart title",
                    TextSize = 25,
                    Padding = new LiveChartsCore.Drawing.Padding(15)
                },
                Dock = DockStyle.Fill,
            };
            Controls.Add(chart);
        }

        public void UpdateChart()
        {
            int[] times = new int[probe.values.Count];
            float[] values = new float[probe.values.Count];
            for (int i = 0; i < probe.values.Count; i++)
            {
                times[i] = probe.values[i].Item1;
                values[i] = probe.values[i].Item2;
            }

            series[0].Values = values;
            chart.Series = this.series;
        }
    }
}
