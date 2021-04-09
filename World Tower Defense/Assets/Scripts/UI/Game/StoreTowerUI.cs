using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//상점 내 타워 기물을 표시하는 UI
public class StoreTowerUI : MonoBehaviourSubUI
{
    private TowerInstance towerInstance;
    private Text text_towerName;
    private Text text_cost;
    private Image img_cost;
    private Button button;
    public override void Init()
    {
        button = transform.Find("Button").GetComponent<Button>();
        AddButtonEvent(button, SelectTowerPos);
        text_towerName = transform.Find("Info/TowerName").GetComponent<Text>();
        text_cost = transform.Find("Info/Cost/Text").GetComponent<Text>();
        InitTower();
    }

    private void InitTower()
    {
        towerInstance = null;
        text_towerName.text = "";
        text_cost.text = "";
        SetActiveButton(false);
    }
    
    public void SetTower(TowerInstance p_towerInstance)
    {
        towerInstance = p_towerInstance;
        text_towerName.text = towerInstance.GetTowerData().TowerName;
        text_cost.text = towerInstance.GetTowerData().Cost.ToString();
        SetActiveButton(true);
    }

    private void SelectTowerPos()
    {
        if (StoreManager.isSelecting || towerInstance == null)
        {
            SetActiveButton(false);
            return;
        }
        
        //StoreManager에 타워 정보 전달 -> 타워 배치 가능한 버튼들 ON & 기물 구매 OFF
        StoreManager.Instance.SetSelectedInstance(towerInstance);
        StoreManager.Instance.BuyTower();
        MapUI.Instance.SetViewSelectableButtons();
        StoreUI.Instance.SetActiveButtons(false);
        InitTower();
    }

    public void SetActiveButton(bool state)
    {
        button.interactable = state;
        if (towerInstance == null)
            button.interactable = false;
    }
 
    
    

}
