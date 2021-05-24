using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLobbyUI : MonoBehaviourSubUI
{
    public override void Init()
    {
        AddButtonEvent("MenuUI/StartBtn", StartGame);
        AddButtonEvent("MenuUI/CollectionBtn", 
            () =>
            {
                SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 11);
                LobbyUIManager.Instance.SetUI(UIState.CollectionUI, true);
            });
        AddButtonEvent("MenuUI/OptionBtn", () =>
        {
            SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 11);
            LobbyUIManager.Instance.SetUI(UIState.OptionUI, true);
        });
        AddButtonEvent("MenuUI/EndBtn", EndGame);
    }

    private void StartGame()
    {
        SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 11);
        SceneManager.Instance.LoadScene("Game");
        SoundManager.Instance.PlaySound(SOUNDTYPE.BACKGROUND, 1);
    }
    
    private void EndGame()
    {
        Application.Quit();
    }
}
