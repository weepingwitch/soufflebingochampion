using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    public bool isOsx = false;


    //set the static instance, make sure there is only one
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }


        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
            isOsx = true;
        else
            isOsx = false;
    }


    public Vector2 getPlayerMove(int pnum = 0)
    {
        if (pnum == 0)
        {
            return new Vector2(Input.GetAxisRaw("P1MoveHorizontal"), Input.GetAxisRaw("P1MoveVertical"));
        }
        else
        {
            return new Vector2(Input.GetAxisRaw("P2MoveHorizontal"), Input.GetAxisRaw("P2MoveVertical"));
        }
    }


    public Vector2 getPlayerAim(int pnum = 0)
    {

        if (pnum == 0)
        {
            if (isOsx)
                return new Vector2(Input.GetAxisRaw("P1AimHorizontalMac"), -Input.GetAxisRaw("P1AimVerticalMac"));
            else
                return new Vector2(Input.GetAxisRaw("P1AimHorizontalWin"), -Input.GetAxisRaw("P1AimVerticalWin"));
        }
        else
        {
            if (isOsx)
                return new Vector2(Input.GetAxisRaw("P2AimHorizontalMac"), -Input.GetAxisRaw("P2AimVerticalMac"));
            else
                return new Vector2(Input.GetAxisRaw("P2AimHorizontalWin"), -Input.GetAxisRaw("P2AimVerticalWin"));
        }
    }



}
