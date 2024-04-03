using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Vector3 StartHeght;
    public float StartH { set { Scale = StartHeght.y / value; } }
    float Scale;
    
    public void UpdateFrame(Vector3 pos)
    {
        gameObject.transform.position = pos * Scale;
    }

    private void Start()
    {
        StartHeght = transform.position;
    }
}
