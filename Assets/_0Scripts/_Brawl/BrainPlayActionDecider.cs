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

    Vector3 _rawEnemyDir;
    Vector3 _rawMoveDirDir;
    float CloseRange = 1.5f;
    float MedRange = 4.5f;
    float FarRange = 6.5f;


    public void InitmePlz(HandsActionsCoordinator argCoordinatorOfHAnds, BrawlPointsTargetTracker argPointsTrack_Vectorizer) {
        HandsCoordinator = argCoordinatorOfHAnds;
        
       
    }

    public void RunDecisionMaking_andActions() {
        _rawEnemyDir = _PointsTrack_Vectorizer.Get_V3_EnemyDir_();
        _rawMoveDirDir = _PointsTrack_Vectorizer.Get_V3_MoveDir_();

        Decision_ToWalk();

    }
 


    void Decision_ToWalk()
    {
        HandsCoordinator.WALK_Openended_TO_Direction(_rawMoveDirDir.normalized);
    }
    void Decision_byRange() {
      

        if (_rawEnemyDir.magnitude < CloseRange)
        {
            HandsCoordinator.FIRE_TAP(_rawEnemyDir);
        }
        else
        if (_rawEnemyDir.magnitude > CloseRange && _rawEnemyDir.magnitude < FarRange)
        {
            HandsCoordinator.FIRE_FAST_Direction(_rawEnemyDir);
        }
        else if (_rawEnemyDir.magnitude > FarRange)
        {
            HandsCoordinator.FIRE_Openended_AT_Direction(_rawEnemyDir);
        }
    }



}


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
