using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonUI : MonoBehaviourSubUI
{
    private bool isPlaceTower;
    private Button button;
    private Image image;
    public Tower tower { get; private set; }
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
        if(TowerUI.IsMoving)
        {
            gameObject.SetActive(true);
            return;
        }
        
        if(state)
            gameObject.SetActive(!isPlaceTower);
        else
            gameObject.SetActive(isPlaceTower);
    }

    public void InitTower()
    {
        tower = null;
        isPlaceTower = false;
        image.color = new Color(0,0,255,1);
        SetView(false);
    }
    
    private void SelectTower()
    {
        if (TowerUI.IsMoving)
        {
            TowerUI.IsMoving = false;
            TowerManager.Instance.AddTower_Swap(this);
            MapUI.Instance.SetViewSelectableButtons(false);
            TowerManager.Instance.SwapPos();
            return;
        }
        
        if (!isPlaceTower)
        {
            //타워 생성
            isPlaceTower = true;
            image.color = new Color(0,0,255,0);
            tower = StoreManager.Instance.CreateTower(transform.position);
            tower.SetButtonUI(this);
        }
        else
        {
            //타워 UI 띄우기
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
