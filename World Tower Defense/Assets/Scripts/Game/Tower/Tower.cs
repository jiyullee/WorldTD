using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class Tower : PollingObject
{
    #region Fields

    [SerializeField] private string towerName;
    private string[] synergyName;
    [SerializeField] private int cost;
    [SerializeField] private int grade;
    
    [SerializeField] private float range;
    private float speed;
    private float[] attack;
    private float cur_attack;
    
    private bool canAttack;
    private TOWER_STATE TowerState;
    public LayerMask LayerMask_Attack;
    private Transform target;
    
    private Queue<Bullet> list_bullet = new Queue<Bullet>();

    #endregion

    #region Callbacks

    public override void OnCreated()
    {
        
    }

    public override void OnInitiate()
    {
        
    }

    private void Start()
    {
        ChangeState(TOWER_STATE.SearchTarget);
    }

    private void Update()
    {
        
    }

    #endregion

    #region Functions

    public void SpawnBullet()
    {
        Bullet bullet = (Bullet)Polling2.GetObject(gameObject, "Bullet");
        bullet.transform.position = transform.position;
        bullet.SpawnTo(target);
        list_bullet.Enqueue(bullet);

    }
    
    public void SetTowerData(TowerInstance p_towerInstance)
    {
        TowerData.TowerDataClass towerData = p_towerInstance.GetTowerData();
        towerName = towerData.TowerName;
        synergyName = towerData.SynergyName.ToArray();
        cost = towerData.Cost;
        range = towerData.Range;
        speed = towerData.Speed;
        attack = towerData.Attack.ToArray();
        grade = 1;
        cur_attack = attack[grade];

        canAttack = true;
    }
    
    private void ChangeState(TOWER_STATE state)
    {
        StopCoroutine(TowerState.ToString());
        TowerState = state;
        StartCoroutine(state.ToString());
    }

    #endregion

    IEnumerator SearchTarget()
    {
        while (true)
        {
            float closetDist = Mathf.Infinity;
            PollingObject[] list_monsters = MonsterSponer.Instance.monsterQueue.ToArray();
            for (int i = 0; i < list_monsters.Length; i++)
            {
                float dist = Vector2.Distance(list_monsters[i].transform.position, transform.position);

                if (dist <= range && dist <= closetDist)
                {
                    closetDist = dist;
                    target = list_monsters[i].transform;
                }
            }

            if (target != null)
            {
                ChangeState(TOWER_STATE.Attack);
            }

            yield return null;
        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (target == null)
            {
                ChangeState(TOWER_STATE.SearchTarget);
                break;
            }

            float dist = Vector3.Distance(target.position, transform.position);
            if (dist >= range)
            {
                target = null;
                ChangeState(TOWER_STATE.SearchTarget);
                break;
            }
            SpawnBullet();
            yield return new WaitForSeconds(1 / speed);
        }
    }
    
}