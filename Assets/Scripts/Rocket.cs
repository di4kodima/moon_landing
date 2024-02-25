using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    float StartHeght;
    private void Start()
    {
        StartHeght = transform.position.y;
    }

    void move(double MaxH, double h)
    {
        float d = ((float)MaxH / StartHeght);
        transform.position = new Vector3(0,(float)h / d, 0);
    }    
}
