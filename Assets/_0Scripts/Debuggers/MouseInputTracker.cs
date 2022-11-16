using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputTracker : MonoBehaviour
{
    #region _private_Vars
    float mouseXdebug_fl;
    float mouseYdebug_fl;
    #endregion

    #region Public_Vars
    public float RawMouse_X;
    public float RawMouse_Y;

    #endregion

    #region Enabled_Disable_Awake_Start_Destroy
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    private void Awake()
    {

    }
    void Start()
    {

    }
    private void OnDestroy()
    {

    }
    #endregion

    #region EventHAndlers


    #endregion

    #region _private_methods
    void FectchMousePos(float argmouseX, float argMouseY)
    {

        mouseXdebug_fl = argmouseX;
        if (mouseXdebug_fl > 3600) mouseXdebug_fl = 3600;
        if (mouseXdebug_fl < 0) mouseXdebug_fl = 0;
        mouseXdebug_fl = mouseXdebug_fl % 360;


        mouseYdebug_fl = argMouseY;
        if (mouseYdebug_fl > 3600) mouseYdebug_fl = 3600;
        if (mouseYdebug_fl < 0) mouseYdebug_fl = 0;
        mouseYdebug_fl = mouseYdebug_fl % 360;

       

    }
    #endregion

    #region PUBLIC_Methods

    #endregion

    #region UPDATE
    void Update()
    {
        RawMouse_X = Input.mousePosition.x;
        RawMouse_Y = Input.mousePosition.y;
        FectchMousePos(RawMouse_X, RawMouse_Y);
    }
    #endregion



}
