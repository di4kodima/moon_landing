using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private Model _model;

    private void Awake()
    {
        _model.OnPhisicFrame += OnPhisicFrame;
    }

    private void OnPhisicFrame()
    {
        if (_model.RocketPos.y < 0)
            GameStateMashine.Instance.GameState = GameStateMashine.State.Of;
    }
}
