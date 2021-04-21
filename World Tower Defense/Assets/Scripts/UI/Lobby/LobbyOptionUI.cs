using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyOptionUI : MonoBehaviourSubUI
{
    private Slider slider_sound;
    public override void Init()
    {
        AddButtonEvent("BackBtn", () => LobbyUIManager.Instance.SetUI(UIState.OptionUI, false));
        slider_sound = transform.Find("Slider").GetComponent<Slider>();
        slider_sound.onValueChanged.AddListener(volume => ControlVolume(volume));
    }

    public void ControlVolume(float volume)
    {
        
    }
}
