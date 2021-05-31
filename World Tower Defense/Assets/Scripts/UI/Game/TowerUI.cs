using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviourSubUI
{
    public static TowerUI Instance;
    private GameObject obj_info;
    private GameObject obj_menu;
    private Image img_flag;
    private Button btn_info;
    private Button btn_compound;
    private Button btn_sell;
    private Text text_towerName;
    private Text text_damage;
    private Text text_speed;
    private Text text_range;
    private GameObject[] objs_grade;
    
    private Tower tower;
    
    public override void Init()
    {
        Instance = this;
        obj_info = transform.Find("InfoUI").gameObject;
        obj_menu = transform.Find("Buttons").gameObject;
        btn_info = obj_menu.transform.Find("InfoButton").GetComponent<Button>();
        
        AddButtonEvent(btn_info, ShowInfo);
        img_flag = transform.Find("InfoUI/FlagImage").GetComponent<Image>();
        text_towerName = obj_info.transform.Find("TowerInfo/TowerNameText").GetComponent<Text>();
        text_damage = obj_info.transform.Find("TowerInfo/DamageText").GetComponent<Text>();
        text_speed = obj_info.transform.Find("TowerInfo/SpeedText").GetComponent<Text>();
        text_range = obj_info.transform.Find("TowerInfo/RangeText").GetComponent<Text>();
        objs_grade = new GameObject[3];
        for (int i = 0; i < objs_grade.Length; i++)
        {
            objs_grade[i] = obj_info.transform.Find($"Grade/Image{i}").gameObject;     
        }

        btn_compound = obj_info.transform.Find("CompoundButton").GetComponent<Button>();
        btn_sell = obj_info.transform.Find("SellButton").GetComponent<Button>();
        
        AddButtonEvent(btn_compound, CompoundTower);
        AddButtonEvent(btn_sell, SellTower);
        
        SetView(false);
        btn_compound.interactable = false;
 
    }

    public override void SetView(bool state)
    {
        obj_menu.SetActive(state);
        obj_info.SetActive(state);
        gameObject.SetActive(state);
    }

    private void ShowInfo()
    {
        obj_menu.SetActive(false);
        obj_info.SetActive(true);
    }

    public void SetPosition(Vector3 p_pos)
    {
        gameObject.SetActive(true);
        obj_info.SetActive(true);
        
        transform.position = p_pos;
        if(transform.position.x < 110f)
            transform.position = new Vector3(120f, transform.position.y, transform.position.z);
        else if(transform.position.x > 610f)
            transform.position = new Vector3(620f, transform.position.y, transform.position.z);
        
    }

    public void SetTexts(Tower p_tower)
    {
        
        tower = p_tower;
        img_flag.sprite = Resources.Load<Sprite>($"Images/Flags/{tower.TowerName}");
        text_towerName.text = tower.TowerName;
    
        text_damage.text = "공격력 : " + String.Format("{0:0.##}", tower.GetCurrentDamage());
        text_speed.text = "공격 속도 : " + String.Format("{0:0.##}", tower.GetCurrentSpeed());
        text_range.text = "공격 범위 : " + String.Format("{0:0.##}칸", (int)(tower.GetCurrentRange() / 0.6f));
        text_range.text = "공격 범위 : " + String.Format("{0:0.##}칸", (tower.GetCurrentRange() / 0.6f));
        
        SetGrade(tower.Grade);
        btn_compound.interactable = TowerManager.Instance.CanCompound(tower);
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
            SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 6);
            TowerManager.Instance.CompoundTower(tower);
            SetView(false);
        }
    }

    public void SellTower()
    {
        if (tower != null)
        {
            SetView(false);
            TowerManager.Instance.SellTower(tower);
        }
    }
}
