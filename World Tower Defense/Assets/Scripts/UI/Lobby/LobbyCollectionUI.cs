using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCollectionUI : MonoBehaviourSubUI
{
    public static LobbyCollectionUI Instance;
    private GameObject obj_collection;
    private GameObject panel_filter;
    private GameObject panel_info;
    private Image img_tower;
    private Text text_name;
    private Text text_damage;
    private Text text_speed;
    private Text text_range;
    private Text text_cost;
    
    public override void Init()
    {
        Instance = this;
        AddButtonEvent("BackBtn", () => LobbyUIManager.Instance.SetUI(UIState.CollectionUI, false));
        AddButtonEvent("InfoPanel/CloseBtn", CloseInfoPanel);
        panel_info = transform.Find("InfoPanel").gameObject;
        panel_filter = transform.Find("Filter").gameObject;
        img_tower = panel_info.transform.Find("Image").GetComponent<Image>();
        text_name = panel_info.transform.Find("TowerNameText").GetComponent<Text>();
        text_damage = panel_info.transform.Find("Damage/DamageText").GetComponent<Text>();
        text_speed = panel_info.transform.Find("Speed/SpeedText").GetComponent<Text>();
        text_range = panel_info.transform.Find("Range/RangeText").GetComponent<Text>();
        text_cost = panel_info.transform.Find("Cost/CostText").GetComponent<Text>();
        
        obj_collection = transform.Find("Scroll View/Viewport/Content/Button").gameObject;
        GameObject content = transform.Find("Scroll View/Viewport/Content").gameObject;
        int cnt_collection = TowerData.Instance.GetTable().Count;

        for (int i = 0; i < cnt_collection; i++)
        {
            GameObject obj_clt = Instantiate(obj_collection);
            obj_clt.SetActive(true);
            obj_clt.transform.SetParent(content.transform);
            obj_clt.GetComponent<Collection>().Init();
            obj_clt.GetComponent<Collection>().InitTowerInfo(new TowerInstance(i));
        }
        panel_info.SetActive(false);
        panel_filter.SetActive(false);
    }

    public void SetInfoPanel(TowerInstance p_towerInstance)
    {
        panel_info.SetActive(true);
        panel_filter.SetActive(true);
        TowerData.TowerDataClass towerDataClass = p_towerInstance.GetTowerData();
        text_name.text = towerDataClass.TowerName;
        if (text_name.text.Length >= 7)
            text_name.fontSize = 27;
        else
            text_name.fontSize = 31;
        
        string damage = "";
        for (int i = 0; i < towerDataClass.Damage.Count; i++)
        {
            if (i != towerDataClass.Damage.Count - 1)
                damage += $"{towerDataClass.Damage[i]}/";
            else
                damage += $"{towerDataClass.Damage[i]} ★";
        }
   
        img_tower.sprite = Resources.Load<Sprite>($"Image/Flags/{text_name.text}");
        text_damage.text = damage;
        text_speed.text = towerDataClass.Speed.ToString();
        text_range.text = towerDataClass.Range.ToString();
        text_cost.text = towerDataClass.Cost.ToString();
    }

    public void CloseInfoPanel()
    {
        panel_info.SetActive(false);
        panel_filter.SetActive(false);
    }

}
