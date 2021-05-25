using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIManager : MonoBehaviourSubUI
{
    public static LobbyUIManager Instance { get; private set; }
    
    public Dictionary<UIState, MonoBehaviourSubUI> uiList = new Dictionary<UIState, MonoBehaviourSubUI>();
    private List<Text> list_texts = new List<Text>();
    void Awake()
    {
        Instance = this;
        Init();
    }
    
    private void Start()
    {
        list_texts = GetComponentsInChildren<Text>().ToList();
        for (int i = 0; i < list_texts.Count; i++)
        {
            list_texts[i].font = FontManager.Instance.GetFont(1);
        }
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

    public void SetUI(UIState state, bool active)
    {
        if (uiList.ContainsKey(state))
        {
            uiList[state].SetView(active);
        }
    }
}
