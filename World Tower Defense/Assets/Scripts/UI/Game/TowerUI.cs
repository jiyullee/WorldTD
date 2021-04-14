using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviourSubUI
{
    public static TowerUI Instance;
    private Text text_towerName;
    private Text text_damage;
    private Text text_speed;
    private Text text_range;
    private GameObject[] objs_grade;

    private Tower tower;
    public override void Init()
    {
        Instance = this;
        text_towerName = transform.Find("TowerInfo/TowerNameText").GetComponent<Text>();
        text_damage = transform.Find("TowerInfo/DamageText").GetComponent<Text>();
        text_speed = transform.Find("TowerInfo/SpeedText").GetComponent<Text>();
        text_range = transform.Find("TowerInfo/RangeText").GetComponent<Text>();
        objs_grade = new GameObject[3];
        for (int i = 0; i < objs_grade.Length; i++)
        {
            objs_grade[i] = transform.Find($"Grade/Image{i}").gameObject;     
        }
       
        AddButtonEvent("CompoundButton", CompoundTower);
        SetView(false);
    }
    

    public void SetPosition(Vector3 p_pos)
    {
        SetView(true);
        transform.position = Camera.main.WorldToScreenPoint(p_pos);
        if(transform.position.x < 110f)
            transform.position = new Vector3(120f, transform.position.y, transform.position.z);
        else if(transform.position.x > 610f)
            transform.position = new Vector3(620f, transform.position.y, transform.position.z);
    }

    public void SetTexts(Tower p_tower, string p_name, float p_damage, float p_speed, float p_range, int p_grade)
    {
        tower = p_tower;
        text_towerName.text = p_name;
        text_damage.text = $"공격력 : {p_damage}";
        text_speed.text = $"공격 속도 : {p_speed}";
        text_range.text = $"공격 범위 : {p_range}";
        SetGrade(p_grade);
    }

    private void SetGrade(int p_grade)
    {
        for (int i = 0; i <= p_grade - 1; i++)
        {
            objs_grade[i].SetActive(true);
        }

        for (int i = p_grade; i < objs_grade.Length; i++)
        {
            objs_grade[i].SetActive(false);
        }
    }

    public void CompoundTower()
    {
        if (tower != null)
        {
            TowerManager.Instance.CompoundTower(tower);

        }
    }
    
}
