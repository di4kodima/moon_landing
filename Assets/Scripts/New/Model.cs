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

    public bool ParceData = true;

    public double Time;
    public double angle;
    public double JetV;
    public double StartH;
    public double StartFm;
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
    public event Action FuelRanOut;

    private void Awake()
    {
        GameStateMashine.Start += OnStart;
        GameStateMashine.Continue += OnContinue;
        GameStateMashine.Stop += OnPause;
        GameStateMashine.TurnOf += OnTurnOf;
        GameStateMashine.StartClk += OnClkStart;
    }

    private void OnDestroy()
    {

        GameStateMashine.Start -= OnStart;
        GameStateMashine.Continue -= OnContinue;
        GameStateMashine.Stop -= OnPause;
        GameStateMashine.TurnOf -= OnTurnOf;
        GameStateMashine.StartClk -= OnClkStart;
    }

    private void OnStart()
    {
        if (!ParceData)
        {
            StartCoroutine(PhysicFrame((float)delta));
            RocketPos.y = StartH;
			v = new vect(0, 0, 0);
			ac = v;
			return;
        }
		if (_uiReader.ReadData(out Fm, out Rm, out JetV, out double jetm, out StartH, out G, out MaxFF, out LandV, out delta) )
        {
            RocketPos.y = StartH;
            StartFm = Fm;
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
            jetM= MaxFF;
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
            if (Fm < jetM * delta)
            {
                jetM = Fm / delta;
            }
            if (Fm == 0) 
            {
                jetM = 0;
                FuelRanOut?.Invoke();
            }
            v = Verle(out ac, v, delta, JetV, jetM, Rm + Fm);
            Fm = Fm - jetM * delta;

            OnPhisicFrame?.Invoke();

            Time += delta;
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

    public double TsiolkovskyByStep()
    {
        return v.y + 2.3f * (JetV * Math.Log10((Rm + Fm) / (Rm + Fm - jetM)));
    }

    public double Tsiolkovsky()
    {
        return v.y + 2.3f * (JetV * Math.Log10((Rm + Fm) / (Rm)));
    }
    
}
public struct vect
{
    public double x, y, z;

    public static explicit operator Vector3(vect v)
    {
        Vector3 res = new Vector3((float)v.x, (float)v.y, (float)v.z);
        return res;
    }

    public static implicit operator vect(Vector3 v)
    {
        vect res = new vect(v.x, v.y, v.z);
        return res;
    }

    public double modul()

    { return Math.Sqrt(x * x + y * y + z * z); }
    public double Mod
    {
        get { return Math.Sqrt(x * x + y * y + z * z); }
    }

    public override string ToString()
    {
        return $"({string.Format("{0:f2}", x)}, {string.Format("{0:f2}", y)}, {string.Format("{0:f2}", z)})";
    }

    public vect(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public static vect operator +(vect a, vect b)
    {
        vect c = new vect();
        c.x = a.x + b.x;
        c.y = a.y + b.y;
        return c;
    }
    public static double operator *(vect a, vect b)
    {
        return a.x * b.x + a.y * b.y;
    }
    public static vect operator *(double c, vect a)
    {
        vect h = new vect();
        h.x = a.x * c;
        h.y = a.y * c;
        return h;
    }
    public static vect operator *(vect a, double c)
    {
        vect h = new vect();
        h.x = a.x * c;
        h.y = a.y * c;
        return h;
    }
    public static vect operator /(vect a, double c)
    {
        vect h = new vect();
        h.x = a.x / c;
        h.y = a.y / c;
        return h;
    }
    public static vect operator -(vect a, vect b)
    {
        vect c = new vect();
        c.x = a.x - b.x;
        c.y = a.y - b.y;
        return c;
    }
    public static vect operator &(vect a, vect b)
    {
        vect c = new vect();
        c.x = a.y * b.z - a.z * b.y;
        c.y = a.z * b.x - a.x * b.z;
        c.z = a.x * b.y - a.y * b.x;
        return c;
    }
}
