using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviourSubUI
{
    public static MapUI Instance;

    public GameObject obj_towerButton;
    public GameObject obj_arrows;
    private List<TowerButtonUI> list_buttonUI = new List<TowerButtonUI>();
    private Animator _animator;
    
    public override void Init()
    {
        Instance = this;
        obj_towerButton = transform.Find("Buttons/TowerButton").gameObject;
        obj_arrows = transform.Find("Arrows").gameObject;
        _animator = transform.Find("Arrows").GetComponent<Animator>();
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
            GameObject buttonObject = Instantiate(obj_towerButton);
            TowerButtonUI buttonUI = buttonObject.GetComponent<TowerButtonUI>();
            buttonObject.transform.position = Camera.main.WorldToScreenPoint(pos_towers[i]);
            buttonObject.transform.SetParent(transform.Find("Buttons"), true);
            buttonUI.Init(); 
            list_buttonUI.Add(buttonUI);
        }
        obj_towerButton.SetActive(false);
    }

    public void SetViewSelectableButtons(bool state)
    {
        for (int i = 0; i < list_buttonUI.Count; i++)
        {

            if (!list_buttonUI[i].isPlaceTower)
            {
                list_buttonUI[i].SetView(state);
                list_buttonUI[i].SetInteractable(state);
            }
        }
    }

    public void PlayRoadAnim()
    {
        obj_arrows.SetActive(true);
        _animator.Play("Arrow");
    }

    public void StopRoadAnim()
    {
        obj_arrows.SetActive(false);
        _animator.Play("Entry");
    }
}
