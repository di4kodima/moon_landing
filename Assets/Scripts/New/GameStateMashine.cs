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

    private static State state;

    public static void StartGame()
    {
        if (state == State.Process)
            return;
        if (state == State.Start || state == State.Of)
        {
            state = State.Process;
            Start?.Invoke();
        }
        if (state == State.Pause)
        {
            state = State.Process;
            Continue?.Invoke();
        }
    }
    public static void TurnOfGame()
    {
        Stop?.Invoke();
        state = State.Of;
    }

    public static void StayOnPauseGame()
    {
        if (state == State.Process)
        {
            state = State.Pause;
            Stop?.Invoke();
        }
    }
}
