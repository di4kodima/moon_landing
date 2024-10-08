using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using XCharts.Runtime;

public class UiController : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] TMP_InputField Fuel;
    [SerializeField] TMP_InputField RocketMass;
    [SerializeField] TMP_InputField jetSpeed;
    [SerializeField] TMP_InputField jetMass;
    [SerializeField] TMP_InputField HighStart;
    [SerializeField] TMP_InputField Gravity;
    [SerializeField] TMP_InputField MaxFuelFlow;
    [SerializeField] TMP_InputField LandingSpeed;
    [SerializeField] TMP_InputField delta_input;

    [SerializeField] TMP_Text out_speed;
    [SerializeField] TMP_Text out_accel;
    [SerializeField] TMP_Text out_speedd;
    [SerializeField] TMP_Text out_height;
    [SerializeField] TMP_Text out_Fuel;
    [SerializeField] TMP_Text out_fuelFlow;
    [SerializeField] TMP_Text out_MaxCLKSpeed;
    [SerializeField] TMP_Text out_FinishLable;

    [SerializeField] LineChart VChart;
    [SerializeField] LineChart AChart;
    [SerializeField] LineChart FChart;
    [SerializeField] LineChart FFChart;
    [SerializeField] LineChart TChart;
    [SerializeField] LineChart HChart;
    [SerializeField] LineChart ClkChart;


    [SerializeField] Rocket rocket;
    [SerializeField] ParticleSystem Rocket_ParticlesSystem;
    #endregion
    [SerializeField]
    public bool UseCialkovskiy { get; set; }

    #region Params
    vect RocketPos;
    double Rm;
    public double Fm = 100;
    double _jetM;

    public double jetM
    {
        get { return _jetM;}
        set {
            if (value <= MaxFF && value >= 0)
                _jetM = value;
        }
    }
    double jetV;
    double StartH;
    double G = 2.6;
    vect LandV;
    public double MaxFF;
    public double trust;
    public vect v = new vect(0,0,0);
    public vect ac = new vect(0,0,0);
    public double delta;
    int FPS;

    [SerializeField]
    double angel = 90;

    private Status status = Status.off;

    enum Status {
        off,
        work,
        pause
    }
    
    List<vect> Varray = new();
    List<double> Harray = new();
    List<double> Farray = new();
    List<double> FFarray = new();
    List<double> Tarray = new();
    List<double> CLKarray = new();
    List<vect> Aarray = new();
    #endregion

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
    float RotationSpeed = 1;
    float FuelFlowStep = 0.5f;
    private void FixedUpdate()
    {
        Debug.Log(UseCialkovskiy);
        rocket?.UpdateFrame(new Vector3((float)RocketPos.x, (float)RocketPos.y, 0), (float)angel);
        if (status == Status.off || UseCialkovskiy)
            return;
        jetM += 10 * Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetKey(KeyCode.LeftArrow))
            angel += RotationSpeed;
        if (Input.GetKey(KeyCode.RightArrow))
            angel -= RotationSpeed;
        if (Input.GetKey(KeyCode.UpArrow))
            jetM += FuelFlowStep;
        if (Input.GetKey(KeyCode.DownArrow))
            jetM -= FuelFlowStep;
        if (Input.GetKey(KeyCode.Space))
            jetM = 3;
        if (jetM > 0) Rocket_ParticlesSystem.enableEmission = true;
        else { Rocket_ParticlesSystem.enableEmission = false; }
    }
    public void SceneStart()
    {
        out_FinishLable.text = "";
        if (status == Status.off)
        {
            v = new vect(0, 0, 0);
            ac = v;
            Tarray = new();
            Tarray.Add(0);
            if (ReadData(out Fm, out Rm, out jetV, out double jetm, out StartH, out G, out MaxFF, out LandV, out delta))
            {
                jetM = jetm;
                RocketPos = new vect(0, StartH, 0);
                //rocket.StartH = (float)StartH;
                VChart.series[0].data.Clear();
                FChart.series[0].data.Clear();
                HChart.series[0].data.Clear();
                AChart.series[0].data.Clear();
                TChart.series[0].data.Clear();
                FFChart.series[0].data.Clear();
                ClkChart.series[0].data.Clear();
                AddInChart(0, 0, ClkChart, 0);
                Farray.Add(Fm);
                CLKarray.Add(0);
                if (UseCialkovskiy)
                {
                    G = 0;
                    out_MaxCLKSpeed.text = $"Максимальная скорость по формуле Циалковского:{Cialkivskiy()}";
                }
                status = Status.work;
                StartCoroutine(PhysicFrame((float)delta));
                StartCoroutine(GraphicFrame());
            }
        }
        if (status == Status.pause) 
        {
            status = Status.work;
            StartCoroutine(PhysicFrame((float)delta));
            StartCoroutine(GraphicFrame());
        }
    }

    public void StopProcess()
    {
        StopAllCoroutines();
        if (status != Status.off)
        {
            status = Status.off;
        }
    }

    void FillChart(double[] x, double[] y, LineChart chart)
    {
        Serie serie = new();
        SerieData serieData = new();
        for(int i = 0; i <= x.Length; i++) 
        {
            serieData.data.Clear();
            serieData.data.Add(x[i]);
            serieData.data.Add(y[i]);
            serie.AddSerieData(serieData);
        }
        chart.series.Clear();
        chart.series.Add(serie);
    }

    void AddInChart(double x, double y, LineChart chart, int SerieNumber)
    {
        SerieData serieData = new();
        serieData.data.Add(x);
        serieData.data.Add(y);
        chart.series[0].AddSerieData(serieData);
    }

    public void ScenePause()
    {
        if(status == Status.work)
            status = Status.pause;
    }
    IEnumerator PhysicFrame(float delta)
    {
        while (true)
        {
            if (status == Status.off || status == Status.pause)
            {
                yield break;
            }
            if (Fm < jetM * delta)
            {
                jetM = Fm / delta;
            }
            if (Fm == 0)
                jetM = 0;
            v = Verle(out ac, v, delta, jetV, jetM, Rm + Fm);
            Tarray.Add(Tarray[Tarray.Count - 1] + delta);
            Farray.Add(Fm);
            Varray.Add(v);
            Aarray.Add(ac);
            Harray.Add(RocketPos.y);
            FFarray.Add(jetM);
            Fm = Fm - jetM * delta;
            if (RocketPos.y < 0) {
                status = Status.off;
                Rocket_ParticlesSystem.enableEmission = false;
                if(v.y < LandV.y && v.x < LandV.y && angel < 20 && angel > -20)
                {
                    out_FinishLable.text = "Посадка прошла успешно!";
                    out_FinishLable.fontMaterial.color = Color.green;
                }
                else
                {
                    out_FinishLable.text = "Вы потерпели крушение!";
                    out_FinishLable.fontMaterial.color = Color.red;
                }
            }
            yield return new WaitForSeconds(0.001f);
        }
    }

    double Cialkivskiy()
    {
        return 2.3f * (jetV * Math.Log10((Rm + Fm) / Rm));
    }

    IEnumerator GraphicFrame() {
        while (true)
        {

            if (Tarray.Count > 0)
            {
                AddInChart(Tarray.Last(), Varray.Last().y, VChart,0);
                AddInChart(Tarray.Last(), Farray.Last(), FChart,0);
                AddInChart(Tarray.Last(), Harray.Last(), HChart, 0);
                AddInChart(Tarray.Last(), Aarray.Last().y, AChart, 0);
                AddInChart(Tarray.Last(), FFarray.Last(), FFChart, 0);
            }
            out_accel.text = $"Ускорение: {ac.ToString()}";
            out_Fuel.text = Fm.ToString();
            out_fuelFlow.text = "Расход топлива: " + jetM.ToString();
            out_height.text = "Позиция: " + RocketPos.ToString();
            out_speedd.text = "Скорость: " + v.ToString();
            GraphicFrameUpdate?.Invoke();
            if (status == Status.off || status == Status.pause)
            {
                yield break;
            }
            yield return new WaitForSeconds(1);
        }
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
            x = (jetV * jetM / Rm) * -Math.Sin((Math.PI / 180 )* angel),
            y = (jetV * jetM / Rm ) * Math.Cos((Math.PI / 180) * angel) - G
        };
        return accel;
    }


}