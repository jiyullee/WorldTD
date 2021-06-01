using System;
using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : PollingObject
{
    #region Fields

    public TowerInstance towerInstance { get; private set; }
    public string TowerName { get; private set; }
    public string[] SynergyNames { get; private set; } 
    public int Cost { get; private set; }
    public int Grade { get; private set; }

    [SerializeField] private float range;
    [SerializeField] private float speed;
    private float[] damages;
    private float cur_damage;

    private bool canAttack;
    private bool isIsland;
    private bool isContinent;

    private TOWER_STATE TowerState;

    public PollingObject target;
    private Coroutine runningRoutine;
    private Queue<Bullet> list_bullet = new Queue<Bullet>();

    private float decreaseMonsterSpeed = 0f;
    private float increaseAttack = 0f;
    private float increaseSpeed = 0f;
    public float increaseRange { get; private set; }
    private bool isActiveAfricaSynergy;
    private bool ignoreArmor;
    private float aroundDamage;
    private bool isDamageAround;
    private float percent_damageBuff;
    private bool isDamageBuff;
    private float decreaseArmor;
    private float trueDamageWeight;
    private float trueDamageCount;
    private float trueDamage => trueDamageWeight * trueDamageCount;

    private const int MAX_GRADE = 3 + 1;
    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites = new Sprite[MAX_GRADE];
    private Collider2D[] colliders;
    public LayerMask AroundTowerLayer;
    public TowerButtonUI ButtonUI { get; private set; }
    
    #endregion

    #region Callbacks

    public override void OnCreated()
    {
        target = null;
        list_synergy = new List<Synergy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnInitiate()
    {

    }

    #endregion

    #region Functions

    public void ReturnTower()
    {
        ButtonUI.InitTower();
        ButtonUI = null;
        PoolingManager.ReturnObject(this);

    }

    public void SetTowerData(TowerInstance p_towerInstance)
    {
        towerInstance = p_towerInstance;
        TowerData.TowerDataClass towerData = towerInstance.GetTowerData();
        TowerName = towerData.TowerName;
        SynergyNames = towerData.SynergyName.ToArray();
        Cost = towerData.Cost;
        range = towerData.Range * 0.6f; // 범위 보정
        speed = towerData.Speed;
        damages = towerData.Damage.ToArray();
        Grade = 1;
        cur_damage = damages[Grade - 1];
        canAttack = true;

        for (int i = 1; i < MAX_GRADE; i++)
        {
            sprites[i] = Resources.Load<Sprite>($"Images/Towers/{TowerName}{i}");
        }

        spriteRenderer.sprite = sprites[Grade];

        for (int i = 0; i < SynergyNames.Length; i++)
        {
            if (SynergyNames[i] == SYNERGY.Island.ToString())
                isIsland = true;
            if (SynergyNames[i] == SYNERGY.Continent.ToString())
                isContinent = true;

            Synergy synergy = gameObject.AddComponent(System.Type.GetType(SynergyNames[i])) as Synergy; 
            list_synergy.Add(synergy);
        }

        ChangeState(TOWER_STATE.SearchTarget);
        StartCoroutine(SearchTarget());
        if (isIsland)
            StartCoroutine(SearchAround(1.5f));
        if (isContinent)
            StartCoroutine(SearchAround(0.9f));
    }

    public void SetPosition(Vector2 p_pos)
    {
        transform.position = new Vector3(p_pos.x, p_pos.y + 0.05f, 0);
    }

    public void SetPositionFromScreen(Vector2 p_pos)
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(p_pos);
        transform.position = new Vector3(targetPos.x, targetPos.y + 0.05f, 0);
    }

    public void SpawnBullet()
    {
        Bullet bullet = (Bullet)PoolingManager.GetObject(gameObject, "Bullet");
        bullet.transform.position = new Vector3(transform.position.x, transform.position.y, 1);
        bullet.Init(target, GetDamage(), decreaseMonsterSpeed, decreaseArmor, aroundDamage, trueDamage);
        bullet.IgnoreArmor(ignoreArmor);
        bullet.DamageAround(isDamageAround);
        list_bullet.Enqueue(bullet);
        SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 2, 0.25f);
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
        return speed * (1 - increaseSpeed);
    }

    public float GetCurrentRange()
    {
        return range * (1 + increaseRange);
    }

    //최종 타워 데미지 환산 메소드
    private float GetDamage()
    {
        float rand = Random.Range(0f, 1f);
        if(isDamageBuff && rand < percent_damageBuff)
            cur_damage = damages[Grade - 1] * (1 + increaseAttack) * 2;
        else
            cur_damage = damages[Grade - 1] * (1 + increaseAttack);
        return cur_damage;
    }

    public void Upgrade()
    {
        ParticleSystem particle = EffectManager.GetUpgradeParticle(gameObject);
        EffectManager.ReturnUpgradeParticle(particle);
        Grade++;
        spriteRenderer.sprite = sprites[Grade];
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
                
                if (dist <= GetCurrentRange() && dist <= closetDist)
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
            if (dist >= GetCurrentRange())
            {
                target = null;
                ChangeState(TOWER_STATE.SearchTarget);
                StartCoroutine(SearchTarget());
                break;
            }

            if (MonsterManager.IsPoolingObject(target))
                SpawnBullet();
            else
                target = null;
            
            yield return new WaitForSeconds(GetCurrentSpeed());
        }
    }

    IEnumerator SearchAround(float radius)
    {
        while (true)
        {
            yield return null;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, AroundTowerLayer);
            if (isIsland)
            {
                if (colliders.Length == 1)
                    ignoreArmor = true;
                else
                    ignoreArmor = false;
            }

            if (isContinent && colliders.Length >= 1)
            {
                trueDamageCount = colliders.Length - 1;
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
        isDamageBuff = percent_damageBuff != 0;
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
        isDamageAround = p != 0;
        aroundDamage = GetCurrentDamage() * p;
    }

    //대륙
    public void IncreaseTrueDamage(float p_damage)
    {
        trueDamageWeight = p_damage;
    }
    
    #endregion

}