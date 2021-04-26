using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Gamemanager : UnitySingleton<Gamemanager>
{
    [SerializeField]
    private const int MaxHp = 30;
    private int hp;
    public override void OnCreated()
    {
    }


    public override void OnInitiate()
    {
        hp = MaxHp;
    }

    public void Damage(int damage)
    {
        hp -= damage;
        StateUI.Instance.SetHPText(hp);
        if (hp <= 0)
        {
            GameOver();
        }
    }

    //게임 클리어
    public void GameClear()
    {
        PopUpUI.Instance.PopUp(POPUP_STATE.GameWin);
        TimeManager.Instance.Pause();
        
        //승리 UI
    }

    //게임에서 짐.
    public void GameOver()
    {
        PopUpUI.Instance.PopUp(POPUP_STATE.GameLose);
        TimeManager.Instance.Pause();
        
        //게임 오버 UI표기.
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit() // 어플리케이션 종료
#endif
    }
}
