using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : UnitySingleton<InputManager>
{
    public override void OnCreated()
    {
        
    }

    public override void OnInitiate()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray2D ray2D = new Ray2D(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(ray2D.origin, ray2D.direction, 15);

        if (raycastHit2D)
        {
            
            
        }
        
    }

    
}
