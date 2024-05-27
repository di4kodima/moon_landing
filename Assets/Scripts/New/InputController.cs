using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    Model Model;

    float RotationSpeed = 1;
    float FuelFlowStep = 0.5f;

    private void Awake()
    {
        GameStateMashine.Start += OnGameStartWork;
        GameStateMashine.Continue += OnGameStartWork;
        GameStateMashine.Stop += OnGameStopWork;
        GameStateMashine.TurnOf += OnGameStopWork;
    }

    private void OnGameStartWork()
    {
        StartCoroutine(UpdateInput());
    }

    private void OnGameStopWork()
    {
        StopAllCoroutines();
    }

    private IEnumerator UpdateInput()
    {
        while (true)
        {
            Model.jetM += 10 * Input.GetAxis("Mouse ScrollWheel");
            Model.jetM += FuelFlowStep * Input.GetAxis("Vertical");
            Model.angle -= RotationSpeed * Input.GetAxis("Horizontal");

            yield return null;
        }
    }
}
