using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonUI : MonoBehaviourSubUI
{
    private bool isPlaceTower;
    private Button button;
    private Image image;
    private Tower tower;
    public override void Init()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        AddButtonEvent(button, Select);
        isPlaceTower = false;
        SetView(false);
        image.color = Color.blue;
    }

    public override void SetView(bool state)
    {
        if (!isPlaceTower)
        {
            gameObject.SetActive(state);
        }
        else
        {
            image.color = new Color(0,0,255,0);
        }
    }

    public void SetView()
    {
        //타워 배치 중이면 미배치된 타워 버튼 ON
        //타워 배치 중이 아니라면 배치된 타워 버튼 ON
        if (StoreManager.isSelecting)
        {
            
            gameObject.SetActive(!isPlaceTower);
        }
        else
        {
            gameObject.SetActive(isPlaceTower);
        }
            
    }
    
    private void Select()
    {
        if (!isPlaceTower)
        {
            //타워 생성
            isPlaceTower = true;
            tower = StoreManager.Instance.CreateTower(transform.position);     
        }
        else
        {
            if (tower != null)
            {
                tower.SetTowerViewUI();
            }
        }
       
    }
}
