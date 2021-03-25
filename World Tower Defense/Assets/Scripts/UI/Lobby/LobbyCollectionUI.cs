using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCollectionUI : MonoBehaviourSubUI
{
    public override void Init()
    {
        AddButtonEvent("BackBtn", () => LobbyUIManager.Instance.SetUI(UIState.Lobby));
    }
}
