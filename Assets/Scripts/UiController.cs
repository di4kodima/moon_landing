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
    [SerializeField] 
    TMP_Text out_speedd;
    [SerializeField]
    TMP_Text out_height;
    [SerializeField]
    TMP_Text out_Fuel;
    [SerializeField]
    TMP_Text out_fuelFlow;

    [SerializeField]
    Rocket rocket;
    /// <summary>
    /// Масса корпуса ракеты
    /// </summary>
    double Rm;
    /// <summary>
    /// Масса топлива на ракете
    /// </summary>
    double Fm;
    /// <summary>
    /// Масса выбрасываемого вещества
    /// </summary>
    double jetM;
    /// <summary>
    /// Скорость потока вещества
    /// </summary>
    double jetV;
    /// <summary>
    /// Стартовая высота
    /// </summary>
    double StartH;
    /// <summary>
    /// Ускорение свободного падения
    /// </summary>
    double G = 2.6;
    /// <summary>
    /// Безопасная скорость посадки
    /// </summary>
    vect LandV;
    /// <summary>
    /// Максимальный расход топлива
    /// </summary>
    double MaxFF;

    /// <summary>
    /// 
    /// </summary>
    public double trust;
    /// <summary>
    /// Вектор скорости ракеты
    /// </summary>
    public vect v = new vect(0,0,0);
    /// <summary>
    /// Текущая высота
    /// </summary>
    public double h;       
    /// <summary>
    /// Вектор ускорения
    /// </summary>
    public vect ac = new vect(0,0,0);
    /// <summary>
    /// Промежуток времени между физ. кадрами
    /// </summary>
    public double delta;
    /// <summary>
    /// Кол-во кадров физики
    /// </summary>
    int FPS;

    private Status status = Status.off;

    enum Status {
        off,
        work,
        pause
    }


    public event Action GraphicFrameUpdate;
    public bool ReadData(out double Fm, out double Rm, out double JetS, out double JetM, out double Hstart, out double G,out double MaxFF, out vect Ls, out double delta)
    {
        bool res = true;

        if (!double.TryParse(Gravity.text ,out G)) res = false;
        if (!double.TryParse(Fuel.text, out Fm)) res = false;
        if (!double.TryParse(RocketMass.text, out Rm)) res = false;
        if (!double.TryParse(jetSpeed.text, out JetS)) res = false;
        if (!double.TryParse(jetMass.text, out JetM)) res = false;
        if (!double.TryParse(HighStart.text, out Hstart)) res = false;
        if (!double.TryParse(MaxFuelFlow.text, out MaxFF)) res = false;
        if (!double.TryParse(LandingSpeed.text, out double Lv)) res = false;
            Ls = new vect(Lv,Lv,0);
        if (!double.TryParse(delta_input.text, out delta)) res = false;
        return res;
    }
    public void SceneStart()
    {

        v = new vect(0,0,0);
        h = 0;
        ac = v;
            if (ReadData(out Fm, out Rm, out jetV, out jetM, out StartH, out G, out MaxFF, out LandV, out delta))
            {
                //rocket.StartH = (float)StartH;
                status = Status.work;
                StartCoroutine(PhysicFrame((float)delta));
                StartCoroutine(GraphicFrame());
            }
            else
                Debug.Log("Неудалось считать дынные");
    }
    public void ScenePause()
    {
        status = Status.pause;
    }
    IEnumerator PhysicFrame(float delta)
    {
        if (status == Status.off || status == Status.pause)
        {
            yield break;
        }
        if(Fm <= 0)
            yield break;
        v = Verle(out ac, v, delta, jetV, jetM, Rm + Fm);
        Fm = Fm - jetM * delta;
        out_speed.text = v.ToString();
        yield return new WaitForSeconds(0.001f);
        yield return StartCoroutine(PhysicFrame(delta));
    }

    IEnumerator GraphicFrame() {
        if (status == Status.off || status == Status.pause)
        {
            yield break;
        }
        yield return new WaitForSeconds(1/60);
        out_accel.text = ac.ToString();
        out_Fuel.text = Fm.ToString();
        out_fuelFlow.text = jetM.ToString();
        out_height.text = h.ToString();
        out_speedd.text = v.y.ToString();
        rocket?.UpdateFrame(new Vector3(0,(float)h,0));
        GraphicFrameUpdate?.Invoke();
        yield return StartCoroutine(GraphicFrame());
    }

    vect Verle(out vect a, vect v, double delta, double jetV, double jetM, double m)
    {
        a = calculate_accel(jetV, jetM, m, delta);
        h = h + v.y * delta + 0.5 * a.y * Math.Sqrt(delta);
        v.y = v.y + 0.5 * (ac.y + a.y) * delta;

        return v;
    }
    vect Eiler(out vect a, vect v, double delta, double jetV, double jetM, double m)
    {
        a = calculate_accel(jetV, jetM, m, delta);
        v = v + a * delta;
        h = h + v.y * delta;
        return v;
    }

    vect calculate_accel(double jetV, double jetM, double Rm, double delta) 
    {
        vect accel = new()
        {
            y = jetV * jetM / Rm //- G 
        };
        return accel;
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

        public override string ToString()
        {
            return $"({x}, {y}, {z})";
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