using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;


public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Model _model;
    [SerializeField] private TMP_Text ResuultLabel;
    private void Awake()
    {
        _model.OnPhisicFrame += OnPhisicFrame;
        GameStateMashine.Start += GameStart;
        GameStateMashine.StartClk += GameStart;
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
            if (Math.Abs(_model.v.x) > _model.LandV.x || Math.Abs(_model.v.y) > _model.LandV.y || Math.Abs(_model.angle) > 10)
                ResuultLabel.text = "Миссия потерпела крушение! \n-25млрд$";
            else ResuultLabel.text = "Посадка прошла успешно!";
        } 
    }
}