using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMashine
{
    public enum State { Start, Process, Pause, Of }

    static public event Action OnStateChanged;

    static public event Action Start;

    static public event Action Continue;

    static public event Action Stop;

    static public event Action TurnOf;

    static private GameStateMashine _instence;

    State _state;

    void StartGame()
    {
        if(_state == State.Start)
        {
            _state = State.Process;
        }
    }
    void StopGame()
    {
    }

    public void StayOnPause()
    { 
    }
}
