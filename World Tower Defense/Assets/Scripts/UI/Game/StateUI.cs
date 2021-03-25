using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateUI : MonoBehaviourSubUI
{
    private Text scoreText;
    private Text roundText;
    private Text heartText;
    private Text goldText;

    public override void Init()
    {
        AddButtonEvent("Option/Button", PauseGame);
    }

    private void PauseGame()
    {
        UIManager.Instance.uiList[UIState.StateUI].SetView(false);
        UIManager.Instance.uiList[UIState.StoreUI].SetView(false);
        UIManager.Instance.uiList[UIState.GameOptionUI].SetView(true);
    }
}
