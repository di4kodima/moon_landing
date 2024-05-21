using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour, IStar
{
    public event IStar.EventHandler OnBigDistance;

    private float _range = 40;
    private GameObject _target;
    void IStar.Activate(GameObject target, float range)
    {
        _target = target;
        _range = range;
        Debug.Log("FDS");
        StartCoroutine("DistanceMonitoring");
    }

    IEnumerator DistanceMonitoring()
    {
        while (true)
        {
            Debug.Log("GGGd");
            if (Vector3.Distance( transform.position, _target.transform.position) > _range)
                OnBigDistance.Invoke(gameObject);
            yield return new WaitForSecondsRealtime(1);
        }
    }

    void IStar.Deactivate()
    {
    }
}
