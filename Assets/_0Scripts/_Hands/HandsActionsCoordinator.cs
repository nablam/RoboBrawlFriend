using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsActionsCoordinator : MonoBehaviour
{
    /// <summary>
    /// All public methods and driven update
    /// </summary>
    #region _private_Vars
    Joy_D _myhandDROITE;
    Joy_G _myhandGAUCHE;
    e_ButtonLocationType _thibutton_G, _thibutton_D;
    #endregion

    #region Public_Vars

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

    #endregion

    #region PUBLIC_Methods
    public void Initme_givemeMyHands(Joy_D arg_DROITE, Joy_G arg_GAUCHE)
    {
        _myhandDROITE = arg_DROITE;
        _myhandGAUCHE = arg_GAUCHE;

        _thibutton_G = e_ButtonLocationType._0_Main;
        _thibutton_D = e_ButtonLocationType._0_Main;
    }
    public void Update_2Vectors(Vector3 arg_moveVEc, Vector3 argFireVec)
    {
        _myhandDROITE.Update_FIREatV3(argFireVec);
        _myhandGAUCHE.Update_MoveToV3(arg_moveVEc);
    }
    public void _TwidleYoy_walk(bool StartITOn)
    {
        _myhandGAUCHE.Update_UseJoystick_OnLocation(_thibutton_G, StartITOn);
    }
    public void FIRE_TAP()
    {
        if (_thibutton_D != e_ButtonLocationType._0_Main)
        {
            _thibutton_D = e_ButtonLocationType._0_Main;
            //L O N G T RANSIT n Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Transiting, AppSettings.Instance.TimeThreshold_Tapping, _thibutton_D);
        }
        else
        {
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Tapping, _thibutton_D);
        }

    }
    public void SUPER_TAP()
    {
        if (_thibutton_D != e_ButtonLocationType._1_SuperFire)
        {
            _thibutton_D = e_ButtonLocationType._1_SuperFire;
            //L O N G T RANSIT n Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Transiting, AppSettings.Instance.TimeThreshold_Tapping, _thibutton_D);
        }
        else
        {
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Tapping, _thibutton_D);
        }
    }
    public void GADGET_TAP()
    {
        if (_thibutton_D != e_ButtonLocationType._2_GadgetFire)
        {
            _thibutton_D = e_ButtonLocationType._2_GadgetFire;
            //L O N G T RANSIT n Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Transiting, AppSettings.Instance.TimeThreshold_Tapping, _thibutton_D);
        }
        else
        {
            //shortransition Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Tapping, _thibutton_D);
        }
    }
    public void FIRE_FAST_Direction()
    {
        if (_thibutton_D != e_ButtonLocationType._0_Main)
        {
            _thibutton_D = e_ButtonLocationType._0_Main;
            //L O N G T RANSIT n Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Transiting, AppSettings.Instance.TimeThreshold_Dashing, _thibutton_D);
        }
        else
        {
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Dashing, _thibutton_D);

        }
    }
    public void SUPER_FAST_Direction()
    {
        if (_thibutton_D != e_ButtonLocationType._1_SuperFire)
        {
            _thibutton_D = e_ButtonLocationType._1_SuperFire;
            //L O N G T RANSIT n Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Transiting, AppSettings.Instance.TimeThreshold_Dashing, _thibutton_D);
        }
        else
        {
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Dashing, _thibutton_D);
        }
    }
    public void SUPER_Aim_AT()
    {
        if (_thibutton_D != e_ButtonLocationType._1_SuperFire)
        {
            _thibutton_D = e_ButtonLocationType._1_SuperFire;
            //L O N G T RANSIT n Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Transiting, AppSettings.Instance.TimeThreshold_Dashing + 0.5f, _thibutton_D);
        }
        else
        {
            //shortransition Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Dashing + 0.5f, _thibutton_D);
        }
    }
    public void Fire_Aim_AT()
    {
        if (_thibutton_D != e_ButtonLocationType._0_Main)
        {
            _thibutton_D = e_ButtonLocationType._0_Main;
            //L O N G T RANSIT n Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Transiting, AppSettings.Instance.TimeThreshold_Dashing + 10.5f, _thibutton_D);
        }
        else
        {
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Dashing + 10.5f, _thibutton_D);
        }
    }
    public void Break_AT_CloseEnd()
    {
        _myhandDROITE.DoForceFireUnload();
    }
    #endregion

    #region UPDATE_____________________Driven
    //void Update()
    //{

    //}
    #endregion
}