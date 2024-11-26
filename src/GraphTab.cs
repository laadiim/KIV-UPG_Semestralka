using static LiveChartsCore.LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.Defaults;
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
        private ISeries[] series = new ISeries[]
        {
            new LineSeries<ObservablePoint>
            {
                Values = [],
                GeometrySize = 0,
                Fill = null,
            }
        };
        private CartesianChart chart;

        public GraphTab(IProbe probe, int index)
        {
            this.probe = (Probe)probe ?? throw new ArgumentNullException(nameof(probe));
            this.index = index;
            InitializeChart();
            //UpdateChart();
        }

        private void GetData()
        {
            ObservablePoint[] values = new ObservablePoint[probe.values.Count];
            for (int i = 0; i < probe.values.Count; i++)
            {
                float v = probe.values[i].Item2 * 100;
                values[i] = new ObservablePoint(probe.values[i].Item1 ,Math.Round(v, 2));
            }
            series[0].Values = values;
        }
        private void InitializeChart()
        {
            GetData();
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
                YAxes = new Axis[]{
                new Axis(){
                        Name = "Intenzita el. pole [10 GN/C]",
                        MinLimit = 0,
                        MinStep = 1,
                }},
            };
            Controls.Add(chart);
        }

        public void UpdateChart()
        {
            GetData();
            chart.Series = this.series;
        }
    }
}
