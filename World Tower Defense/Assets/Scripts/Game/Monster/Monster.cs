using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
public class Monster : PollingObject
{
    #region variable
    [SerializeField] private SpriteRenderer spriteRenderer;
    // public AnimationCurve moveCurve;
    private Transform[] map;
    private Transform thisTransform;
    [SerializeField] [Range(1, 5)] private float moveSpeed = 1;
    private float initMoveSpeed;
    protected Color color;
    protected Color hitColor;
    private float hp;
    private float armor;
    private int index = 1;
    private int maxIndex;
    private string info;

    public LayerMask AroundMonsterLayer;
    private bool IsTarget => spriteRenderer.sprite != null;
    #endregion

    #region UnityCircle

    public override void OnCreated()
    {
        if (spriteRenderer == null)
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        thisTransform = this.gameObject.transform;
        map = LoadManager.Instance.GetMap();
        maxIndex = map.Length;
        GetComponent<CircleCollider2D>().radius = 0.06f;
        color = spriteRenderer.color;
        hitColor = color;
        hitColor.a = 0.5f;
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

    #region Setting

    /// <summary>
    /// 몬스터의 데이터를 세팅해줌.
    /// 스테이지를 통해 데이터를 불러옴, 스프라이트는 외부 할당
    /// </summary>
    public void SetMonsterData(int stage, Sprite sprite)
    {
        hp = MonsterData.Instance.GetTableData(stage).HP;
        armor = MonsterData.Instance.GetTableData(stage).Armor;
        initMoveSpeed = MonsterData.Instance.GetTableData(stage).Speed;
        info = MonsterData.Instance.GetTableData(stage).info;
        spriteRenderer.sprite = sprite;
        SetDifficulty();
        moveSpeed = initMoveSpeed;
        //현재 직접 리스트를 추가하고 있지만 MonsterSpawner 함수에서 추가하도록 수정해야함
        if (IsTarget)
            MonsterSpawner.spawned_monsters.Add(this);
    }

    /// <summary>
    /// 난이도 조절 함수 차후에 GameManager에서 받아올것.
    /// </summary>
    protected void SetDifficulty()
    {
        hp = LevelManager.Instance.SetHpWeight(hp);
        initMoveSpeed = LevelManager.Instance.SetSpeedWeight(initMoveSpeed);
    }



    #endregion


    #region Move

    /// <summary>
    /// 몬스터가 움직이는 함수
    /// </summary>
    private void Move()
    {
        Vector3 dir = map[index].position - transform.position;
        // thisTransform.position = Vector3.Lerp(thisTransform.position, map[index].position, Time.deltaTime * moveSpeed);
        thisTransform.position += dir.normalized * Time.deltaTime * moveSpeed;
        if ((dir).magnitude < 0.1f)
        {
            index++;
            Look();
            if (index >= map.Length)
            {
                index = 0;
                Polling2.ReturnObject(this);
                int damage = (info == "Boss") ? 5 : 1;
                Gamemanager.Instance.Damage(damage);
                //현재 직접 리스트를 제거하고 있지만 MonsterSpawner 함수에서 제거하도록 수정해야함
                if (MonsterSpawner.spawned_monsters.Contains(this))
                    MonsterSpawner.spawned_monsters.Remove(this);
            }
        }
    }

    /// <summary>
    /// 몬스터가 갈 보는 함수
    /// </summary>
    private void Look()
    {
        if (index >= maxIndex)
            return;
        Vector3 dir = map[index].position - map[index - 1].position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    #endregion

    #region Interaction

    /// <summary>
    /// 데미지를 받는 함수
    /// 체력이 0이하로 내려가게 되면 풀링풀에 반환해줌
    /// </summary>
    public void GetDamage(float dmg, bool ignoreArmor, float decreaseArmor, float aroundDamage, float trueDamage)
    {
        Transform tempPos = transform;
        if (!ignoreArmor)
        {
            armor -= decreaseArmor;
            armor = armor - decreaseArmor >= 0 ? armor - decreaseArmor : 0;
            hp = hp + armor - dmg;
            hp -= trueDamage;
        }
        else
        {
            hp -= dmg + trueDamage;
        }

        DamageAround(aroundDamage);
        if (hp <= 0)
        {
            ParticleSystem particle = EffectManager.GetParticle(tempPos);
            EffectManager.ReturnParticle(particle);

            index = 0;
            if (MonsterSpawner.spawned_monsters.Contains(this))
                MonsterSpawner.spawned_monsters.Remove(this);
            Polling2.ReturnObject(this);
            if (MonsterSpawner.spawned_monsters.Contains(this))
                MonsterSpawner.spawned_monsters.Remove(this);
        }
        ChangeColor();
    }

    public void GetDamage(float aroundDamage)
    {
        Transform tempPos = transform;
        hp -= aroundDamage;
        if (hp <= 0)
        {
            ParticleSystem particle = EffectManager.GetParticle(tempPos);
            EffectManager.ReturnParticle(particle);

            index = 0;
            Polling2.ReturnObject(this);
        }

        ChangeColor();
    }

    public void DamageAround(float aroundDamage)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f, AroundMonsterLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            Monster monster = colliders[i].GetComponent<Monster>();
            monster.GetDamage(aroundDamage);
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
}
