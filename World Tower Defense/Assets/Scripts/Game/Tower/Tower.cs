using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : PollingObject
{
    #region Fields

    public string TowerName { get; private set; }
    public string[] SynergyNames { get; private set; }
    [SerializeField] private int cost;
    public int Grade { get; private set; }

    [SerializeField] private float range;
    [SerializeField] private float speed;
    private float[] damages;
    private float cur_damage;

    private bool canAttack;
    private bool isPeninsula;
    private bool isContinent;

    private TOWER_STATE TowerState;

    public PollingObject target;
    private Coroutine runningRoutine;
    private Queue<Bullet> list_bullet = new Queue<Bullet>();

    private float decreaseMonsterSpeed = 0f;
    private float increaseAttack = 0f;
    private float increaseSpeed = 0f;
    private float increaseRange = 0f;
    private bool isActiveAfricaSynergy;
    private bool ignoreArmor;
    private float aroundDamage;
    private float percent_damageBuff;
    private float decreaseArmor;
    private float trueDamage;

    private Collider2D[] colliders;
    public LayerMask AroundTowerLayer;
    public TowerButtonUI ButtonUI { get; private set; }
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

    public void ReturnTower()
    {
        ButtonUI.InitTower();
        PoolingManager.ReturnObject(this);

    }

    public void SetTowerData(TowerInstance p_towerInstance)
    {
        TowerData.TowerDataClass towerData = p_towerInstance.GetTowerData();
        TowerName = towerData.TowerName;
        SynergyNames = towerData.SynergyName.ToArray();
        cost = towerData.Cost;
        range = towerData.Range * 0.6f; // 범위 보정
        speed = towerData.Speed;
        damages = towerData.Damage.ToArray();
        Grade = 1;
        cur_damage = damages[Grade - 1];
        canAttack = true;
  
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Image/Towers/{TowerName}{Grade}");
        if ((gameObject.GetComponent<SpriteRenderer>().sprite) == null)
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Image/TowerSample");

        for (int i = 0; i < SynergyNames.Length; i++)
        {
            if (SynergyNames[i] == SYNERGY.Peninsula.ToString())
                isPeninsula = true;
            if (SynergyNames[i] == SYNERGY.Continent.ToString())
                isContinent = true;

            Synergy synergy = gameObject.AddComponent(System.Type.GetType(SynergyNames[i])) as Synergy; ;
            list_synergy.Add(synergy);
        }

        ChangeState(TOWER_STATE.SearchTarget);
        StartCoroutine(SearchTarget());
        if (isPeninsula)
            StartCoroutine(SearchAround(1.2f));
        if (isContinent)
            StartCoroutine(SearchAround(0.6f));
    }

    public void SetPositionFromScreen(Vector2 p_pos)
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(p_pos);
        transform.position = new Vector3(targetPos.x, targetPos.y, 0);
    }

    public void SpawnBullet()
    {
        Bullet bullet = (Bullet)PoolingManager.GetObject(gameObject, "Bullet");
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        bullet.Init(target, GetDamage(), decreaseMonsterSpeed, decreaseArmor, aroundDamage, trueDamage);
        bullet.IgnoreArmor(ignoreArmor);
        list_bullet.Enqueue(bullet);

    }

    public void SetButtonUI(TowerButtonUI p_buttonUI)
    {
        ButtonUI = p_buttonUI;
    }

    public void ActiveSynergy()
    {
        for (int i = 0; i < list_synergy.Count; i++)
        {
            int index = SynergyManager.Instance.GetActivateIndex(SynergyNames[i]);
            list_synergy[i].SetSynergy(this, SynergyNames[i], index);
        }
    }

    private void ChangeState(TOWER_STATE state)
    {
        TowerState = state;
    }

    public float GetCurrentDamage()
    {
        return damages[Grade - 1] * (1 + increaseAttack);
    }

    public float GetCurrentSpeed()
    {
        return speed * (1 + increaseSpeed);
    }

    public float GetCurrentRange()
    {
        return range * (1 + increaseRange);
    }

    //최종 타워 데미지 환산 메소드
    private float GetDamage()
    {
        float rand = Random.Range(0f, 1f);
        if (rand < percent_damageBuff)
            cur_damage = damages[Grade - 1] * (1 + increaseAttack) * 2;
        else
            cur_damage = damages[Grade - 1] * (1 + increaseAttack);

        return cur_damage;
    }

    public void Upgrade()
    {
        Grade++;
        ButtonUI.SetViewTowerUI();
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Image/Towers/{TowerName}{Grade}");
        if ((gameObject.GetComponent<SpriteRenderer>().sprite) == null)
            gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Image/TowerSample");
        ParticleSystem particle = EffectManager.GetParticle(transform);
        EffectManager.ReturnParticle(particle);
    }

    #endregion

    #region Coroutine

    IEnumerator SearchTarget()
    {
        while (true)
        {
            yield return null;

            float closetDist = Mathf.Infinity;
            List<PollingObject> list_monsters = MonsterManager.spawned_monsters;
            for (int i = 0; i < list_monsters.Count; i++)
            {
                float dist = Vector2.Distance(list_monsters[i].transform.position, transform.position);

                if (dist <= GetCurrentRange() + increaseRange && dist <= closetDist)
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
            yield return new WaitForSeconds(1 / GetCurrentSpeed());
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
        decreaseMonsterSpeed = p;
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
        increaseAttack = p;
    }

    //유럽
    public void IncreaseSpeed(float p)
    {
        increaseSpeed = p;
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