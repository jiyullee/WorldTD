using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameData;
public class Monster : PollingObject
{
    [SerializeField]
    protected float[] weight = { 1, 1.25f, 1.5f };

    [SerializeField] private int hp;
    [SerializeField] private int amor;
    
    private Difficulty difficulty;
    public Difficulty Difficulty
    {
        get => difficulty;
        set => difficulty = value;
    }
    private void Start()
    {
        SetDifficulty();
    }

    public void SetMonsterData(int stage)
    {
        hp = MonsterData.Instance.GetTableData(stage).HP;
        amor = MonsterData.Instance.GetTableData(stage).Armor;
    }
    
    protected void SetDifficulty()
    {
        hp *= (int)weight[(int)difficulty];
        amor *= (int)weight[(int)difficulty];
    }

    public override void OnCreated()
    {
    }

    public override void OnInitiate()
    {
    }
}
