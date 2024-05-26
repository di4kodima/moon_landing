using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketView : MonoBehaviour
{
    [SerializeField] Model Model;
    [SerializeField] GameObject image;


    private void Update()
    {
        if (Model.RocketPos.x is double.NaN)
            return;
        transform.position = (Vector3) Model.RocketPos;

        Quaternion quaternion = gameObject.transform.rotation;
        quaternion.eulerAngles = new Vector3(0, 0, (float)Model.angle);
        image.transform.rotation = quaternion;
    }
}
