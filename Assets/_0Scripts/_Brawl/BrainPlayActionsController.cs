using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainPlayActionsController : MonoBehaviour
{
    RoboHand _MAIN_GAICHE;
    RoboHand _MAIN_DROITE;
    public void InitmePlz() {
        _MAIN_GAICHE = new RoboHand(e_HandSide.LEFT_hand); 
        _MAIN_DROITE = new RoboHand(e_HandSide.RRIGHT_hand);
        _MAIN_GAICHE.Set_POI(e_ButtonLocationType.Center);
        _MAIN_DROITE.Set_POI(e_ButtonLocationType.Center);

        WaitResettime_G = _MAIN_GAICHE.Get_AllTimers();
        WaitResettime_D = _MAIN_DROITE.Get_AllTimers();
    }
    public void Fire_inDirection(Vector3 argTarget) { }
    public void Fire_Tap() { }
    public void FirePower_inDirection(Vector3 argTarget) { }
    public void FirePower_Tap() { }
    public void FireGadget_Tap() { }

    public void NAvigate_Player_ToDirection(Vector3 argTarget) {
        SvosBiAng biang_G = _MAIN_GAICHE.Manage_Navigation_using_SelectedPOI(argTarget);
        EventsManagerLib.CALL_Hand_Broadcast(biang_G.SR, biang_G.SL, biang_G.SolinoidState, e_HandSide.LEFT_hand);
        //if (!LeftIs_stillCentering)
        //{
        //    SvosBiAng biang_G = _MAIN_GAICHE.Manage_Navigation_using_SelectedPOI(argTarget);
        //    EventsManagerLib.CALL_Hand_Broadcast(biang_G.SR, biang_G.SL, biang_G.SolinoidState, e_HandSide.LEFT_hand);
        //}
        //else {

        //    Debug.Log("sorry, still centering bro");
        //}

    }

    public void Go_ToTest_xy_G(float argxG, float argYG, float arg_xD, float argYD)
    {
        SvosBiAng biang_G = _MAIN_GAICHE.Convert_XY_TO_SvoBiAngs(argxG, argYG);
        SvosBiAng biang_D = _MAIN_DROITE.Convert_XY_TO_SvoBiAngs(arg_xD, argYD);

        EventsManagerLib.CALL_SRSLBroadcast(biang_G.SR, biang_G.SL, biang_D.SR, biang_D.SL, biang_G.SolinoidState, biang_D.SolinoidState);
      //  Debug.Log("Sending " + biang_G.SR + " " + biang_G.SL);
        //  _LeftHandServos.MoveitTo(x,y,usemain);
    }

    public void WaitGo_AllThumbsUPf() { if (!RightIsStillCentering) {
            _MAIN_DROITE.StartRoutineTurnKey_();
            StartCoroutine(Wait_RightCEnter()); } }
    public void WaitGoLeft_ThumbsUPf() {
        if (!LeftIs_stillCentering) {
            _MAIN_GAICHE.StartRoutineTurnKey_();
            StartCoroutine(Wait_leftCEnter());
        }
    }

    //--------------------------------------------


    bool LeftIs_stillCentering, RightIsStillCentering; //may not need it ... becasue whil it is centerting, we ll send servo center string premade 
    float WaitResettime_D, WaitResettime_G = 3f;
    IEnumerator Wait_leftCEnter()
    {


        LeftIs_stillCentering = true;
        
        
        yield return new WaitForSeconds(WaitResettime_G);
        LeftIs_stillCentering = false;
    }

    IEnumerator Wait_RightCEnter()
    {
        RightIsStillCentering = true;
         
        yield return new WaitForSeconds(WaitResettime_D);
        RightIsStillCentering = false;
    }

    //--------------------------------------------

    public void Get_MEssage_27bytes() {
    
        //generate string using dir and fire vectors 


       //if leftstii scentering , generate tring for center 

        // if right is still ce... same


    }
    


}
