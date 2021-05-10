using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collection : MonoBehaviourSubUI
{
    private TowerInstance towerInstance;
    private Button button;

    public override void Init()
    {
        button = GetComponent<Button>();
        AddButtonEvent(button, ShowTowerInfo);

    }

    public void InitTowerInfo(TowerInstance p_towerInstance)
    {
        towerInstance = p_towerInstance;
      gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Images/Flags/{towerInstance.GetTowerData().TowerName}");
    }

    private void ShowTowerInfo()
    {
        LobbyCollectionUI.Instance.SetInfoPanel(towerInstance);
    }
}
