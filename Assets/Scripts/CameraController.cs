using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject _target;

    void Update()
    {
        Vector3 v = transform.position;
        if (_target.transform.position.y > 40f)
            transform.position =  new Vector3(_target.transform.position.x,_target.transform.position.y, v.z);        
    }
}
