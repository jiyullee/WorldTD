using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    Dictionary<int, Tower> all_towers_dic = new Dictionary<int, Tower>();
    Dictionary<int, Tower> remain_towers_dic = new Dictionary<int, Tower>();
    
    void Awake()
    {
        
    }
}
