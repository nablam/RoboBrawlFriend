using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsActionsCoordinator : MonoBehaviour
{

    Joy_D _myhandDROITE;
    Joy_G _myhandGAUCHE;
    e_ButtonLocationType _thibutton_G, _thibutton_D;

    public void Initme_givemeMyHands(Joy_D arg_DROITE, Joy_G arg_GAUCHE )
    {
        _myhandDROITE = arg_DROITE;
        _myhandGAUCHE = arg_GAUCHE;

        _thibutton_G = e_ButtonLocationType._0_Main;
        _thibutton_D = e_ButtonLocationType._0_Main;


    }
    public void Update_2Vectors(Vector3 arg_moveVEc, Vector3 argFireVec) {
           _myhandDROITE.Update_FIREatV3(    argFireVec);
            _myhandGAUCHE.Update_MoveToV3(arg_moveVEc );
    }

    //---------------------WALKING _ LEFT HAND GAUCHE---------------------------------------
    //---------------MAIN
    public void _TwidleYoy_walk( bool StartITOn) {
        _myhandGAUCHE.Update_TwiddleStick_atThisHome(_thibutton_G, StartITOn);
    }





    public void FIRE_Openended_AT_Direction(bool arg_start)
    {

        //if same home  just set routine on
        //if diff home start coroutine change home and thrn set routine one
         
            
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
            //shortransition Tap (mode TAPACTION)
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
            //shortransition Tap (mode TAPACTION)
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
            //shortransition Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Dashing, _thibutton_D);

        }

    }


    //---------------SEC
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
            //shortransition Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Dashing, _thibutton_D);

        }
    }



    public void SUPER_Aim_AT()
    {
        if (_thibutton_D != e_ButtonLocationType._1_SuperFire)
        {

            _thibutton_D = e_ButtonLocationType._1_SuperFire;
            //L O N G T RANSIT n Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Transiting, AppSettings.Instance.TimeThreshold_Dashing+0.5f, _thibutton_D);
        }
        else
        {
            //shortransition Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Dashing+0.5f, _thibutton_D);

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
            //shortransition Tap (mode TAPACTION)
            _myhandDROITE.DoTransitAction(AppSettings.Instance.TimeThreshold_Tapping, AppSettings.Instance.TimeThreshold_Dashing + 10.5f, _thibutton_D);

        }
    }
    public void Break_AT_CloseEnd()
    {

         _myhandDROITE.DoForceFireUnload();
    }



    public void _CHangeHomes(e_ButtonLocationType argNewHome) {
       // _myhandGAUCHE.Update_HomeLocation(argNewHome);
       // _myhandDROITE.Update_HomeLocation(argNewHome);

    }













    //---------------------ATTACKING _ RIGHT HAND DROITE------------------------------------
    //---------------MAIN
    bool tapStarted;
    //IEnumerator DoTapUnTap() {
    //    tapStarted = true;
    //    _myhandDROITE.Update_TwiddleStick_atThisHome(e_ButtonLocationType._0_Main, true, true);
    //    yield return new WaitForSeconds(0.5f);
    //    tapStarted = false;
    //    _myhandDROITE.Update_TwiddleStick_atThisHome(e_ButtonLocationType._0_Main, false);
    //}
    //IEnumerator DoTapUnTapSuper()
    //{
    //    tapStarted = true;
    //    _myhandDROITE.Update_TwiddleStick_atThisHome(e_ButtonLocationType._1_SuperFire, true, true);
    //    yield return new WaitForSeconds(0.5f);
    //    tapStarted = false;
    //    _myhandDROITE.Update_TwiddleStick_atThisHome(e_ButtonLocationType._1_SuperFire, false);
    //}
    //IEnumerator DoTapUnTapGAdget()
    //{
    //    tapStarted = true;
    //    _myhandDROITE.Update_TwiddleStick_atThisHome(e_ButtonLocationType._2_GadgetFire, true, true);
    //    yield return new WaitForSeconds(0.5f);
    //    tapStarted = false;
    //    _myhandDROITE.Update_TwiddleStick_atThisHome(e_ButtonLocationType._2_GadgetFire, false);
    //}
 
  
 
    //---------------TER


  
    // CoroutineCoordinator_Lefthand
    //pass : ACTION HOME_Main  argNormalized
    public void QUICK_HOVER_D() {

       // _myhandDROITE.QUICKHOVER();
    }
    public void QUICK_HOVER_G()
    {

        //_myhandGAUCHE.QUICKHOVER();
    }





}


/* //---------------------WALKING _ LEFT HAND GAUCHE---------------------------------------
    //---------------MAIN
    public void WALK_Openended_TO_Direction(Vector3 arg_v3_Normed) {
        _myhandGAUCHE.DOACTIONWalkThumb(e_ButtonLocationType.Main, arg_v3_Normed);
    }


    //---------------------ATTACKING _ RIGHT HAND DROITE------------------------------------
    //---------------MAIN
    public void FIRE_TAP(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FireTap(e_ButtonLocationType.Main, argTarget);
    }
    public void FIRE_FAST_Direction(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FastFire(e_ButtonLocationType.Main, argTarget);
    }
    public void FIRE_Openended_AT_Direction(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FireWait(e_ButtonLocationType.Main, argTarget);
    }

    //---------------SEC
    public void SUPER_TAP(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FireTap(e_ButtonLocationType.SuperFire, argTarget);
    }
    public void SUPER_FAST_Direction(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FastFire(e_ButtonLocationType.SuperFire, argTarget);
    }
    public void SUPER_Openended_AT_Direction(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FireWait(e_ButtonLocationType.SuperFire, argTarget);
    }
    //---------------TER
    public void GADGET_TAP(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FireTap(e_ButtonLocationType.GadgetFire, argTarget);
    }


    public void Break_TO_CloseEnd(Vector3 argTarget) {
        _myhandGAUCHE.DOACTION_CLOSEWALK(argTarget);
    }
    public void Break_AT_CloseEnd(Vector3 argTarget) {
        _myhandDROITE.DOACTION_CLOSEFirewait(argTarget);
    }*/
