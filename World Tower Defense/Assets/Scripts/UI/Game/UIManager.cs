using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSubUI
{
    public static UIManager Instance;

    private Button btn_event;
    public Dictionary<UIState, MonoBehaviourSubUI> uiList = new Dictionary<UIState, MonoBehaviourSubUI>();

    //현재 보여주는 UI
    protected MonoBehaviourSubUI selectUI;

    private GraphicRaycaster graphicRaycaster;
    private PointerEventData ped;
    public static Canvas canvas { get; private set; }

    private List<Text> list_texts = new List<Text>();
    
    void Awake()
    {
        Instance = this;
        Init();
    }

    private void Start()
    {
        list_texts = FindObjectsOfType<Text>().ToList();
        for (int i = 0; i < list_texts.Count; i++)
        {
            list_texts[i].font = FontManager.Instance.GetFont(1);
        }
    }

    public override void Init()
    {
        Instance = this;
        canvas = GetComponent<Canvas>();
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        ped = new PointerEventData(null);
        btn_event = transform.Find("EventButton").GetComponent<Button>();
        SetEventButton(false);
        
        AddUI(UIState.StateUI,"StateUI", "StateUI");
        AddUI(UIState.StoreUI, "StoreUI", "StoreUI");
        AddUI(UIState.MapUI, "MapUI", "MapUI");
        AddUI(UIState.SynergyUI, "SynergyUI", "SynergyUI");
        AddUI(UIState.TowerUI, "TowerUI", "TowerUI");
        AddUI(UIState.PopUpUI, "PopUpUI", "PopUpUI");
        foreach (var data in uiList)
        {
            if(data.Value)
                data.Value.Init();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(ped, results);
            if (results.Count > 0){
                //타워 UI Off
                if(results[0].gameObject.layer != 8)
                    SetTowerView();
                
                //타워 클릭 시
                if (results.Count >= 2)
                {
                    if(results[1].gameObject.layer == 9)
                        results[1].gameObject.GetComponent<TowerButtonUI>().SetViewTowerUI(); 
                }
                   
            }
            
        }

    }

    /// <summary>
    /// 해당 UI를 등록하는 메소드
    /// </summary>
    /// <param name="state"></param>
    /// <param name="path"></param>
    /// <param name="script"></param>
    protected void AddUI(UIState state, string path, string script = "")
    {
        if (!string.IsNullOrEmpty(script))
        {
            uiList.Add(state, transform.Find(path).gameObject.AddComponent(System.Type.GetType(script)) as MonoBehaviourSubUI);
        }
        else
        {
            uiList.Add(state, transform.Find(path).GetComponent<MonoBehaviourSubUI>());
        }
            
    }

    /// <summary>
    /// 해당 UI를 보여주는 메소드
    /// </summary>
    /// <param name="state"></param>
    public void SetUI(UIState state)
    {
        if (uiList.ContainsKey(state))
        {
            if (selectUI) selectUI.SetView(false);

            selectUI = uiList[state];

            if (selectUI) selectUI.SetView(true);
        }
        else
            selectUI = null;
    }

    public void SetTowerView()
    {
        bool state = TowerUI.Instance.gameObject.activeSelf;
        if (state)
        {
            TowerUI.Instance.SetView(false);
            SetEventButton(false);
        }
    }

    /// <summary>
    /// 화면 터치 버튼 활성화 여부 메소드
    /// </summary>
    /// <param name="state"></param>
    public void SetEventButton(bool state)
    {
        btn_event.interactable = state;
        btn_event.targetGraphic.raycastTarget = state;
    }
}
