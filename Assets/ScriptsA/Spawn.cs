using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject object1BluePrint;
    private GameObject obj;
    [SerializeField] private float elapsedTime;
    public float IntervalTime;
    public Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= IntervalTime)
        {
            SpawnObj();
            elapsedTime = 0;
        }
    }

    private void SpawnObj()
    {
        obj = Instantiate(object1BluePrint) as GameObject;
        obj.transform.position = pos;
    }
}
