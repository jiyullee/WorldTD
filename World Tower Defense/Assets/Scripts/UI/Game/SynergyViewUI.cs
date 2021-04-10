using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergyViewUI : MonoBehaviourSubUI
{
    private Text text_synergyName;
    private Text text_count;
    public override void Init()
    {
        text_synergyName = transform.Find("SynergyName").GetComponent<Text>();
        text_count = transform.Find("Text").GetComponent<Text>();
    }

    public void InitTexts(string p_synergyName, List<int> list_activateNums, int idx)
    {
        text_synergyName.text = p_synergyName;

        if (p_synergyName == "섬")
        {
            text_count.text = "항상 적용";
            return;
        }
        
        string text = "";
        for (int i = 1; i < list_activateNums.Count; i++)
        {
            if (i != list_activateNums.Count - 1)
            {
                if(idx == i)
                    text += $"<color=#000000ff>{list_activateNums[i]}</color> / ";
                else
                    text += $"<color=#808080ff>{list_activateNums[i]}</color> / ";
                
            }
            else
            {
                if(idx == i)
                    text += $"<color=#000000ff>{list_activateNums[i]}</color>";
                else
                    text += $"<color=#808080ff>{list_activateNums[i]}</color>";
            }
        }

        text_count.text = text;
    }
}
