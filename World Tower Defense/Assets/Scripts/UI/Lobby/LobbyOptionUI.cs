using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyOptionUI : MonoBehaviourSubUI
{
    public override void Init()
    {
        AddButtonEvent("BackBtn", () => LobbyUIManager.Instance.SetUI(UIState.Lobby));
    }
}
