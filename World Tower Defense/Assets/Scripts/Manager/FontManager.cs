using System.Collections;
using System.Collections.Generic;
using GameData;
using UnityEngine;

public class FontManager : UnitySingleton<FontManager>
{
    public List<Font> list_font = new List<Font>();
    public override void OnCreated()
    {
        DontDestroyOnLoad(gameObject);
        int cnt = FontData.Instance.GetTable().Count;
        for (int i = 0; i < cnt; i++)
        {
            string path = FontData.Instance.GetTableData(i).Path;
            Font font = Resources.Load<Font>(path);
            list_font.Add(font);
        }
    }

    public override void OnInitiate()
    {
        
    }

    public Font GetFont(int key)
    {
        if (list_font[key] == null)
            return list_font[0];
        
        return list_font[key];
    }
}