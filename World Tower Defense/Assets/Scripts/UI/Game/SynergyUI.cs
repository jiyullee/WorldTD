using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SynergyUI : MonoBehaviourSubUI
{
    public static SynergyUI Instance;

    private SYNERGY_STATE SynergyState;
    private List<SynergyViewUI> SynergyViewUis = new List<SynergyViewUI>();
    private int synergy_count;
    private Vector3[] Pos;
    private Button btn_view;
    private Button btn_info;
    private Text text_info;
    private Text text_view;

    private GameObject obj_background;
    private GameObject obj_ScrollView;
    public override void Init()
    {
        Instance = this;
        GameObject obj_synergy = transform.Find("Scroll View/Viewport/Content/Synergy").gameObject;
        obj_background = transform.Find("Image").gameObject;
        obj_ScrollView = transform.Find("Scroll View").gameObject;
        synergy_count = SynergyManager.Instance.synergy_count;

        Transform content = transform.Find("Scroll View/Viewport/Content");
        for (int i = 0; i < synergy_count; i++)
        {
            GameObject obj = Instantiate(obj_synergy);
            obj.transform.SetParent(content);
            obj.gameObject.SetActive(true);
            SynergyViewUI synergyViewUI = obj.GetComponent<SynergyViewUI>();
            synergyViewUI.Init();
            synergyViewUI.InitTexts(i);
            SynergyViewUis.Add(synergyViewUI);
        }

        btn_info = transform.Find("InfoPanel").GetComponent<Button>();
        btn_view = transform.Find("ViewButton").GetComponent<Button>();
        text_info = transform.Find("InfoPanel/Text").GetComponent<Text>();
        text_view = transform.Find("ViewButton/Text").GetComponent<Text>();
        
        AddButtonEvent(btn_info, () =>
        {
            if(btn_info.gameObject.activeSelf)
                btn_info.gameObject.SetActive(false);
        });
        AddButtonEvent(btn_view, ChangeState);
        Pos = new Vector3[2];
        Pos[0] = transform.Find("ViewButton/Pos1").transform.position;
        Pos[1] = transform.Find("ViewButton/Pos2").transform.position;
        InitState();
    }

    public override void SetView(bool state)
    {
        obj_background.SetActive(state);
        obj_ScrollView.SetActive(state);
    }
    
    public void SetSynergyUIs(int i, int idx)
    {
        SynergyViewUis[i].SetTexts(idx);
    }

    private void InitState()
    {
        SynergyState = SYNERGY_STATE.DOWN;
        SetView(false);
        btn_view.transform.position = Pos[1];
        text_view.text = SynergyState.ToString();
    }

    public void ChangeState()
    {
        SynergyState = SynergyState == SYNERGY_STATE.UP ? SYNERGY_STATE.DOWN : SYNERGY_STATE.UP;
        if (SynergyState == SYNERGY_STATE.UP)
        {
            SetView(true);
            btn_view.transform.position = Pos[0];
        }
        else
        {
            SetView(false);
            btn_view.transform.position = Pos[1];
        }
        if(btn_info.gameObject.activeSelf)
            btn_info.gameObject.SetActive(false);
        text_view.text = SynergyState.ToString();
    }

    public void SetViewInfo(bool state, int p_index)
    {
        List<int> list_activateNums = SynergyData.Instance.GetTableData(p_index).ActivateNum;
        string text = "";
        text +=  $"<color=#add8e6ff>{SynergyData.Instance.GetTableData(p_index).SynergyName_KR}</color> \n";
        text += SynergyData.Instance.GetTableData(p_index).SynergyInfo;
        text_info.text = text;
        btn_info.gameObject.SetActive(state);
    }
}
