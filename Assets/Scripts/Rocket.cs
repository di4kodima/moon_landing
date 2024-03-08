using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    float StartHeght;
    public double V = 0;
    public double a = 0;

    private void Start()
    {
        StartHeght = transform.position.y;
    }
}
