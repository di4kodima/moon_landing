using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

public class ChartFiller : MonoBehaviour
{
    [SerializeField] Model model;

    [SerializeField] FlightDataRecorder FDRecorder;

    [SerializeField] LineChart VChart;
    [SerializeField] LineChart AChart;
    [SerializeField] LineChart FChart;
    [SerializeField] LineChart FFChart;
    [SerializeField] LineChart HChart;
    [SerializeField] LineChart ClkChart;

    private void Awake()
    {
        GameStateMashine.Start += OnGameStart;
        GameStateMashine.StartClk += OnGameStart;
        GameStateMashine.TurnOf += OnStopFill;
        GameStateMashine.Stop += OnStopFill;
    }

    private void OnGameStart() 
    {
        VChart.series[0].data.Clear();
        FChart.series[0].data.Clear();
        HChart.series[0].data.Clear();
        AChart.series[0].data.Clear();
        FFChart.series[0].data.Clear();
        ClkChart.series[0].data.Clear();

        StartCoroutine(GraphicFrame());
    }

    private void OnStopFill()
    {
        //FillChart(FDRecorder.Tarray,FDRecorder.Farray, FChart);

        StopAllCoroutines();
    }

    IEnumerator GraphicFrame()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);

            AddInChart(model.Time, model.v.y, VChart, 0);
            AddInChart(model.Time, model.Fm, FChart, 0);
            AddInChart(model.Time, model.RocketPos.y, HChart, 0);
            AddInChart(model.Time, model.ac.y, AChart, 0);
            AddInChart(model.Time, model.jetM, FFChart, 0);
            AddInChart(model.Time, model.CialkivskiyByStep(), ClkChart, 0);

        }
    }

    void FillChart(List<double> x, List<double> y, LineChart chart)
    {
        Serie serie = new();
        SerieData serieData = new();
        for (int i = 0; i <= x.Count; i++)
        {
            serieData.data.Clear();
            serieData.data.Add(x[i]);
            serieData.data.Add(y[i]);
            serie.AddSerieData(serieData);
        }
        chart.series.Clear();
        chart.series.Add(serie);
    }

    void AddInChart(double x, double y, LineChart chart, int SerieNumber)
    {
        SerieData serieData = new();
        serieData.data.Add(x);
        serieData.data.Add(y);
        chart.series[0].AddSerieData(serieData);
    }
}
