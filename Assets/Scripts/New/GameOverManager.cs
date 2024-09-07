using Assets.Scripts.New;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using XCharts.Runtime;


public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Model _model;
    [SerializeField] private TMP_Text ResuultLabel; 
    [SerializeField] FlightDataRecorder FlightDataRecorder;
	private void Awake()
    {
        _model.OnPhisicFrame += OnPhisicFrame;
        GameStateMashine.Start += GameStart;
        GameStateMashine.StartClk += GameStart;
    }

    private void OnDestroy()
    {
        _model.OnPhisicFrame -= OnPhisicFrame;
        GameStateMashine.Start -= GameStart;
        GameStateMashine.StartClk -= GameStart;
    }
    private void GameStart()
    {
        ResuultLabel.text = "";
    }

    private void OnPhisicFrame()
    {
        if (_model.RocketPos.y < 0)
        {
            GameStateMashine.TurnOfGame();
            if (Math.Abs(_model.v.x) > _model.LandV.x || Math.Abs(_model.v.y) > _model.LandV.y || Math.Abs(Mathf.Sin((float)_model.angle * Mathf.PI/180f)) > 0.5)
                ResuultLabel.text = "Миссия потерпела крушение! \n-25млрд$";
            else 
            { 
                ResuultLabel.text = "Посадка прошла успешно!";
                FlightResult flightResult = new FlightResult(DateTime.Now, FlightDataRecorder.Tarray.Last(), _model.Fm, _model.StartFm);
				LevelStats.History.Add(flightResult);
                LevelStats.SaveSetings();

                Debug.Log(LevelStats.History.Count());
                Debug.Log($"Лучшее время: {LevelStats.BestTimeFlight().FlightTime}, по топливу: ");
                foreach (var flight in LevelStats.History)
                    Debug.Log($"Дата полета: {flight.dateTime.ToShortDateString()}" +
                        $", время полета: {flight.FlightTime}," +
                        $"Топлива сохранено: {flight.RemainingFuel}");
                
            }
        } 
    }
}