using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]
    GameObject Sprite;
    [SerializeField]
    public ParticleSystem particleSystem;
    [SerializeField]
    UiController UiController;
    [SerializeField]
    int MaxParticleCount = 1000;
    Vector3 StartHeght;
    public float StartH { set { Scale = StartHeght.y / value; } }
    float Scale;
    
    public void UpdateFrame(Vector3 pos, float angle)
    {
        gameObject.transform.position = pos;
        Quaternion quaternion = gameObject.transform.rotation;
        quaternion.eulerAngles = new Vector3(0, 0, angle);
        Sprite.transform.rotation = quaternion;
        particleSystem.maxParticles = (int)(MaxParticleCount * (UiController.jetM / UiController.MaxFF));
    }

    private void Start()
    {
        StartHeght = transform.position;
    }
}
