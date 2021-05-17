using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
    [SerializeField]
    private const int maxHp = 30;
    private int hp;
    
    public override void OnCreated()
    {
        
    }


    public override void OnInitiate()
    {
        hp = maxHp;
    }

    public void Damage(int damage)
    {
        hp = hp - damage >= 0 ? hp - damage : 0;
        StateUI.Instance.SetHPText(hp);
        if (hp <= 0)
        {
            GameOver();
        }
    }


    //게임 클리어
    public void GameClear()
    {
        TimeManager.Instance.Pause();
        PopUpUI.Instance.PopUp(POPUP_STATE.GameWin);
    }

    //게임에서 짐.
    public void GameOver()
    {
        TimeManager.Instance.Pause();
        PopUpUI.Instance.PopUp(POPUP_STATE.GameLose);

        //게임 오버 UI표기.
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit(); // 어플리케이션 종료
#endif
    }
}
