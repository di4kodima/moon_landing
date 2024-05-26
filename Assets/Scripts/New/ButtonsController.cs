using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    #region SerializedFields

    #endregion

    public void OnStartButton_Click()
    {
        GameStateMashine.Instance.GameState = GameStateMashine.State.Process;
    }
    public void OnEndButton_Click() 
    {
        GameStateMashine.Instance.GameState = GameStateMashine.State.Of;
    }

    public void OnPauseButton_Click()
    {
        GameStateMashine.Instance.GameState = GameStateMashine.State.Pause;
    }
}
