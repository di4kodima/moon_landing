using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Chart : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    float H = 1;
    [SerializeField]
    float W = 1;

    LineRenderer lineRenderer;
    // Start is called before the first frame update

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Start()
    {
    }

    public void Show(float[] xs, float[] ys)
    {
        lineRenderer.positionCount = 0;
        Array.Sort(xs);
        float xmin = xs.Min();
        float xmax = xs.Max();

        float ymin = ys.Min();
        float ymax = ys.Max();

        Vector3[] Points = new Vector3[xs.Length];
        if (xs.Length != ys.Length) return;

        lineRenderer.positionCount = xs.Length;

        for (int i = 0; i < xs.Length; i++) 
        {
            Vector3 p = new Vector3();

            System.Random rnd = new System.Random();

            p.x = (xs[i] / xmax) * this.W; 

            p.y = (ys[i] / ymax) * this.H;
            Debug.Log(H);
            Points[i] = p;
        }

        lineRenderer.SetPositions(Points);
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(H);

        Show(new float[] { 0, 2, 3, 5, 12 }, new float[] { 0, 5, 2, 7, 3 });
    }
}
