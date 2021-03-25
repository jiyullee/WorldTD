using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptionUI : MonoBehaviourSubUI
{
    public override void Init()
    {
        AddButtonEvent("ResumeBtn", Resume);
        AddButtonEvent("LobbyBtn", LoadLobby);
        AddButtonEvent("ExitBtn", ExitGame);
    }

    private void Resume()
    {
        SetView(false);
        UIManager.Instance.uiList[UIState.StateUI].SetView(true);
        UIManager.Instance.uiList[UIState.StoreUI].SetView(true);
    }

    private void LoadLobby()
    {
       // SceneManager.Instance.LoadScene("Lobby");
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
