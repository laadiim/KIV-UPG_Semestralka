using static LiveChartsCore.LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.VisualElements;
using LiveChartsCore.SkiaSharpView.WinForms;
using LiveChartsCore.VisualElements;
using UPG_SP_2024.Interfaces;
using UPG_SP_2024.Primitives;

namespace UPG_SP_2024
{
    /// <summary>
    /// trida karty s grafem
    /// </summary>
    internal class GraphTab : TabPage
    {
        private List<IProbe> probes;
        private List<ISeries> series = new List<ISeries>();
        private CartesianChart chart;

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="probe">instance sondy</param>
        public GraphTab(IProbe probe)
        {
            this.probes = new List<IProbe>();
            this.probes.Add(probe);
            InitializeChart();
            //UpdateChart();
        }

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="probes">seznam sond</param>
        public GraphTab(List<IProbe> probes)
        {
            this.probes = probes;
            InitializeChart();
            //UpdateChart();
        }

        /// <summary>
        /// nacte data do grafu
        /// </summary>
        private void GetData()
        {
            for (int j = 0; j < this.probes.Count; j++)
            {
                Probe probe = (Probe)this.probes[j];
                ObservablePoint[] values = new ObservablePoint[probe.values.Count];
                for (int i = 0; i < probe.values.Count; i++)
                {
                    float v = probe.values[i].Item2 * 100;
                    values[i] = new ObservablePoint(probe.values[i].Item1, Math.Round(v, 2));
                }
                this.series[j].Values = values;
            }
            
        }
        
        /// <summary>
        /// inicializace grafu
        /// </summary>
        private void InitializeChart()
        {
            for (int j = 0; j < this.probes.Count; j++)
            {
                Probe probe = (Probe)this.probes[j];
                LineSeries<ObservablePoint> s = new LineSeries<ObservablePoint>
                {
                    Values = [],
                    GeometrySize = 0,
                    Fill = null,
                    Name = $"Probe {probe.id}"
                };

                this.series.Add(s);
            }

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
                        MaxLimit = 1000,
                        MinLimit = 0,
                        MinStep = 1,
                }},
                LegendPosition = LiveChartsCore.Measure.LegendPosition.Bottom,
            };
            Controls.Add(chart);
        }

        /// <summary>
        /// nacte nova data
        /// </summary>
        public void UpdateChart()
        {
            GetData();
            chart.Series = this.series;
        }
    }
}
