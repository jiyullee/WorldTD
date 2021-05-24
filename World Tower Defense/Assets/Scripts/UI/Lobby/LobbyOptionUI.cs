using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyOptionUI : MonoBehaviourSubUI
{
    private Slider slider_backSound;
    private Slider slider_effectSound;
    public override void Init()
    {
        AddButtonEvent("BackBtn", () =>
        {
            SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 11);
            LobbyUIManager.Instance.SetUI(UIState.OptionUI, false);
        });
        slider_backSound = transform.Find("BackgroundSlider").GetComponent<Slider>();
        slider_effectSound = transform.Find("EffectSlider").GetComponent<Slider>();
        slider_backSound.onValueChanged.AddListener(volume => SoundManager.Instance.ControlVolume(SOUNDTYPE.BACKGROUND, volume));
        slider_effectSound.onValueChanged.AddListener(volume => SoundManager.Instance.ControlVolume(SOUNDTYPE.EFFECT, volume));
        SetView(false);
    }
}
