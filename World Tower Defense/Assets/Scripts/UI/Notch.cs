using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rect safeArea = Screen.safeArea;
        
        Vector2 newAnchorMin = safeArea.position;
        Vector2 newAnchorMax = safeArea.position + safeArea.size;
        newAnchorMin.x /= Screen.width;
        newAnchorMax.x /= Screen.width;
        newAnchorMin.y /= Screen.height;
        newAnchorMax.y /= Screen.height;
        
        
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchorMin = newAnchorMin;
        rect.anchorMax = newAnchorMax;
    }

}
