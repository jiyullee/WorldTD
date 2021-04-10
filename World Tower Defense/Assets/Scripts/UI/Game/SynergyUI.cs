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
            string synergyName = SynergyData.Instance.GetTableData(i).SynergyName;
            GameObject obj = Instantiate(obj_synergy);
            obj.transform.SetParent(content);
            obj.gameObject.SetActive(true);
            SynergyViewUI synergyViewUI = obj.GetComponent<SynergyViewUI>();
            synergyViewUI.Init();
            SynergyViewUis.Add(synergyViewUI);
        }

        btn_view = transform.Find("ViewButton").GetComponent<Button>();
        text_view = transform.Find("ViewButton/Text").GetComponent<Text>();
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

    public void InitSynergyUIs(int i, string p_synergyName, List<int> list_activeNums, int idx)
    {
        SynergyViewUis[i].InitTexts(p_synergyName, list_activeNums, idx);
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
        text_view.text = SynergyState.ToString();
    }
}
