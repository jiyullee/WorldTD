using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PollingObject
{
    private float moveSpeed = 6f;
    private float damage;
    private float decreaseMonsterSpeed;
    private PollingObject target;
    
    #region CallBacks

    public override void OnCreated()
    {
        target = null;
    }

    public override void OnInitiate()
    {

    }
    
    private void Update()
    {
        if (target != null)
        {
            Move();
            
            if (target.gameObject.activeSelf == false)
                DestroySelf();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Monster monster = collider.gameObject.GetComponent<Monster>();
        if (monster != null && monster == target)
        {
            monster.GetDamage(damage);
            monster.DecreaseSpeed(decreaseMonsterSpeed);
            DestroySelf();
        }
    }
    
    #endregion

    #region Functions

    private void DestroySelf()
    {
        target = null;
        Polling2.ReturnObject(this);
    }

    public void Init(PollingObject p_target, float p_damage, float p_decrease = 1)
    {
        target = p_target;
        damage = p_damage;
        decreaseMonsterSpeed = p_decrease;
    }

    public void Move()
    {
        Vector3 dir = target.transform.position - transform.position;
        transform.position += dir.normalized * Time.deltaTime * moveSpeed;
    }

    #endregion
}
