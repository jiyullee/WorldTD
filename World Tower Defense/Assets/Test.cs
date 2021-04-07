using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        float dist = Vector3.Distance(transform.position, obj.transform.position);
        Debug.Log(dist);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
