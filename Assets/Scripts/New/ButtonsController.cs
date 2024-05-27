using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsController : MonoBehaviour
{
    #region SerializedFields

    #endregion

    public void OnStartButton_Click()
    {
        GameStateMashine.StartGame();
    }

    public void OnEndButton_Click() 
    {
        GameStateMashine.TurnOfGame();
    }

    public void OnPauseButton_Click()
    {
        GameStateMashine.StayOnPauseGame();
    }
}
