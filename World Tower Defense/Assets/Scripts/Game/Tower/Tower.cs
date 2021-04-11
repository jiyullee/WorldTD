using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private bool isPeninsula;
    private bool isContinent;
    
    private TOWER_STATE TowerState;
    public LayerMask LayerMask_Attack;
    public PollingObject target;
    private Coroutine runningRoutine;
    private Queue<Bullet> list_bullet = new Queue<Bullet>();

    private float decreaseMonsterSpeed;
    private float increaseAttack;
    private float increaseRange;
    private bool isActiveAfricaSynergy;
    private bool ignoreArmor;
    private float aroundDamage;
    private float percent_damageBuff;
    private float decreaseArmor;
    private float trueDamage;

    private Collider2D[] colliders;
    public LayerMask AroundTowerLayer;
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
            if (synergyName[i] == SYNERGY.Peninsula.ToString())
                isPeninsula = true;
            if (synergyName[i] == SYNERGY.Continent.ToString())
                isContinent = true;
            
            Synergy synergy = gameObject.AddComponent(System.Type.GetType(synergyName[i])) as Synergy;;
            list_synergy.Add(synergy);
        }
        
        ChangeState(TOWER_STATE.SearchTarget);
        StartCoroutine(SearchTarget());
        if(isPeninsula)
            StartCoroutine(SearchAround(1.2f));
        if(isContinent)
            StartCoroutine(SearchAround(0.6f));
    }
    
    public void SpawnBullet()
    {
        Bullet bullet = (Bullet)Polling2.GetObject(gameObject, "Bullet");
        bullet.transform.position = transform.position;
        bullet.Init(target, GetDamage(), decreaseMonsterSpeed, decreaseArmor, aroundDamage, trueDamage);
        bullet.IgnoreArmor(ignoreArmor);
        list_bullet.Enqueue(bullet);

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

    //최종 타워 데미지 환산 메소드
    private float GetDamage()
    {
        float rand = Random.Range(0f, 1f);
        if (rand < percent_damageBuff)
            cur_attack = attack[grade] * increaseAttack * 2;
        else
            cur_attack = attack[grade] * increaseAttack;
        
        return cur_attack;
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
 
                if (dist <= range + increaseRange && dist <= closetDist)
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

    IEnumerator SearchAround(float radius)
    {
        while (true)
        {
            yield return null;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, AroundTowerLayer);

            if (isPeninsula)
            {
                if (colliders.Length == 0)
                    ignoreArmor = true;
                else
                    ignoreArmor = false;
            }
        }
    }

    #endregion

    #region SynergyFuncs

    // 아시아
    public void DecreaseMonsterSpeed(float p)
    {
        decreaseMonsterSpeed = 1 - p;
    }

    //아프리카
    public void DamageBuff(float p_percent)
    {
        percent_damageBuff = p_percent;
    }

    //북미
    public void IncreaseRange(float p)
    {
        increaseRange = p;
    }

    //남미
    public void DecreaseArmor(float p)
    {
        decreaseArmor = p;
    }

    //오세아니아
    public void IncreaseAttack(float p)
    {
        increaseAttack = 1 + p;
    }

    //유럽
    public void IncreaseSpeed(float p)
    {
        speed *= 1 + p;
    }

    //반도
    public void AroundAttack(float p)
    {
        aroundDamage = p;
    }

    //대륙
    public void IncreaseTrueDamage(float p_damage)
    {
        if (colliders != null)
        {
            trueDamage = p_damage * colliders.Length;    
        }
        else
        {
            trueDamage = 0;
        }
    }
    #endregion

}