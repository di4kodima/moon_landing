using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelRunOut : MonoBehaviour
{
    [SerializeField] Model model;


    private void Awake()
    {
        model.FuelRanOut += OnFuelRunOut;
    }

    private void OnFuelRunOut() 
    {
        GameStateMashine.TurnOfGame();
    }
}
