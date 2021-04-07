using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonUI : MonoBehaviourSubUI
{
    private bool placeTower;
    private Button button;
    private Image image;
    public override void Init()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        AddButtonEvent(button, Select);
        placeTower = false;
        SetView(false);
        image.color = Color.blue;
    }
    public void SetView()
    {
        //타워 배치 중이면 미배치된 타워 버튼 ON
        //타워 배치 중이 아니라면 배치된 타워 버튼 ON
        if (StoreManager.isSelecting)
        {
            gameObject.SetActive(!placeTower);
        }
        else
        {
            gameObject.SetActive(placeTower);
        }
            
    }
    
    private void Select()
    {
        //타워 생성
        placeTower = true;
        StoreManager.Instance.CreateTower(transform.position);
    }
}
