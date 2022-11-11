using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlBrain : MonoBehaviour
{

    BrainPlayActionDecider ActionsDEcider;
    BrawlPointsTargetTracker PointsTrack_Vectorizer;
    HandsActionsCoordinator HandsCoordinator;
    Joy_D _DROITE;
    Joy_G _GAUCHE;
    SimpleComm _CommBrainRef;


    bool can_startWritingToArduino;
    bool CanStartInited;

    private void Awake()
    {
        ActionsDEcider = GetComponent<BrainPlayActionDecider>();
        PointsTrack_Vectorizer = GetComponent<BrawlPointsTargetTracker>();
        HandsCoordinator = GetComponent<HandsActionsCoordinator>();
        _DROITE = GetComponent<Joy_D>();
        _GAUCHE = GetComponent<Joy_G>();
        _GameState = e_BrawGameState.Loading;
    }




    public void INITme_giveemminimap(MinimapTest argMinimap, SimpleComm argComm, bool argUseCOmm)
    {
        PointsTrack_Vectorizer.INITme_giveemminimap(argMinimap);
        HandsCoordinator.Initme_givemeMyHands(_DROITE, _GAUCHE);
        ActionsDEcider.InitmePlz(HandsCoordinator, PointsTrack_Vectorizer);
        

        _CommBrainRef = argComm;
        if (argUseCOmm)
        {
            _CommBrainRef.InitializeMe(3, 115200);
        }
        else {
            Debug.LogWarning("COmm not open");
        }

        StartCoroutine(StartCOmmIn_3());
    }


    
    bool coroutinIsRuning;
    IEnumerator StartCOmmIn_3() {
        coroutinIsRuning = true;
        yield return new WaitForSeconds(3);
        Debug.Log("waited 2 seecons");
        if (!CanStartInited)
        {
            Debug.Log("state nont can start , must turnonkey");
            can_startWritingToArduino = true;
            CanStartInited = true;//and neveragain until reset
            _CommBrainRef.AllowSerialWrite(can_startWritingToArduino);
        }
        coroutinIsRuning = false;
    }
    public void ResetComm()
    {
        Debug.Log("reseting actions");
        can_startWritingToArduino = false;
        CanStartInited = false;
        _CommBrainRef.AllowSerialWrite(can_startWritingToArduino);
       if (!coroutinIsRuning) {     StartCoroutine(StartCOmmIn_3());}

    }


    e_BrawGameState _GameState;

    void Update()
    {

        if (!can_startWritingToArduino) return;
        ActionsDEcider.RunDecisionMaking_andActions();
    }
   
    
}


/*
 * 
 * 
 * 
 * 
 * 
    //public float X_G, Y_G;
    //public float X_D, Y_D;
 * 
 * 
 *     float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis, bool clockwise = false)
    {
        Vector3 right;
        if (clockwise)
        {
            right = Vector3.Cross(forward, axis);
            forward = Vector3.Cross(axis, right);
        }
        else
        {
            right = Vector3.Cross(axis, forward);
            forward = Vector3.Cross(right, axis);
        }
        return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
    }



    float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        // angle in [0,180]
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        // angle in [-179,180]
        float signed_angle = angle * sign;

        // angle in [0,360] (not used but included here for completeness)
        float angle360 = (signed_angle + 180) % 360;

        return angle360;
    }
  
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * //if (Input.GetKeyDown(KeyCode.Space))
//{
//    ActionsComander.WaitGoLeft_ThumbsUPf();
//}else
//if (Input.GetKeyDown(KeyCode.F))
//{
//    ActionsComander.TAP_wait_D();
//}
//else //it dont matter if we send move commad, the action commader wiill wait till it is recentered
//{
//    //ActionsComander.Go_ToTest_xy_G( X_G,Y_G,X_D,Y_D);
//   // ActionsComander.NAvigate_Player_ToDirection(PointsTrack_Vectorizer.Get_V3_MoveDir_NOT_normed());
//  // ActionsComander.Go_ToDirection(PointsTrack_Vectorizer.Get_V3_MoveDir_Normalized());
//    //  ActionsComander.fire(PointsTrack_Vectorizer.Get_V3_MoveDir_Normalized());
//}
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * void MoveTo_andShootTo(Vector3 argFrom, Vector3 argMoveTo, Vector3 argShootat, bool argL_on, bool arg_R_On)
    {



        //float Ang_L = Get_360_angle(argFrom, argMoveTo);

        //float Ang_R = Get_360_angle(argFrom, argShootat);

        //int tempCommand = 0;
        //int tempdebug = 0;
        //if (EmergencyBreakOn)

        //{
        //    tempCommand = 911;


        //    ArduinoNerve.Update_Message(Ang_L, Ang_R, argL_on, arg_R_On, tempCommand, PubDebug);
        //}
        //else
        //    if (isSvoTestOn)
        //{
        //    tempCommand = PubCommand;
        //    if (tempCommand != 311 && tempCommand != 611 && tempCommand != 811) tempCommand = 811;

        //    if (tempCommand == 311)
        //    {

        //        ArduinoNerve.Update_Message(PubTestAng_A, PubTestAng_B, argL_on, arg_R_On, tempCommand, PubSvoToTest);

        //    }
        //    else
        //                   if (tempCommand == 611)
        //    {

        //        ArduinoNerve.Update_Message(PubTestAng_A, PubTestAng_B, argL_on, arg_R_On, tempCommand, PubSvoToTest);

        //    }
        //    else
        //                   if (tempCommand == 811)
        //    {

        //        ArduinoNerve.Update_Message(mouseXdebug_fl, mouseYdebug_fl, argL_on, arg_R_On, tempCommand, PubSvoToTest);

        //    }


        //}
        //else
        //{
        //    ArduinoNerve.Update_Message(Ang_L, Ang_R, argL_on, arg_R_On, tempCommand, tempdebug);

        //}
    } 
 * 
 * 
 * 
 * 
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 *
 * 
 * 

 
 */
