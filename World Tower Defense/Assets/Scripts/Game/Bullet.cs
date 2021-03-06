using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PollingObject
{
    private float moveSpeed = 6f;
    private float damage;
    private float decreaseMonsterSpeed;
    private float decreaseArmor;
    private float aroundDamage;
    private float trueDamage;
    private PollingObject target;
    private bool ignoreArmor;
    private bool damageAround;
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
        BulletSprite();
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
            monster.Damage(damage, ignoreArmor, decreaseArmor, trueDamage);
            if(damageAround) monster.DamageAround(aroundDamage);
            monster.DecreaseSpeed(decreaseMonsterSpeed);
            DestroySelf();
        }
    }
    
    #endregion

    #region Functions

    private void DestroySelf()
    {
        target = null;
        PoolingManager.ReturnObject(this);
    }

    public void Init(PollingObject p_target, float p_damage, float p_decreaseSpeed, float p_decreaseArmor, float p_aroundDamage, float p_trueDamage)
    {
        target = p_target;
        damage = p_damage;
        decreaseMonsterSpeed = p_decreaseSpeed;
        decreaseArmor = p_decreaseArmor;
        aroundDamage = p_aroundDamage;
        trueDamage = p_trueDamage;
    }

    public void IgnoreArmor(bool flag)
    {
        ignoreArmor = flag;
    }

    public void DamageAround(bool flag)
    {
        damageAround = flag;
    }

    public void Move()
    {
        Vector3 dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg+90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.position += dir.normalized * Time.deltaTime * moveSpeed;
    }

    public void BulletSprite()
    {
        string towerName = gameObject.transform.parent.GetComponent<Tower>().TowerName;
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Bullets/{towerName}_??????");

    }

    #endregion
}
