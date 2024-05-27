using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UiController;

public class FlightDataRecorder : MonoBehaviour
{
    [SerializeField]
    Model model;

    public List<vect> Varray { get; private set; }
    public List<vect> PosArray {  get; private set; }
    public List<double> Farray {  get; private set; }
    public List<double> FFarray { get; private set; }
    public List<double> Tarray {  get; private set; } 
    public List<double> CLKarray {  get; private set; }
    public List<vect> Aarray { get; private set; }

    private void Awake()
    {
        model.OnPhisicFrame += Record;
        GameStateMashine.Start += ResetData;

        Varray = new List<vect>();
        PosArray = new List<vect>();
        Farray = new List<double>();
        FFarray = new List<double>();
        Tarray = new List<double>();
        CLKarray = new List<double>();
        Aarray = new List<vect>();

    }

    private void ResetData()
    {
        Varray.Clear();
        PosArray.Clear();
        Farray.Clear();
        FFarray.Clear();
        Tarray.Clear();
        CLKarray.Clear();
        Aarray.Clear();
    }

    private void Record()
    {
        Varray.Add(model.v);
        PosArray.Add(model.RocketPos);
        Farray.Add(model.Fm);
        FFarray.Add(model.jetM);
        Tarray.Add(model.Time);
        CLKarray.Add(model.CialkivskiyByStep());
        Aarray.Add(model.ac);
    }    
}
