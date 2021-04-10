using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class Tower : PollingObject
{
    #region Fields

    public string towerName { get; private set; }
    public string[] synergyName { get; private set; }
    [SerializeField] private int cost;
    [SerializeField] private int grade;

    [SerializeField] private float range;
    [SerializeField] private float speed;
    private float[] attack;
    private float cur_attack;

    private bool canAttack;
    private TOWER_STATE TowerState;
    public LayerMask LayerMask_Attack;
    public PollingObject target;
    private Coroutine runningRoutine;
    private Queue<Bullet> list_bullet = new Queue<Bullet>();

    private float decreaseMonsterSpeed;
    #endregion

    #region Callbacks

    public override void OnCreated()
    {
        target = null;
        list_synergy = new List<Synergy>();
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
        bullet.Init(target, cur_attack, decreaseMonsterSpeed);
        list_bullet.Enqueue(bullet);

    }

    public void SetTowerData(TowerInstance p_towerInstance)
    {
        TowerData.TowerDataClass towerData = p_towerInstance.GetTowerData();
        towerName = towerData.TowerName;
        synergyName = towerData.SynergyName.ToArray();
        cost = towerData.Cost;
        range = towerData.Range * 0.6f; // 범위 보정
        speed = towerData.Speed;
        attack = towerData.Attack.ToArray();
        grade = 1;
        cur_attack = attack[grade];

        canAttack = true;
        decreaseMonsterSpeed = 1f;
        
        for (int i = 0; i < synergyName.Length; i++)
        {
            Synergy synergy = gameObject.AddComponent(System.Type.GetType(synergyName[i])) as Synergy;;
            list_synergy.Add(synergy);
        }
        
        ChangeState(TOWER_STATE.SearchTarget);
        StartCoroutine(SearchTarget());
    }
    
    public void ActiveSynergy()
    {
        for (int i = 0; i < list_synergy.Count; i++)
        {
            int index = SynergyManager.Instance.GetActivateIndex(synergyName[i]);
            list_synergy[i].SetSynergy(this, synergyName[i], index);
        }
    }
    
    private void ChangeState(TOWER_STATE state)
    {
        TowerState = state;
    }

    #endregion

    #region Coroutine

    IEnumerator SearchTarget()
    {
        while (true)
        {
            yield return null;

            float closetDist = Mathf.Infinity;
            List<PollingObject> list_monsters = MonsterSponer.spawned_monsters;
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

    #endregion

    #region SynergyFuncs

    public void DecreaseMonsterSpeed(float p)
    {
        decreaseMonsterSpeed = p;
    }

    #endregion

}