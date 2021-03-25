using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : UnitySingleton<SceneManager>
{
    public override void OnCreated()
    {
        DontDestroyOnLoad(gameObject);
    }

    public override void OnInitiate()
    {
        
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
