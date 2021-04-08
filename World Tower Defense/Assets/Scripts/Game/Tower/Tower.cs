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
    public PollingObject target;
    private Coroutine runningRoutine;
    private Queue<Bullet> list_bullet = new Queue<Bullet>();

    #endregion

    #region Callbacks

    public override void OnCreated()
    {
        target = null;
    }

    public override void OnInitiate()
    {

    }

    #endregion

    #region Functions

    public void SpawnBullet()
    {
        Bullet bullet = (Bullet)Polling2.GetObject(gameObject, "Bullet");
        bullet.transform.position = transform.position;
        bullet.SpawnTo(target, speed, cur_attack);
        list_bullet.Enqueue(bullet);

    }

    public void SetTowerData(TowerInstance p_towerInstance)
    {
        TowerData.TowerDataClass towerData = p_towerInstance.GetTowerData();
        towerName = towerData.TowerName;
        synergyName = towerData.SynergyName.ToArray();
        cost = towerData.Cost;
        range = towerData.Range * 0.65f;
        speed = towerData.Speed;
        attack = towerData.Attack.ToArray();
        grade = 1;
        cur_attack = attack[grade];

        canAttack = true;
        ChangeState(TOWER_STATE.SearchTarget);
        StartCoroutine(SearchTarget());
    }

    private void ChangeState(TOWER_STATE state)
    {
        TowerState = state;
    }

    #endregion

    IEnumerator SearchTarget()
    {
        while (true)
        {
            yield return null;

            float closetDist = Mathf.Infinity;
            List<PollingObject> list_monsters = MonsterSponer.Instance.spawned_monsters;
            for (int i = 0; i < list_monsters.Count; i++)
            {
                float dist = Vector2.Distance(list_monsters[i].transform.position, transform.position);
                if (dist <= range && dist <= closetDist)
                {
                    closetDist = dist;
                    target = list_monsters[i];
                }
            }

            if (target != null)
            {
                ChangeState(TOWER_STATE.Attack);
                StartCoroutine(Attack());
                break;
            }

        }
    }

    IEnumerator Attack()
    {
        while (true)
        {
            if (target == null)
            {
                ChangeState(TOWER_STATE.SearchTarget);
                StartCoroutine(SearchTarget());
                break;
            }

            float dist = Vector2.Distance(target.transform.position, transform.position);
            if (dist >= range)
            {
                target = null;
                ChangeState(TOWER_STATE.SearchTarget);
                StartCoroutine(SearchTarget());
                break;
            }
            SpawnBullet();
            yield return new WaitForSeconds(1 / speed);
        }
    }

}