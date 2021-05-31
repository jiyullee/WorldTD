using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergyViewUI : MonoBehaviourSubUI
{
    private Text text_synergyName;
    private Text text_count;

    private int Index;
    List<int> list_activateNums = new List<int>();
    public override void Init()
    {
        text_synergyName = transform.Find("Button/SynergyName").GetComponent<Text>();
        text_count = transform.Find("Button/Text").GetComponent<Text>();
        
        AddButtonEvent("Button",ShowInfo);
    }

    public void InitTexts(int index)
    {
        Index = index;
        string synergyName_KR = SynergyData.Instance.GetTableData(index).SynergyName_KR;
        list_activateNums = SynergyData.Instance.GetTableData(index).ActivateNum;
        
        text_synergyName.text = synergyName_KR;
        
        if (synergyName_KR == "섬")
        {
            text_count.text = "항상 적용";
            return;
        }
        
        string text = "";
        for (int i = 1; i < list_activateNums.Count; i++)
        {
            if (i != list_activateNums.Count - 1)
            {
                text += $"<color=#808080ff>{list_activateNums[i]}</color> / ";
                
            }
            else
            {
                text += $"<color=#808080ff>{list_activateNums[i]}</color>";
            }
        }

        text_count.text = text;
    }
    
    public void SetTexts(int idx)
    {
        if (text_synergyName.text == "섬")
        {
            text_count.text = "항상 적용";
            return;
        }
        string text = "";
        if (idx == 10) idx = list_activateNums.Count - 1;
        for (int i = 1; i < list_activateNums.Count; i++)
        {
            if (i != list_activateNums.Count - 1)
            {
                if(idx == i)
                    text += $"<color=#ffffffff>{list_activateNums[i]}</color> / ";
                else
                    text += $"<color=#808080ff>{list_activateNums[i]}</color> / ";
                
            }
            else
            {
                if(idx == i)
                    text += $"<color=#ffffffff>{list_activateNums[i]}</color>";
                else
                    text += $"<color=#808080ff>{list_activateNums[i]}</color>";
            }
        }

        text_count.text = text;
    }

    public void ShowInfo()
    {
        SoundManager.Instance.PlaySound(SOUNDTYPE.EFFECT, 10, 0.7f);
        SynergyUI.Instance.SetViewInfo(true, Index);
    }
}
