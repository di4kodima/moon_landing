using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;
using static UiController;

public class Model : MonoBehaviour
{
    [SerializeField]
    GameStateMashine StateM;
    [SerializeField]
    private UiReader _uiReader;

    #region Params

    public bool IsIntegrate = false;
    public double jetM
    {
        get { return _jetM; }
        set
        {
            FuelFlowChanged?.Invoke();
            if (value <= MaxFF && value >= 0)
                _jetM = value;
        }
    }

    int FPS;

    public double Time;
    public double angle;
    public double JetV;
    public double StartH;
    public double G;
    public double Rm;
    public double Fm;
    public double MaxFF;
    public double delta;
    
    private double _jetM = 2;

    public vect RocketPos;
    public vect LandV;
    public vect v = new vect(0, 0, 0);
    public vect ac = new vect(0, 0, 0);
    #endregion

    public event Action OnPhisicFrame;
    public event Action FuelFlowChanged;

    private void Awake()
    {
        GameStateMashine.Start += OnStart;
        GameStateMashine.Continue += OnContinue;
        GameStateMashine.Stop += OnPause;
        GameStateMashine.TurnOf += OnTurnOf;
        GameStateMashine.StartClk += OnClkStart;
    }

    private void OnStart()
    {
        if (_uiReader.ReadData(out Fm, out Rm, out JetV, out double jetm, out StartH, out G, out MaxFF, out LandV, out delta))
        {
            RocketPos.y = StartH;
            v = new vect(0,0,0);
            ac = v;
            angle = 0;
            Time = 0;
            StartCoroutine(PhysicFrame((float)delta));
        }
    }

    private void OnClkStart() 
    {

        if (_uiReader.ReadData(out Fm, out Rm, out JetV, out double jetm, out StartH, out G, out MaxFF, out LandV, out delta))
        {
            RocketPos.y = StartH;
            G = 0;
            v = new vect(0, 0, 0);
            ac = v;
            angle = 0;
            Time = 0;
            StartCoroutine(PhysicFrame((float)delta));
        }
    }

    private void OnContinue()
    {
        StartCoroutine(PhysicFrame((float)delta));
    }

    private void OnPause()
    {
        StopAllCoroutines();
    }
    private void OnTurnOf()
    {
        StopAllCoroutines();
    }

    IEnumerator PhysicFrame(float delta)
    {
        while (true)
        {
            Time += delta;
            if (Fm < jetM * delta)
            {
                jetM = Fm / delta;
            }
            if (Fm == 0)
                jetM = 0;
            v = Verle(out ac, v, delta, JetV, jetM, Rm + Fm);
            Fm = Fm - jetM * delta;

            OnPhisicFrame?.Invoke();

            yield return new WaitForSeconds(1/60);
        }

        vect Verle(out vect a, vect v, double delta, double jetV, double jetM, double m)
        {
            a = calculate_accel(jetV, jetM, m, delta);
            RocketPos = RocketPos + v * delta + 0.5 * a * delta * delta;
            v = v + 0.5 * (ac + a) * delta;
            return v;
        }

        vect Eiler(out vect a, vect v, double delta, double jetV, double jetM, double m)
        {
            a = calculate_accel(jetV, jetM, m, delta);
            v = v + a * delta;
            RocketPos = RocketPos + v * delta;
            return v;
        }

        vect calculate_accel(double jetV, double jetM, double Rm, double delta)
        {
            vect accel = new()
            {
                x = (jetV * jetM / Rm) * -Math.Sin((Math.PI / 180) * angle),
                y = (jetV * jetM / Rm) * Math.Cos((Math.PI / 180) * angle) - G
            };
            return accel;
        }
    }

    public double CialkivskiyByStep()
    {
        return v.y + 2.3f * (JetV * Math.Log10((Rm + Fm) / (Rm + Fm - jetM)));
    }
    public double Cialkivskiy()
    {
        return v.y + 2.3f * (JetV * Math.Log10((Rm + Fm) / (Rm)));
    }
}
