using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PollingObject
{
    private float speed;
    private PollingObject target;
    public override void OnCreated()
    {
        gameObject.SetActive(false);
    }

    public override void OnInitiate()
    {
        
    }

    private void DestroySelf()
    {
        target = null;
        gameObject.SetActive(false);
        Polling2.ReturnObject(this);
    }

    public void SpawnTo(PollingObject p_target, float p_speed)
    {
        target = p_target;
        speed = p_speed;
        //Invoke("DestroySelf", 1 / speed);
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, 5 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<PollingObject>() == target)
            DestroySelf();
    }
}
