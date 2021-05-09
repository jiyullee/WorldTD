using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviourSubUI
{
    public static StoreUI Instance;
    private Text text_gold;
    private Text text_exp;
    private Text text_level;
    private Image image_expBar;
    private GameObject _storeTower;
    private StoreTowerUI[] storeTowers = new StoreTowerUI[5];
    private Button btn_refresh;
    public override void Init()
    {
        Instance = this;

        text_gold = transform.Find("Bottom/State/GoldText/Text").GetComponent<Text>();
        text_level = transform.Find("Bottom/State/LevelText/Text").GetComponent<Text>();
        text_exp = transform.Find("Bottom/State/LevelText/ExpText/Text").GetComponent<Text>();
        image_expBar = transform.Find("Bottom/State/ExpBar/Bar").GetComponent<Image>();

        btn_refresh = transform.Find("Bottom/RefreshBtn").GetComponent<Button>();
        AddButtonEvent(btn_refresh, () =>
        {
            StoreManager.Instance.RefreshStore(true);
        });
        AddButtonEvent("Bottom/LvUpBtn", ExpUp);
        _storeTower = transform.Find("Top/Scroll View/Viewport/StoreTower").gameObject;
        for (int i = 0; i < storeTowers.Length; i++)
        {
            GameObject storeTower = Instantiate(_storeTower);
            storeTowers[i] = storeTower.GetComponent<StoreTowerUI>();
            storeTowers[i].transform.parent = transform.Find("Top/Scroll View/Viewport");
            storeTowers[i].gameObject.SetActive(true);
            storeTowers[i].Init();
        }

        
        for (int i = 0; i < storeTowers.Length; i++)
        {
            storeTowers[i].Init();
        }
        
    }

    public void SetActiveButtons(bool state)
    {
        for (int i = 0; i < storeTowers.Length; i++)
        {
            storeTowers[i].SetActiveButton(state);
        }
        btn_refresh.interactable = state;
    }

    public void Refresh(int idx, TowerInstance p_towerInstance)
    {
        storeTowers[idx].SetTower(p_towerInstance);
    }

    public void ExpUp()
    {
        StoreManager.Instance.ExpUp(4);
    }

    public void SetGoldUI(int p_gold, int p_interest)
    {
        text_gold.text = $"{p_gold}(+{p_interest})";
    }

    public void SetLevelUI(int p_level)
    {
        text_level.text = $"{p_level} 레벨";
    }

    public void SetExpUI(int p_exp, int p_maxExp, bool isMaxLevel = false)
    {
        if (isMaxLevel)
        {
            text_exp.text = "MAX";
            image_expBar.fillAmount = 1f;
        }
        else
        {
            text_exp.text = $"{p_exp}/{p_maxExp}";
            image_expBar.fillAmount = (float)p_exp / p_maxExp;    
        }
        
    }
}
