using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLobbyUI : MonoBehaviourSubUI
{
    public override void Init()
    {
        AddButtonEvent("MenuUI/StartBtn", StartGame);
        AddButtonEvent("MenuUI/CollectionBtn", 
            () => LobbyUIManager.Instance.SetUI(UIState.CollectionUI));
        AddButtonEvent("MenuUI/OptionBtn", () => LobbyUIManager.Instance.SetUI(UIState.OptionUI));
        AddButtonEvent("MenuUI/EndBtn", EndGame);
    }

    private void StartGame()
    {
        Debug.Log("Click_StartGame");
    }
    
    private void EndGame()
    {
        Debug.Log("Click_EndGame");
    }
}
