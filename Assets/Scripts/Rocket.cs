using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]
    GameObject Sprite;
    Vector3 StartHeght;
    public float StartH { set { Scale = StartHeght.y / value; } }
    float Scale;
    
    public void UpdateFrame(Vector3 pos, float angle)
    {
        gameObject.transform.position = pos;
        Quaternion quaternion = gameObject.transform.rotation;
        quaternion.eulerAngles = new Vector3(0, 0, angle);
        Sprite.transform.rotation = quaternion;
    }

    private void Start()
    {
        StartHeght = transform.position;
    }
}
