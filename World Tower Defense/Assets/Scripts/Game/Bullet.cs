using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PollingObject
{
    private float speed;
    private float damage;
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

    public void SpawnTo(PollingObject p_target, float p_speed, float p_damage)
    {
        target = p_target;
        speed = p_speed;
        damage = p_damage;
        //Invoke("DestroySelf", 1 / speed);
    }

    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, 5 * Time.deltaTime);
            // transform.position = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        }
        if (target.gameObject.activeSelf == false)
            DestroySelf();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Monster monster = other.gameObject.GetComponent<Monster>();
        if (monster == target)
        {
            monster.GetDamage((int)damage);
        }
    }
}
