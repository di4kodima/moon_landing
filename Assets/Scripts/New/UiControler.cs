using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using XCharts.Runtime;
using static UiController;

public class UiReader : MonoBehaviour
{
    #region SerializeFields
    [SerializeField] TMP_InputField Fuel;
    [SerializeField] TMP_InputField RocketMass;
    [SerializeField] TMP_InputField jetSpeed;
    [SerializeField] TMP_InputField jetMass;
    [SerializeField] TMP_InputField HighStart;
    [SerializeField] TMP_InputField Gravity;
    [SerializeField] TMP_InputField MaxFuelFlow;
    [SerializeField] TMP_InputField LandingSpeedX;
    [SerializeField] TMP_InputField LandingSpeedY;
    [SerializeField] TMP_InputField delta_input;
    #endregion

    public delegate void InputDelegate();
    public event InputDelegate OnInputWasEdit;

    public bool ReadData(out double Fm, out double Rm, out double JetS, out double JetM, out double Hstart, out double G, out double MaxFF, out vect Lv, out double delta)
    {
        bool res = true;
        Lv = new vect();

        if (!double.TryParse(Gravity.text, out G)) res = false;
        if (!double.TryParse(Fuel.text, out Fm)) res = false;
        if (!double.TryParse(RocketMass.text, out Rm)) res = false;
        if (!double.TryParse(jetSpeed.text, out JetS)) res = false;
        if (!double.TryParse(jetMass.text, out JetM)) res = false;
        if (!double.TryParse(HighStart.text, out Hstart)) res = false;
        if (!double.TryParse(MaxFuelFlow.text, out MaxFF)) res = false;
        if (!double.TryParse(LandingSpeedX.text, out Lv.x)) res = false;
        if (!double.TryParse(LandingSpeedY.text, out Lv.y)) res = false;

        if (!double.TryParse(delta_input.text, out delta)) res = false;
        Debug.Log(res);
        return res;
    }
}
