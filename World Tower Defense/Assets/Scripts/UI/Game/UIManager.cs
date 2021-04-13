using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviourSubUI
{
    public static UIManager Instance;
    
    public Dictionary<UIState, MonoBehaviourSubUI> uiList = new Dictionary<UIState, MonoBehaviourSubUI>();

    //현재 보여주는 UI
    protected MonoBehaviourSubUI selectUI;

    void Awake()
    {
        Instance = this;
        Init();
    }
    public override void Init()
    {
        Instance = this;
        
        AddUI(UIState.StateUI,"StateUI", "StateUI");
        AddUI(UIState.StoreUI, "StoreUI", "StoreUI");
        AddUI(UIState.GameOptionUI, "OptionUI", "GameOptionUI");
        AddUI(UIState.MapUI, "MapUI", "MapUI");
        AddUI(UIState.SynergyUI, "SynergyUI", "SynergyUI");
        AddUI(UIState.TowerUI, "TowerUI", "TowerUI");
        
        foreach (var data in uiList)
        {
            if(data.Value)
                data.Value.Init();
        }
    }
    
    /// <summary>
    /// 해당 UI를 등록하는 메소드
    /// </summary>
    /// <param name="state"></param>
    /// <param name="path"></param>
    /// <param name="script"></param>
    protected void AddUI(UIState state, string path, string script = "")
    {
        if (!string.IsNullOrEmpty(script))
        {
            uiList.Add(state, transform.Find(path).gameObject.AddComponent(System.Type.GetType(script)) as MonoBehaviourSubUI);
        }
        else
        {
            uiList.Add(state, transform.Find(path).GetComponent<MonoBehaviourSubUI>());
        }
            
    }

    /// <summary>
    /// 해당 UI를 보여주는 메소드
    /// </summary>
    /// <param name="state"></param>
    public void SetUI(UIState state)
    {
        if (uiList.ContainsKey(state))
        {
            if (selectUI) selectUI.SetView(false);

            selectUI = uiList[state];

            if (selectUI) selectUI.SetView(true);
        }
        else
            selectUI = null;
    }

    public void SetTowerView(bool state)
    {
        if (TowerUI.Instance != null && TowerUI.Instance.gameObject.activeSelf)
        {
            TowerUI.Instance.SetView(state);
        }
    }
}
