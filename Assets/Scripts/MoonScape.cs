using UnityEngine;

public class MoonScape : MonoBehaviour
{
    [SerializeField] 

    private GameObject _target;
    [SerializeField]
    private float step = 40;

    private void Update()
    {
        Vector3 v1 = transform.position;
        Vector3 v2 = _target.transform.position;
        v1.y = 0;
        v2.y = 0;
        if(Vector3.Distance(v1,v2) > step)
            transform.position = v2;

    }
}
