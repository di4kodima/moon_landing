using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketParticles : MonoBehaviour
{
    [SerializeField] private Model model;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private int MaxParticlesCount = 500;

    private float MaxFF;
    private float FuelFlow;


    private void Awake()
    {
        GameStateMashine.Start += particleSystem.Play;
        GameStateMashine.StartClk += particleSystem.Play;
        GameStateMashine.Stop += particleSystem.Pause;
        GameStateMashine.TurnOf += particleSystem.Stop;

        model.FuelFlowChanged += FuelFlowChanged;
    }

    private void FuelFlowChanged()
    {
        particleSystem.maxParticles = (int)(MaxParticlesCount * (model.jetM / model.MaxFF));
    }
}