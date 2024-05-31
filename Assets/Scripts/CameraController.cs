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
        if (_target.transform.position.y > 41f)
        {
            transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, v.z);
        }
        else if (Mathf.Abs(_target.transform.position.x - transform.position.x) < 100)
            transform.position = new Vector3(_target.transform.position.x, 41f, v.z);
    }
}
