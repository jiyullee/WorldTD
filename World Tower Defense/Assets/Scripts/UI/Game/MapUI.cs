using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviourSubUI
{
    public static MapUI Instance;
    
    private List<TowerButtonUI> list_buttonUI = new List<TowerButtonUI>();
    public override void Init()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateButtons();
    }

    private void CreateButtons()
    {
        Vector3[] pos_towers = MapManager.Instance.list_towerPos.ToArray();

        for (int i = 0; i < pos_towers.Length; i++)
        {
            GameObject buttonObject = new GameObject("TowerButton");
            buttonObject.layer = 9;
            buttonObject.AddComponent<Button>();
            buttonObject.AddComponent<Image>();
            TowerButtonUI buttonUI = buttonObject.AddComponent<TowerButtonUI>();
            buttonUI.Init();

            Vector2 pos = Camera.main.WorldToScreenPoint(pos_towers[i]);
            buttonObject.GetComponent<RectTransform>().SetPositionAndRotation(pos, Quaternion.identity);
            buttonObject.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
            buttonObject.transform.SetParent(transform.Find("Buttons"));
            
            list_buttonUI.Add(buttonUI);
        }
    }

    public void SetViewSelectableButtons(bool state)
    {
        for (int i = 0; i < list_buttonUI.Count; i++)
        {
            list_buttonUI[i].SetView(state);
        }
    }
}
