using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StarSpwnera : MonoBehaviour
{
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject StarPrefab;
    [SerializeField]
    float SpawnRange = 50;
    [SerializeField]
    int StarsCount = 30;
    private List<IStar> _stars;
    private Vector3 LastTargetPos;
    // Start is called before the first frame update
    private void Awake()
    {
        _stars = new List<IStar>();
        for (int i = 0; i < StarsCount; i++)
            SpawnStar();
        LastTargetPos = target.transform.position;
    }

    void ReplaceStar(GameObject star)
    {
        star.transform.position = target.transform.position + new Vector3((Random.value - 0.5f) * SpawnRange + 200, (Random.value - 0.5f) * SpawnRange + 200, transform.position.z);
        Debug.Log("lk");
    }

    void SpawnStar()
    {
        if (_stars.Count < StarsCount) 
        {
            GameObject NewStar = Instantiate(StarPrefab, transform);
            IStar StarCS= NewStar.GetComponent<IStar>();

            NewStar.transform.position = target.transform.position + new Vector3((Random.value - 0.5f) * SpawnRange + 200, (Random.value - 0.5f) * SpawnRange + 200, transform.position.z); //new Vector3((Random.value - 0.5f)* 2 * SpawnRange, (Random.value - 0.5f) * 2 * SpawnRange,0);
            _stars.Add(StarCS);
            StarCS.OnBigDistance += ReplaceStar;
            StarCS.Activate(target,SpawnRange);
           
        }
    }
    void Update()
    {
        
    }
}

public interface IStar
{
    public delegate void EventHandler(GameObject sender);
    public event EventHandler OnBigDistance;
    public void Activate(GameObject target, float range);
    public void Deactivate();
}
