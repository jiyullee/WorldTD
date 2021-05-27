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
        SetResolution();
      
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

    void SetResolution()
    {
        int setWidth = 720;
        int setHeight = 1280;

        int width = Screen.width;
        int height = Screen.height;
        
        Screen.SetResolution(width, width * setHeight / setWidth,true);

        if ((float) setWidth / setHeight < (float) width / height)
        {
            float newWidth = ((float) setWidth / setHeight) / ((float) width / height);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else
        {
            float newHeight = ((float) width / height) / ((float) setWidth / setHeight);
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
        }
    }
  
}
