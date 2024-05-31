using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutInfo : MonoBehaviour
{
    [SerializeField] Model model;

    [SerializeField] TMP_Text out_H;
    [SerializeField] TMP_Text out_accel;
    [SerializeField] TMP_Text out_speedd;
    [SerializeField] TMP_Text out_height;
    [SerializeField] TMP_Text out_Fuel;
    [SerializeField] TMP_Text out_fuelFlow;
    [SerializeField] TMP_Text out_MaxCLKSpeed;
    [SerializeField] TMP_Text out_Time;

    private void Start()
    {
        model.OnPhisicFrame += UpdateText;
        GameStateMashine.StartClk += StartGame;
        GameStateMashine.Start += StartGame;
    }

    private void UpdateText()
    {
        out_H.text = "������:" + string.Format("{0:f2}", model.RocketPos.y);
        out_accel.text = $"���������:{model.ac.ToString()}";
        out_Fuel.text = "����� �������:" + string.Format("{0:f2}", model.Fm);
        out_fuelFlow.text = "������ �������:" + string.Format("{0:f2}",model.jetM);
        out_height.text = "�������:" + string.Format("{0:f2}", model.RocketPos);
        out_speedd.text = "��������:" + string.Format("{0:f2}", model.v);
        out_Time.text = "�����:" + string.Format("{0:f2}", model.Time);
    }

    private void StartGame()
    {
        out_MaxCLKSpeed.text = "��������:" + string.Format("{0:f2}", model.Cialkivskiy().ToString());
    }
}
