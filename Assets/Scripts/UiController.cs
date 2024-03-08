using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField]
    TMP_InputField Fuel;
    [SerializeField]
    TMP_InputField RocketMass;
    [SerializeField]
    TMP_InputField jetSpeed;
    [SerializeField]
    TMP_InputField jetMass;
    [SerializeField]
    TMP_InputField HighStart;
    [SerializeField]
    TMP_InputField Gravity;
    [SerializeField]
    TMP_InputField MaxFuelFlow;
    [SerializeField]
    TMP_InputField LandingSpeed;
    [SerializeField]
    TMP_InputField delta_input;

    [SerializeField]
    TMP_Text out_speed;

    [SerializeField]
    TMP_Text out_accel;


    double Rm;
    double Fm;
    double jetM;
    double jetV;
    double StartH;
    double G = 2.6;
    double LandV;
    double MaxFF;
    double Hstart;
    double Lv;
    int Fps;

    vect accel;
    public double trust;
    public vect v;
    public double h;
    public double a;
    public double delta;

    private Status status = Status.off;

    enum Status {
        off,
        work,
        pause
    }

    [SerializeField]
    private Rocket rocket;

    public event Action GraphicFrameUpdate;

    public bool ReadData(out double Fm, out double Rm, out double JetS, out double JetM, out double Hstart, out double G,out double MaxFF, out double Ls, out double delta)
    {
        bool res = true;
        if (!double.TryParse(Gravity.text ,out G)) res = false;
        if (!double.TryParse(Fuel.text, out Fm)) res = false;
        if (!double.TryParse(RocketMass.text, out Rm)) res = false;
        if (!double.TryParse(jetSpeed.text, out JetS)) res = false;
        if (!double.TryParse(jetMass.text, out JetM)) res = false;
        if (!double.TryParse(HighStart.text, out Hstart)) res = false;
        if (!double.TryParse(MaxFuelFlow.text, out MaxFF)) res = false;
        if (!double.TryParse(LandingSpeed.text, out Ls)) res = false;
        if (!double.TryParse(delta_input.text, out delta)) res = false;
        return res;
    }


    public void SceneStart()
    {
        if (status == Status.off || status == Status.pause)
            if(ReadData(out Fm, out Rm, out jetV, out jetM, out Hstart, out G, out MaxFF, out Lv, out delta))
            {
                status = Status.work;
                StartCoroutine(PhysicFrame((float)delta));
                StartCoroutine(GraphicFrame());
            }
    }

    public void ScenePause()
    {
        if (status == Status.pause) {
            SceneStart();
            return;
        }
        status = Status.pause;
    }

    IEnumerator PhysicFrame(float delay)
    {
        if (status == Status.off || status == Status.pause) {
            yield break;
        }
        Rm = Rm - jetM * delta;
        out_speed.text = Verle().y.ToString();
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(PhysicFrame(delay));
    }

    IEnumerator GraphicFrame() {
        yield return new WaitForSeconds(1/60);
        GraphicFrameUpdate?.Invoke();
        yield return StartCoroutine(GraphicFrame());
    }

    vect Verle()
    {
        vect res = new() {
            x = rocket.transform.position.x,
            y = rocket.transform.position.y
        };
       
        vect a = calculate_accel(jetV, jetM, Rm, delta);
        out_accel.text = a.y.ToString();

       res.y = res.y + rocket.V * delta + 0.5 * a.y * math.pow(delta, 2);
        rocket.V = rocket.V + 0.5 * (rocket.a * a.y) * delta;

        rocket.a = a.y;
        //rocket.transform.position = new Vector3((float) res.x, (float) res.y, 0);
        return res;
    }

    vect calculate_accel(double jetV, double jetM, double Rm, double delta) {
        vect res = new()
        {
            y = -((jetV * (jetM * delta) / Rm) - G)
        };
        return res;
    }

    public struct vect
    {
        public double x, y, z;
        public double modul()
        { return Math.Sqrt(x * x + y * y + z * z); }
        public double Mod
        {
            get { return Math.Sqrt(x * x + y * y + z * z); }
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
}