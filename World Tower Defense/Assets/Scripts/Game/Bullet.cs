using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PollingObject
{
    private float speed;
    private Transform target;
    public override void OnCreated()
    {
        speed = 0.1f;
        gameObject.SetActive(false);
    }

    public override void OnInitiate()
    {
        
    }

    private void Start()
    {
        Invoke("DestroySelf",1f);
    }

    private void DestroySelf()
    {
        Polling2.ReturnObject(this);
    }

    public void SpawnTo(Transform p_target)
    {
        target = p_target;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, speed);
        }
    }
}
