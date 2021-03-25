using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : UnitySingleton<SceneManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
