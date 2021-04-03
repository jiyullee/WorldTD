using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class Gamemanager : UnitySingleton<Gamemanager>
{
    [SerializeField]
    private const int MaxHp = 30;
    private int hp;
    /// 차후 게임 시작 창에서 setting해줄것.
    [SerializeField] private Difficulty difficulty;
    public Difficulty Difficulty
    {
        get => difficulty;
    }
    public override void OnCreated()
    {
    }


    public override void OnInitiate()
    {
        hp = MaxHp;
    }

    //일시정지시 0, 배율시 맞는 배율 삽입
    public void TimeSetting(float Magnification)
    {
        // TimeManager.TimeAcceleration(Magnification);
    }
    //게임 클리어
    public void GameClear()
    {
    }
    //게임에서 짐.
    public void GameOver()
    {
        Debug.Log("GameOver");
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
