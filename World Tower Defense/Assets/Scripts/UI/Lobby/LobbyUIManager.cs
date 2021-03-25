using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviourSubUI
{
    public static LobbyUIManager Instance { get; private set; }
    
    public Dictionary<UIState, MonoBehaviourSubUI> uiList = new Dictionary<UIState, MonoBehaviourSubUI>();

    //현재 보여주는 UI
    private MonoBehaviourSubUI selectUI;
    
    void Awake()
    {
        Instance = this;
        Init();
    }
    public override void Init()
    {
        AddUI(UIState.Lobby, "LobbyUI", "MainLobbyUI");
        AddUI(UIState.OptionUI, "OptionUI", "LobbyOptionUI");
        AddUI(UIState.CollectionUI, "CollectionUI", "LobbyCollectionUI");
        
        foreach (var data in uiList)
        {
            if(data.Value)
                data.Value.Init();
        }
    }

    public override void Release()
    {
        
    }

    void AddUI(UIState state, string path, string script = "")
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
}
