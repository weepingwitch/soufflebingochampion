using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public static InputManager instance;

    public bool isOsx = false;
    public bool usekeys = false;


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

        if (Input.GetJoystickNames().Length < 2)
        {
            usekeys = true;
        }
    }


    public Vector2 getPlayerMove(int pnum = 0)
    {
        if (pnum == 0)
        {
            return new Vector2(Input.GetAxisRaw("P1MoveHorizontal"), Input.GetAxisRaw("P1MoveVertical"));
        }
        else
        {
            if (usekeys)
            {
                return new Vector2(Input.GetAxisRaw("P2MoveHorizontalKeys"), Input.GetAxisRaw("P2MoveVerticalKeys"));
            }
            return new Vector2(Input.GetAxisRaw("P2MoveHorizontal"), Input.GetAxisRaw("P2MoveVertical"));
        }
    }

    public bool getPlayerShoot(int pnum = 0)
    {
        if (pnum == 0)
        {
            if (isOsx)
                return (Input.GetAxisRaw("P1FireMac") > .1f);
            else
                return(Input.GetAxisRaw("P1FireWin") > .1f);
        }
        else
        {
            if (usekeys)
                return (Input.GetButton("P2FireKeys"));
            if (isOsx)
                return (Input.GetAxisRaw("P2FireMac") > .1f);
            else
                return (Input.GetAxisRaw("P2FireWin") > .1f);
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
            if (usekeys)
            {
                return new Vector2(Input.GetAxisRaw("P2AimHorizontalKeys"), Input.GetAxisRaw("P2AimVerticalKeys"));
            }
            if (isOsx)
                return new Vector2(Input.GetAxisRaw("P2AimHorizontalMac"), -Input.GetAxisRaw("P2AimVerticalMac"));
            else
                return new Vector2(Input.GetAxisRaw("P2AimHorizontalWin"), -Input.GetAxisRaw("P2AimVerticalWin"));
        }
    }



}
