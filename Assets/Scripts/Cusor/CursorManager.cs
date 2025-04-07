using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private Vector3 mouseWorldPos => Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

    private bool canClick;




   

    private void Update()
    {
        canClick = ObjectAtmousePosisiton();

        

        if(canClick && Input.GetMouseButtonDown(0))
        {
            ClickAction(ObjectAtmousePosisiton().gameObject);
        }


    }

    

    private void ClickAction(GameObject clickObject)
    {
        switch (clickObject.tag)
        {
            case "Teleport" :
                //Debug.Log("Teleport");
                var teleport = clickObject.GetComponent<Teleport>();
                teleport.TeleportToScene();
                break;
            
        }
    }
/// <summary>
/// 检测鼠标点击范围的碰撞体
/// </summary>
/// <returns></returns>
    private Collider2D ObjectAtmousePosisiton()
    {
        return Physics2D.OverlapPoint(mouseWorldPos);
    }
   
}
