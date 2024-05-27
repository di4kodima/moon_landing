using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Model _model;

    private void Awake()
    {
        _model.OnPhisicFrame += OnPhisicFrame;
    }

    private void OnPhisicFrame()
    {
        if(_model.RocketPos.y < 0)
            GameStateMashine.TurnOfGame();
    }
}
