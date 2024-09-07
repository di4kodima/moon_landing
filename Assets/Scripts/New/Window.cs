using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{

    private void Awake()
    {
        GameStateMashine.Start += Hide;
        GameStateMashine.StartClk += Hide;
        GameStateMashine.Continue += Hide;
        GameStateMashine.Stop += Show; 
        GameStateMashine.TurnOf += Show;
    }

    private void OnDestroy()
    {
        GameStateMashine.Start -= Hide;
        GameStateMashine.StartClk -= Hide;
		GameStateMashine.Continue -= Hide;
		GameStateMashine.Stop -= Show;
        GameStateMashine.TurnOf -= Show;
    }


    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Swith()
    {
        gameObject.SetActive(!gameObject.active);
    }
}
