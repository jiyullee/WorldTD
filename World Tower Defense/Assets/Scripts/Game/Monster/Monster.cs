using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
public class Monster : PollingObject
{
    #region Fields

    private SpriteRenderer spriteRenderer;
    // public AnimationCurve moveCurve;
    private Transform[] map;
    private Transform thisTransform;
    [SerializeField] [Range(1, 5)] private float moveSpeed;
    private float initMoveSpeed;
    protected Color color;
    protected Color hitColor;
    private float hp;
    private float armor;
    private float initArmor;
    private int index = 1;
    private int spriteIndex;
    private int maxIndex;
    private string info;
    public bool isBoss { get; private set; }
    private float time;
    private float targetDistance;
    private float correctionSpeed = 2;
    [SerializeField] float requiredTime;
    public LayerMask AroundMonsterLayer;
    private bool IsTarget => MonsterManager.IsPoolingObject(this);

    #endregion

    #region CallBacks

    public override void OnCreated()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        thisTransform = this.gameObject.transform;
        map = LoadManager.Instance.GetMap();
        maxIndex = map.Length;
        GetComponent<CircleCollider2D>().radius = 0.06f;
        color = spriteRenderer.color;
        hitColor = color;
        hitColor.a = 0.5f;
        CalculationRequiredTime();
    }

    public override void OnInitiate()
    {

    }

    private void LateUpdate()
    {
        if (IsTarget)
            Move();
    }
    private void OnEnable()
    {
        Look();
        ResetColor();
    }

    #endregion

    #region Functions

    /// <summary>
    /// 몬스터의 데이터를 세팅해줌.
    /// 스테이지를 통해 데이터를 불러옴, 스프라이트는 외부 할당
    /// </summary>
    public void SetMonsterData(string monster)
    {
        isBoss = (StageManager.Instance.Stage % 5 == 0);
        int monsterKey = Convert.ToInt32(monster);
        hp = MonsterAssocationData.Instance.GetTableData(monsterKey).HP;
        if (isBoss == false)
        {
            hp = hp * Mathf.Pow(1.1f, (int)(StageManager.Instance.Stage / 5));
        }
        armor = MonsterAssocationData.Instance.GetTableData(monsterKey).Armor;
        initArmor = armor;
        initMoveSpeed = MonsterAssocationData.Instance.GetTableData(monsterKey).Speed;
        spriteRenderer.sprite = MonsterManager.Instance.monsterImage[MonsterAssocationData.Instance.GetTableData(monsterKey).spriteIndex];
        moveSpeed = initMoveSpeed;
    }

    /// <summary>
    /// 데미지를 받는 함수
    /// 체력이 0이하로 내려가게 되면 풀링풀에 반환해줌
    /// </summary>
    public void Damage(float dmg, bool ignoreArmor, float decreaseArmor, float trueDamage)
    {
        if (!ignoreArmor)
        {
            armor = initArmor - decreaseArmor;
            armor = armor >= 0 ? armor : 0;
            dmg = armor - dmg >= 0 ? 0 : dmg - armor;
        }
        else
        {
            armor = 0;
        }
        dmg = armor - dmg >= 0 ? 0 : dmg - armor;
        hp -= dmg;
        hp -= trueDamage;
        if (hp <= 0)
        {
            SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 4);
            ParticleSystem particle = EffectManager.GetDestroyParticle(gameObject);
            EffectManager.ReturnDestroyParticle(particle);
            index = 0;
            PoolingManager.ReturnObject(this);
            MonsterManager.Instance.RemoveMonster(this);
        }
        ChangeColor();
    }

    public void Damage(float aroundDamage)
    {
        hp -= aroundDamage;
        if (hp <= 0)
        {
            ParticleSystem particle = EffectManager.GetDestroyParticle(gameObject);
            EffectManager.ReturnDestroyParticle(particle);
            index = 0;
            PoolingManager.ReturnObject(this);
        }

        ChangeColor();
    }

    public void DamageAround(float aroundDamage)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.6f, AroundMonsterLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            Monster monster = colliders[i].GetComponent<Monster>();
            if(monster != null)
                monster.Damage(aroundDamage);
        }
    }



    /// <summary>
    /// 알파값을 낮춰서 색을 바꿔주는 함수.
    /// 중간에 꺼지면 코루틴 오류가 나기 떄문에 invoke사용
    /// </summary>
    void ChangeColor()
    {
        spriteRenderer.color = hitColor;
        Invoke("ResetColor", 0.5f);
    }

    void ResetColor()
    {
        spriteRenderer.color = color;
    }

    public void DecreaseSpeed(float p_decrease)
    {
        moveSpeed = initMoveSpeed * (1 - p_decrease);
    }

    #endregion

    #region Functions_Move
    void CalculationRequiredTime()
    {
        targetDistance = Vector3.Magnitude(map[index - 1].position - map[index].position);
        requiredTime = targetDistance / moveSpeed / correctionSpeed;
    }

    /// <summary>
    /// 몬스터가 움직이는 함수
    /// </summary>
    private void Move()
    {
        time += Time.deltaTime * moveSpeed / correctionSpeed;
        time = Mathf.Clamp(time, 0f, requiredTime);

        thisTransform.position = Vector3.Lerp(map[index - 1].position, map[index].position, time / requiredTime);
        if (time >= requiredTime)
        {
            time = 0;
            index++;
            if (index >= map.Length)
            {
                index = 1;
                PoolingManager.ReturnObject(this);
                MonsterManager.Instance.RemoveMonster(this);
                int damage = (isBoss) ? 5 : 1;
                GameManager.Instance.Damage(damage);
            }
            CalculationRequiredTime();
            Look();
        }
    }

    /// <summary>
    /// 몬스터가 갈 보는 함수
    /// </summary>
    private void Look()
    {
        if (index >= maxIndex)
            return;
        if (index <= 0) index = 1;
        Vector3 dir = map[index].position - map[index - 1].position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    #endregion

}
