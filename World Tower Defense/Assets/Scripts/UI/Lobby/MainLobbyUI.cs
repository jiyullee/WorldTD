using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLobbyUI : MonoBehaviourSubUI
{
    public override void Init()
    {
        AddButtonEvent("MenuUI/StartBtn", StartGame);
        AddButtonEvent("MenuUI/CollectionBtn", 
            () => LobbyUIManager.Instance.SetUI(UIState.CollectionUI, true));
        AddButtonEvent("MenuUI/OptionBtn", () => LobbyUIManager.Instance.SetUI(UIState.OptionUI, true));
        AddButtonEvent("MenuUI/EndBtn", EndGame);
    }

    private void StartGame()
    {
        SceneManager.Instance.LoadScene("Game");
    }
    
    private void EndGame()
    {
        Debug.Log("Click_EndGame");
    }
}
