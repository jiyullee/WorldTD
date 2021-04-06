using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonUI : MonoBehaviourSubUI
{
    private bool canSelect;
    private Button button;
    private Image image;
    public override void Init()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        AddButtonEvent(button, Select);
        canSelect = true;
    }

    public void Selectable(bool state)
    {

        if (!state) 
            image.color = new Color(1,0,0,0);
        else
            image.color =  new Color(0,0,1,1);
    }

    private void Select()
    {
        //타워 생성
        StoreManager.Instance.CreateTower(transform.position);
    }
}
