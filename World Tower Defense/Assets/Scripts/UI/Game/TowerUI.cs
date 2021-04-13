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
    private Image[] images_grade;
    public override void Init()
    {
        Instance = this;
        text_towerName = transform.Find("TowerInfo/TowerNameText").GetComponent<Text>();
        text_damage = transform.Find("TowerInfo/DamageText").GetComponent<Text>();
        text_speed = transform.Find("TowerInfo/SpeedText").GetComponent<Text>();
        text_range = transform.Find("TowerInfo/RangeText").GetComponent<Text>();

        images_grade = transform.Find("Grade").GetComponentsInChildren<Image>();
        AddButtonEvent("CompoundButton", CompoundTower);
        SetView(false);
    }

    public void SetPosition(Vector3 p_pos)
    {
        SetView(true);
        transform.position = Camera.main.WorldToScreenPoint(p_pos);
    }

    public void SetTexts(string p_name, float p_damage, float p_speed, float p_range, int p_grade)
    {
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
            images_grade[i].gameObject.SetActive(true);
        }
    }

    public void CompoundTower()
    {
        Debug.Log("타워 합성");
    }
    
}
