using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    Model Model;

    float RotationSpeed = 1;
    float FuelFlowStep = 0.5f;
    private void Update()
    {
        if (GameStateMashine.Instance.GameState != GameStateMashine.State.Process)
            return;
        Model.jetM += 10 * Input.GetAxis("Mouse ScrollWheel");
        Model.jetM += FuelFlowStep * Input.GetAxis("Vertical");
        Model.angle -= RotationSpeed * Input.GetAxis("Horizontal");
    }
}
