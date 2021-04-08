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
    private Difficulty difficulty;
    [SerializeField] [Range(1, 5)] private float moveSpeed = 1;
    protected float[] weight = { 1f, 1.25f, 1.5f };
    private int hp;
    private int amor;
    private int index = 1;
    private int maxIndex;

    #endregion

    #region UnityCircle

    public override void OnCreated()
    {
        if (spriteRenderer == null)
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        thisTransform = this.gameObject.transform;
        map = LoadManager.Instance.GetMap();
        difficulty = Gamemanager.Instance.Difficulty;
        maxIndex = map.Length;
        GetComponent<CircleCollider2D>().radius = 0.06f;
    }

    public override void OnInitiate() { }

    private void Update()
    {
        Move();
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
        amor = MonsterData.Instance.GetTableData(stage).Armor;
        moveSpeed = MonsterData.Instance.GetTableData(stage).Speed;
        spriteRenderer.sprite = sprite;
        SetDifficulty();
        //현재 직접 리스트를 추가하고 있지만 MonsterSponer의 함수에서 추가하도록 수정해야함
        if (spriteRenderer.sprite != null) 
            MonsterSponer.Instance.spawned_monsters.Add(this);
    }

    /// <summary>
    /// 난이도 조절 함수 차후에 GameManager에서 받아올것.
    /// </summary>
    protected void SetDifficulty()
    {
        hp = (int)(weight[(int)difficulty] * (float)hp);
        amor = (int)(weight[(int)difficulty] * (float)amor);
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
            Look(dir);
            index++;
            if (index >= map.Length)
            {
                index = 0;
                Polling2.ReturnObject(this);
                
                //현재 직접 리스트를 제거하고 있지만 MonsterSponer의 함수에서 제거하도록 수정해야함
                if(MonsterSponer.Instance.spawned_monsters.Contains(this))
                    MonsterSponer.Instance.spawned_monsters.Remove(this);
            }
        }
    }

    /// <summary>
    /// 몬스터가 갈 보는 함수
    /// </summary>
    private void Look(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    #endregion

    #region Interaction

    /// <summary>
    /// 데미지를 받는 함수
    /// 체력이 0이하로 내려가게 되면 풀링풀에 반환해줌
    /// </summary>
    public void GetDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            index = 0;
            Polling2.ReturnObject(this);
        }
    }

    /// <summary>
    /// 알파값을 낮춰서 색을 바꿔주는 함수.
    /// 시간값을 주어야 하기 때문에 코루틴 사용
    /// 하지만 아마 안쓸 예정...
    /// </summary>
    public void changeColor()
    {
        StartCoroutine("IGetDamage");
    }

    IEnumerator IGetDamage()
    {
        Color tmp = spriteRenderer.color;
        tmp.a = 0.5f;
        spriteRenderer.color = tmp;
        yield return new WaitForSeconds(0.5f);
        tmp.a = 1f;
        spriteRenderer.color = tmp;

    }

    #endregion
}
