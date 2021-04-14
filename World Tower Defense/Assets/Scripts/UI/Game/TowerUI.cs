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
    private Button btn_move;
    private Button btn_info;
    private Button btn_compound;
    private Text text_towerName;
    private Text text_damage;
    private Text text_speed;
    private Text text_range;
    private GameObject[] objs_grade;

    public static bool IsMoving;
    private Tower tower;
    
    public override void Init()
    {
        Instance = this;
        obj_info = transform.Find("InfoUI").gameObject;
        obj_menu = transform.Find("Buttons").gameObject;
        btn_move = obj_menu.transform.Find("MoveButton").GetComponent<Button>();
        btn_info = obj_menu.transform.Find("InfoButton").GetComponent<Button>();
        
        AddButtonEvent(btn_move, MoveTower);
        AddButtonEvent(btn_info, ShowInfo);
        
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
        AddButtonEvent(btn_compound, CompoundTower);
        
        SetView(false);
    }

    public override void SetView(bool state)
    {
        obj_menu.SetActive(state);
        obj_info.SetActive(state);
        gameObject.SetActive(state);
    }

    private void MoveTower()
    {
        SetView(false);
        UIManager.Instance.SetEventButton(false);
        IsMoving = true;
        TowerManager.Instance.AddTower_Swap(tower.ButtonUI);
        MapUI.Instance.SetViewSelectableButtons(true);
    }

    private void ShowInfo()
    {
        obj_menu.SetActive(false);
        obj_info.SetActive(true);
    }

    private void ShowMenu()
    {
        gameObject.SetActive(true);
        obj_menu.SetActive(true);
        obj_info.SetActive(false);
    }

    public void SetPosition(Vector3 p_pos)
    {
        ShowMenu();
        
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
            TowerManager.Instance.CompoundTower(tower);
        }
    }
    
}
