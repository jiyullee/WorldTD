using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

//상점 내 타워 기물을 표시하는 UI
public class StoreTowerUI : MonoBehaviourSubUI
{
    private TowerInstance towerInstance;
    private Text text_towerName;
    private Text text_synergyName;
    private Text text_cost;
    private Text text_info_name;
    private Text text_info_damage;
    private Text text_info_speed;
    private Text text_info_range;
    private Image img_cost;
    private Image img_store;
    private Button btn_flag;
    private Button btn_info;
    private Button btn_infoUI;
    private Sprite initSprite;
    public override void Init()
    {
        btn_flag = transform.Find("Button").GetComponent<Button>();
        btn_info = transform.Find("Info/InfoButton").GetComponent<Button>();
        btn_infoUI = transform.Find("Info/InfoUI").GetComponent<Button>();
        AddButtonEvent(btn_flag, SelectTowerPos);
        AddButtonEvent(btn_info, () => ShowInfo(true));
        AddButtonEvent(btn_infoUI, () => ShowInfo(false));
        text_towerName = transform.Find("Info/TowerName").GetComponent<Text>();
        text_synergyName = transform.Find("Info/SynergyName").GetComponent<Text>();
        text_cost = transform.Find("Info/Cost/Text").GetComponent<Text>();
        text_info_name = transform.Find("Info/InfoUI/NameText").GetComponent<Text>();
        text_info_damage = transform.Find("Info/InfoUI/DamageText/Text").GetComponent<Text>();
        text_info_speed = transform.Find("Info/InfoUI/SpeedText/Text").GetComponent<Text>();
        text_info_range = transform.Find("Info/InfoUI/RangeText/Text").GetComponent<Text>();
        
        img_store = transform.Find("Button/Image").GetComponent<Image>();
        initSprite = img_store.sprite;
        ShowInfo(false);
        InitTower();
    }

    private void InitTower()
    {
        towerInstance = null;
        text_towerName.text = "";
        text_synergyName.text = "";
        text_cost.text = "";
        btn_info.interactable = false;
        SetActiveButton(false);
    }
    
    public void SetTower(TowerInstance p_towerInstance)
    {
        towerInstance = p_towerInstance;
        string towerName = towerInstance.GetTowerData().TowerName;
        List<string> synergyNames = new List<string>();
        synergyNames = towerInstance.GetTowerData().SynergyName_KR;
        text_towerName.text = towerName;
        text_synergyName.text = "";
        text_cost.text = towerInstance.GetTowerData().Cost.ToString();
        img_store.sprite = StoreManager.Instance.dic_towerImage[towerName];
        for (int i = 0; i < synergyNames.Count; i++)
        {
            text_synergyName.text += synergyNames[i] + " ";
        }
        SetActiveButton(true);
        btn_info.interactable = true;
        SetInfoUI();
    }

    private void SetInfoUI()
    {
        text_info_name.text = towerInstance.GetTowerData().TowerName;
        List<float> damages = new List<float>();
        damages = towerInstance.GetTowerData().Damage;
        text_info_damage.text = "";
        for (int i = 0; i < damages.Count; i++)
        {
            if (i != damages.Count - 1)
                text_info_damage.text += $"{(int) damages[i]} / ";
            else
                text_info_damage.text += $"{(int) damages[i]} ★";
        }
        text_info_speed.text = towerInstance.GetTowerData().Speed.ToString();
        text_info_range.text = towerInstance.GetTowerData().Range.ToString() + "칸";
    }

    private void SelectTowerPos()
    {
        if (StoreManager.isSelecting || towerInstance == null)
        {
            SetActiveButton(false);
            return;
        }

        if (!TowerManager.Instance.CanBuildTower)
        {
            PopUpUI.Instance.PopUp(POPUP_STATE.OverTower);
            return;
        }
        
        //구매 가능 여부 판단
        if (!StoreManager.Instance.CanBuyTower(towerInstance.GetTowerData().Cost))
        {
            PopUpUI.Instance.PopUp(POPUP_STATE.LackGold);
            return;
        }
      
        //StoreManager에 타워 정보 전달 
        StoreManager.Instance.SetSelectedInstance(towerInstance);
        //타워 배치 가능한 버튼들 ON & 기물 구매 OFF
        StoreManager.Instance.BuyTower();
        MapUI.Instance.SetViewSelectableButtons(true);
        StoreUI.Instance.SetActiveButtons(false);
        InitTower();
        img_store.sprite = initSprite;
    }

    public void SetActiveButton(bool state)
    {
        btn_flag.interactable = state;
        if (towerInstance == null)
            btn_flag.interactable = false;
    }

    public void ShowInfo(bool state)
    {
        btn_infoUI.gameObject.SetActive(state);
    }

}
