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
        AddButtonEvent(button, SelectTower);
        isPlaceTower = false;
        SetView(false);
        image.color = Color.blue;
    }

    public override void SetView(bool state)
    {
        if(state)
            gameObject.SetActive(!isPlaceTower);
        else
            gameObject.SetActive(isPlaceTower);
    }

    //타워 구매 시 호출되는 메소드
    public void SetView()
    {
        gameObject.SetActive(true);
        if (!isPlaceTower)
        {
            image.color = new Color(0,0,255,1);
            button.interactable = true;
        }
        else
        {
            image.color = new Color(0,0,255,0);
            button.interactable = false;
        }
    }

    public void SetInteratable()
    {
        button.interactable = isPlaceTower;
    }
    private void SelectTower()
    {
        
        if (!isPlaceTower)
        {
            //타워 생성
            isPlaceTower = true;
            image.color = new Color(0,0,255,0);
            tower = StoreManager.Instance.CreateTower(transform.position);
        }
        else
        {
            SetViewTowerUI();
        }
    }

    public void SetViewTowerUI()
    {
        if (tower != null)
        {
            tower.SetTowerViewUI();
            UIManager.Instance.SetEventButton(true);
        }
    }
}
