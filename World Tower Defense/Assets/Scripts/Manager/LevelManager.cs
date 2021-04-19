using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IsSetWeight
{
    public bool isSetHpWeight = true;
    public bool isSetAmountWeight = true;
    public bool isSetSpeedWeight = true;
}

public class LevelManager : UnitySingleton<LevelManager>
{
    /// 차후 게임 시작 창에서 setting해줄것.
    [SerializeField] private Difficulty difficulty = Difficulty.Nomal;
    protected float[] weight = { 1f, 1.25f, 1.5f };
    [SerializeField] protected IsSetWeight isSetWeight;
    public Difficulty Difficulty
    {
        get => difficulty;
    }
    public override void OnCreated()
    {

    }

    public override void OnInitiate()
    {

    }

    public float SetHpWeight(float hp)
    {
        if (isSetWeight.isSetHpWeight)
        {
            hp = (int)(weight[(int)difficulty] * (float)hp);
        }
        return hp;
    }
    public int SetamountWeight(int amount)
    {
        if (isSetWeight.isSetAmountWeight)
        {
            amount = (int)(weight[(int)difficulty] * (float)amount);
        }
        return amount;
    }
    public float SetSpeedWeight(float speed)
    {
        if (isSetWeight.isSetSpeedWeight)
        {
            speed = weight[(int)difficulty] * (float)speed;
        }
        return speed;
    }
}
