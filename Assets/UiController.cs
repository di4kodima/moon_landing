using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField]
    GameObject Fuel;
    [SerializeField]
    GameObject RocketMass;
    [SerializeField]
    GameObject jetSpeed;
    [SerializeField]
    GameObject jetMass;
    [SerializeField]
    GameObject HighStart;
    [SerializeField]
    GameObject Gravity;
    [SerializeField]
    GameObject MaxFuelFlow;
    [SerializeField]
    GameObject LandingSpeed;

    public bool ReadData(out double Fm, out double Rm, out double JetS, out double JetM, out double Hstart, out vect G,out double MaxFF, out double Ls)
    {
        bool res = true;
        G = new vect();
        if (!double.TryParse(Gravity.GetComponent<InputField>().text ,out G.y)) res = false;
        if (!double.TryParse(Fuel.GetComponent<InputField>().text,out Fm)) res = false;
        if (!double.TryParse(RocketMass.GetComponent<InputField>().text, out Rm)) res = false;
        if (!double.TryParse(jetSpeed.GetComponent<InputField>().text, out JetS)) res = false;
        if (!double.TryParse(jetMass.GetComponent<InputField>().text, out JetM)) res = false;
        if (!double.TryParse(HighStart.GetComponent<InputField>().text, out Hstart)) res = false;
        if (!double.TryParse(MaxFuelFlow.GetComponent<InputField>().text, out MaxFF)) res = false;
        if (!double.TryParse(LandingSpeed.GetComponent<InputField>().text, out Ls)) res = false;
        return res;
    }

}
