using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
public class MonsterLerp : PollingObject
{
    #region variable
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] protected AnimationCurve moveCurve;
    // public AnimationCurve moveCurve;
    private Transform[] map;
    private Transform thisTransform;
    private Difficulty difficulty;
    private float moveSpeed = 1;
    protected float[] weight = { 1f, 1.25f, 1.5f };
    private int hp;
    private int amor;
    private int index = 1;
    private int maxIndex;
    private float time;
    private float requiredTime;

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
    }

    public override void OnInitiate() { }
    private void OnEnable()
    {
        SetTarget();
    }
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
    }

    /// <summary>
    /// 난이도 조절 함수 차후에 GameManager에서 받아올것.
    /// </summary>
    protected void SetDifficulty()
    {
        hp = (int)(weight[(int)difficulty] * (float)hp);
        amor = (int)(weight[(int)difficulty] * (float)amor);
    }

    protected void SetTarget()
    {
        Vector3 distance = map[index].position - map[index - 1].position;
        requiredTime = distance.magnitude / moveSpeed;
        distance = distance.normalized;
    }


    #endregion


    #region Move

    /// <summary>
    /// 몬스터가 움직이는 함수
    /// </summary>
    private void Move()
    {
        time += Time.deltaTime * moveSpeed;
        thisTransform.position = Vector3.Lerp(map[index - 1].position, map[index].position, moveCurve.Evaluate(time / requiredTime));
        if (time >= requiredTime)
        {
            time = 0;
            index++;
            if (index >= map.Length)
            {
                index = 1;
                Polling2.ReturnObject(this);
            }
            SetTarget();
        }
    }

    /// <summary>
    /// 몬스터가 갈 보는 함수
    /// </summary>
    private void Look(Vector3 directionNomalized)
    {
        float angle = Mathf.Atan2(directionNomalized.y, directionNomalized.x) * Mathf.Rad2Deg;
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
