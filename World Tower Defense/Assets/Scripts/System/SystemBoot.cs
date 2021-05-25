using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemBoot : MonoBehaviour
{
    private ExitPopUpUI ExitPopUpUI;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        ExitPopUpUI = transform.Find("Canvas/ExitUI").GetComponent<ExitPopUpUI>();
        ExitPopUpUI.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPopUpUI.SetView(true);
        }
    }
    
  
}
