using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerButtonUI : MonoBehaviourSubUI, IDragHandler, IBeginDragHandler, IEndDragHandler, IDropHandler
{
    private CanvasGroup canvasGroup;

    public bool isPlaceTower => tower != null;
    private Button button;
    private Image image;
    public Tower tower { get; private set; }

    private TowerButtonUI target;
    public Vector3 initPos;
    public bool CanSwap { get; private set; }
    public override void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        AddButtonEvent(button, SelectTower);
        CanSwap = false;
        SetView(false);
        button.interactable = false;
        initPos = GetComponent<RectTransform>().transform.position;
        #if UNITY_EDITOR
            GetComponent<RectTransform>().sizeDelta = new Vector2(60,60);
        #elif UNITY_ANDROID
            GetComponent<RectTransform>().sizeDelta = new Vector2(70,70);
        #endif
    }

    public override void SetView(bool state)
    {
        if (state)
            image.color = new Color(0,0,0, 0.7f); 
        else
            image.color = new Color(255,255,0, 0);
    }

    public void SetInteractable(bool state)
    {
        button.interactable = state;
    }
    public void InitTower()
    {
        tower = null;
        button.interactable = false;
        SetView(false);
    }
    
    private void SelectTower()
    {
        if (!isPlaceTower)
        {
            //타워 생성
            tower = StoreManager.Instance.CreateTower(transform.position);
            if(tower != null)
                tower.SetButtonUI(this);
            SetView(false);
            SetInteractable(true);
        }
        else
        {
            //타워 UI 띄우기
            SetViewTowerUI();
        }
    }

    public void SetViewTowerUI()
    {
        if (tower != null)
        {
            TowerUI.Instance.SetPosition(transform.position + new Vector3(0, 100,0));
            TowerUI.Instance.SetTexts(tower);
            UIManager.Instance.SetEventButton(true);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.clickCount > 2) return;
        if (!isPlaceTower) return;
        if (StageManager.IsCombatting) return;
        if (StoreManager.isSelecting) return;
        canvasGroup.blocksRaycasts = false;
        UIManager.Instance.SetEventButton(false);
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!isPlaceTower) return;
        if (StoreManager.isSelecting) return;
        if (StageManager.IsCombatting || eventData.clickCount > 2)
        {
            transform.position = initPos;
            tower.SetPositionFromScreen(initPos);
            return;
        }
        transform.position = eventData.position;
        if(isPlaceTower)
            tower.SetPositionFromScreen(transform.position);
    }
    

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isPlaceTower) return;
        if (StoreManager.isSelecting) return;
        canvasGroup.blocksRaycasts = true;
        if (StageManager.IsCombatting || !CanSwap || eventData.clickCount > 2)
        {
            transform.position = initPos;
            tower.SetPositionFromScreen(initPos);
            return;
        }
        CanSwap = false;
    }

    void Swap(TowerButtonUI p_target)
    {
        Vector2 pos1 = initPos;
        Vector2 pos2 = p_target.initPos;
        Vector2 tempPos = pos1;
        
        transform.position = pos2;
        p_target.transform.position = tempPos;

        initPos = transform.position;

        if(isPlaceTower)
            tower.SetPositionFromScreen(pos2);
        
        if(p_target.isPlaceTower)
            p_target.tower.SetPositionFromScreen(tempPos);

        CanSwap = true;
        
        SetView(false);
        SetInteractable(isPlaceTower);
    }        


    public void OnDrop(PointerEventData eventData)
    {
        if (StageManager.IsCombatting) return;
        if (eventData.pointerDrag != null)
        {
            TowerButtonUI targetButtonUI = eventData.pointerDrag.GetComponent<TowerButtonUI>();
            if (targetButtonUI == null) return;
            targetButtonUI.Swap(this);
            initPos = transform.position;
            SetView(false);
            SetInteractable(isPlaceTower);
        }
    }
}
