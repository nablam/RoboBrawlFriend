using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// This is where the decisions are made
/// 
/// </summary>
public class BrainPlayActionDecider : MonoBehaviour
{

    HandsActionsCoordinator HandsCoordinator;
    BrawlPointsTargetTracker _PointsTrack_Vectorizer;

    Vector3 _rawEnemyDir_normed;
   public Vector3 _rawMoveDirDir_normed;
    float CloseRange = 1.5f;
    float MedRange = 4.5f;
    float FarRange = 6.5f;
    bool doprint=false;

    e_ButtonLocationType _thibutton_G, _thibutton_D;

    public void InitmePlz(HandsActionsCoordinator argCoordinatorOfHAnds, BrawlPointsTargetTracker argPointsTrack_Vectorizer) {
        e_ButtonLocationType _thibutton = e_ButtonLocationType._2_GadgetFire;
        HandsCoordinator = argCoordinatorOfHAnds;

        _PointsTrack_Vectorizer = argPointsTrack_Vectorizer;
       // myPointer_G = Decision_ToTWITTStop;
      //  myPointer_D = Decision_To_change_cuttlocation;
    }

    public void RunDecisionMaking_andActions() {
        _rawEnemyDir_normed = _PointsTrack_Vectorizer.Get_V3_EnemyDir_();
        _rawMoveDirDir_normed = _PointsTrack_Vectorizer.Get_V3_MoveDir_();
        HandsCoordinator.Update_2Vectors(_rawMoveDirDir_normed, _rawEnemyDir_normed);

        //DoKeyboardInput();
        //myPointer_D();

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Decision_To_change_Homes(e_ButtonLocationType._0_Main);
        }else
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("key1 amin");
            Decision_To_change_Homes(e_ButtonLocationType._1_SuperFire);

        }
        else
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("key2 gaget");
            Decision_To_change_Homes(e_ButtonLocationType._2_GadgetFire);

        }
        else
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
             print("key3 center");
            Decision_To_change_Homes(e_ButtonLocationType._3_Center);

        }
        else
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (doprint) print("key9 walk");
             
            WalkOn = !WalkOn;
        }
        if (WalkOn)
            Decision_ToTWITTWalk();
        else
            Decision_ToTWITTStop();
    }
    bool WalkOn;
   /*
    void DoKeyboardInput() {

        
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (doprint) print("key9 walk");
            //myPointer_D = Decision_ToWalk
            WalkOn = !WalkOn;
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (doprint) print("keyO killwalk");
            myPointer_G = Decision_ToTWITTStop;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (doprint) print("keyQ killfire");
            myPointer_D = Decision_To_change_cuttlocation;
        }


        if (Input.GetKeyDown(KeyCode.K))
        {
            if (doprint) print("keyK eek G");
            myPointer_G = Decision_ToQUICKHoverG;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (doprint) print("keyL eek D");
            myPointer_D = Decision_ToQUICKHoverD;
        }




        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (doprint) print("key1 tap");
            myPointer_D = Decision_ToFireTap;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (doprint) print("key2 ff");
            myPointer_D = Decision_ToFastFir;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (doprint) print("key3 fw");
            myPointer_D = Decision_ToFirWait;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (doprint) print("key4 SUPERTAP");
            myPointer_D = Decision_ToSUPERTap;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (doprint) print("key5 SUPFF");
            myPointer_D = Decision_ToFastSuper;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (doprint) print("key6 SUPW");
            myPointer_D = Decision_ToSuperWait;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (doprint) print("key7 gadTap");
            myPointer_D = Decision_ToGAdgetTap;
        }
    }
    */
 
    void Decision_ToTWITTWalk()
    {
        if (doprint) print("walk");
        HandsCoordinator._TwidleYoy_walk(_thibutton_G, true);
    }

    void Decision_ToTWITTStop()
    {
        if (doprint) print("stopwalk");
        HandsCoordinator._TwidleYoy_walk( _thibutton_G, false);
    }
    void Decision_To_change_Homes(e_ButtonLocationType argnewhome)
    {
        if (doprint) print("using " + argnewhome.ToString());
        _thibutton_G = argnewhome;
        HandsCoordinator._CHangeHomes(_thibutton_G);
    }


 
    void Decision_byRange() {
      

        if (_rawEnemyDir_normed.magnitude < CloseRange)
        {
            HandsCoordinator.FIRE_TAP();
        }
        else
        if (_rawEnemyDir_normed.magnitude > CloseRange && _rawEnemyDir_normed.magnitude < FarRange)
        {
            HandsCoordinator.FIRE_FAST_Direction();
        }
        else if (_rawEnemyDir_normed.magnitude > FarRange)
        {
            HandsCoordinator.FIRE_Openended_AT_Direction();
        }
    }



}



/// 
// WEL SHIT ... WAS USING WRONG VETOR
///

/*  void Decision_ToWalk()
    {
        if (doprint) print("walk");
        HandsCoordinator.WALK_Openended_TO_Direction(_rawMoveDirDir.normalized);
    }

    void Decision_ToStopWalk()
    {
        if (doprint) print("stopwalk");
        HandsCoordinator.Break_TO_CloseEnd(_rawMoveDirDir.normalized);
    }
    void Decision_ToReleasShoot()
    {
        if (doprint) print("stopshoot");
        HandsCoordinator.Break_AT_CloseEnd(_rawMoveDirDir.normalized);
    }


    void Decision_ToQUICKHoverD()
    {
        if (doprint) print("EEEK D");
        HandsCoordinator.QUICK_HOVER_D();
    }
    void Decision_ToQUICKHoverG()
    {
        if (doprint) print("EEEK G");
        HandsCoordinator.QUICK_HOVER_G();
    }


    void Decision_ToFireTap()
    {
        if (doprint) print("firetap");
        HandsCoordinator.FIRE_TAP(_rawMoveDirDir.normalized);
    }
    void Decision_ToFastFir()
    {
        if (doprint) print("fastfire");
        HandsCoordinator.FIRE_FAST_Direction(_rawMoveDirDir.normalized);
    }
    void Decision_ToFirWait()
    {
        if (doprint) print("firewait");
        HandsCoordinator.FIRE_Openended_AT_Direction(_rawMoveDirDir.normalized);
    }

    void Decision_ToSUPERTap()
    {
        if (doprint) print("supertap");
        HandsCoordinator.SUPER_TAP(_rawMoveDirDir.normalized);
    }
    void Decision_ToFastSuper()
    {
        if (doprint) print("superfast");
        HandsCoordinator.SUPER_FAST_Direction(_rawMoveDirDir.normalized);
    }
    void Decision_ToSuperWait()
    {
        if (doprint) print("superwait");
        HandsCoordinator.SUPER_Openended_AT_Direction(_rawMoveDirDir.normalized);
    }

    void Decision_ToGAdgetTap()
    {
        if (doprint) print("gadgettap");
        HandsCoordinator.GADGET_TAP(_rawMoveDirDir.normalized);
    }
*/



//public void NAvigate_Player_ToDirection(Vector3 argTarget) {
//    SvosBiAng biang_G = _MAIN_GAICHE.Manage_Navigation_using_SelectedPOI(argTarget);
//    EventsManagerLib.CALL_Hand_Broadcast(biang_G.SR, biang_G.SL, biang_G.SolinoidState, e_HandSide.LEFT_hand);
//    //if (!LeftIs_stillCentering)
//    //{
//    //    SvosBiAng biang_G = _MAIN_GAICHE.Manage_Navigation_using_SelectedPOI(argTarget);
//    //    EventsManagerLib.CALL_Hand_Broadcast(biang_G.SR, biang_G.SL, biang_G.SolinoidState, e_HandSide.LEFT_hand);
//    //}
//    //else {

//    //    Debug.Log("sorry, still centering bro");
//    //}

//}

//public void Go_ToTest_xy_G(float argxG, float argYG, float arg_xD, float argYD)
//{
//    SvosBiAng biang_G = _MAIN_GAICHE.Convert_XY_TO_SvoBiAngs(argxG, argYG);
//    SvosBiAng biang_D = _MAIN_DROITE.Convert_XY_TO_SvoBiAngs(arg_xD, argYD);

//    EventsManagerLib.CALL_SRSLBroadcast(biang_G.SR, biang_G.SL, biang_D.SR, biang_D.SL, biang_G.SolinoidState, biang_D.SolinoidState);
//  //  Debug.Log("Sending " + biang_G.SR + " " + biang_G.SL);
//    //  _LeftHandServos.MoveitTo(x,y,usemain);
//}

//public void WaitGo_AllThumbsUPf() { if (!RightIsStillCentering) {
//        _MAIN_DROITE.StartRoutine_REDRAG_();
//        StartCoroutine(Wait_RightCEnter()); } }
//public void WaitGoLeft_ThumbsUPf() {
//    if (!LeftIs_stillCentering) {
//        _MAIN_GAICHE.StartRoutine_REDRAG_();
//        StartCoroutine(Wait_leftCEnter());
//    }
//}

//public void TAP_wait_D()
//{
//    if (!IsStillTapping_D)
//    {
//        _MAIN_DROITE.StartRoutine_RETouch_();
//        StartCoroutine(Wait_RightTapping());
//    }
//}
//public void TAP_wait_G()
//{
//    if (!IsStillTapping_G)
//    {
//        _MAIN_GAICHE.StartRoutine_RETouch_();
//        StartCoroutine(Wait_leftTapping());
//    }
//}

////--------------------------------------------


//bool LeftIs_stillCentering, RightIsStillCentering; //may not need it ... becasue whil it is centerting, we ll send servo center string premade 
//float WaitResettime_D, WaitResettime_G = 3f;
//bool IsStillTapping_D, IsStillTapping_G;
//float Wait_TAP_time_D, Wait_TAP_time_G = 3f;
//IEnumerator Wait_leftCEnter()
//{


//    LeftIs_stillCentering = true;


//    yield return new WaitForSeconds(WaitResettime_G);
//    LeftIs_stillCentering = false;
//}

//IEnumerator Wait_RightCEnter()
//{
//    RightIsStillCentering = true;

//    yield return new WaitForSeconds(WaitResettime_D);
//    RightIsStillCentering = false;
//}


//IEnumerator Wait_leftTapping()
//{


//    IsStillTapping_G = true;


//    yield return new WaitForSeconds(Wait_TAP_time_G);
//    IsStillTapping_G = false;
//}

//IEnumerator Wait_RightTapping()
//{
//    IsStillTapping_D = true;

//    yield return new WaitForSeconds(Wait_TAP_time_D);
//    IsStillTapping_D = false;
//}
////--------------------------------------------

//public void Get_MEssage_27bytes() {

//    //generate string using dir and fire vectors 


//   //if leftstii scentering , generate tring for center 

//    // if right is still ce... same


//}
