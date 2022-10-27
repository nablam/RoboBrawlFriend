using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlBrain : MonoBehaviour
{

    public float mouseXdebug_fl;
    public float mouseYdebug_fl;
    public float mouseXdebug_int;
    public int PubDebug = 989;

    [SerializeField]
    public Vector3 PubFrom;
    [SerializeField]

    public Vector3 PubTo;
    [SerializeField]

    public Vector3 NormalizedFromCenter;

    public Transform CapsuleFrom;
    public Transform CapsuleAim;
    public Transform GreenTran;
    public Transform RedTran;
      Transform _curtargetMoveDirection;
    Transform _curtargetShootAt;
    Transform _curFrom;
    Vector3[] temptargets;
    Vector3[] tempCardinal;
    Vector3 tempPlayer;
    float targetTime = 0.5f;
    public MedianNerve ArduinoNerve;

    public int PubCardinalIndex_move_07;
    public int PubCardinalIndex_shoot_07;

    bool can_startWritingToArduino;
    bool CanStartInited;
    bool EmergencyBreakOn;
    public bool PressLeftOn;
    public bool PressRightOn;
    public bool isSvoTestOn;
    public int PubCommand;

    public int PubTestAng_A;
    public int PubTestAng_B;
    public int PubSvoToTest;
    void OnDrawGizmos()
    {

        PubFrom = GreenTran.position;
        PubTo = RedTran.position;
         

        NormalizedFromCenter = (PubTo - PubFrom).normalized;
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, NormalizedFromCenter);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, NormalizedFromCenter*100);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GreenTran.position, 10);


        Gizmos.color = Color.red;
        Gizmos.DrawSphere(RedTran.position, 10);

       // var angle = Mathf.Atan2(NormalizedFromCenter.x, NormalizedFromCenter.y) * Mathf.Rad2Deg;
       // float angle360 = (angle + 180) % 360;

       // float theDotFL = Vector3.Dot(PubFrom, PubTo);
       // theDotFL = theDotFL / (PubFrom.magnitude * PubTo.magnitude);
       // float varacos = Mathf.Acos(theDotFL);
       // float VarAng = varacos * 180 / Mathf.PI;

       // float fAngle = Vector3.Cross(PubFrom.normalized, PubTo.normalized).y;


       // // Convert to -180 to +180 degrees
       // fAngle *= 180.0f;


       // var v1 = PubFrom;// - this.transform.position;
       // var v2 = PubTo;// - this.transform.position;
       // var axis = Vector3.Cross(v1, v2).normalized;
       //// var axis = Vector3.up;
       // var a = AngleOffAroundAxis(v1, v2, axis, false);


       // Gizmos.color = Color.black;
       // Gizmos.DrawLine(this.transform.position, PubFrom);
       // Gizmos.color = Color.blue;
       // Gizmos.DrawLine(this.transform.position, PubTo);
       // Gizmos.color = Color.red;
       // Gizmos.DrawLine(this.transform.position, this.transform.position + axis * 20f);

       //float SingenAng360= SignedAngleBetween( v2, v1, Vector3.forward);
       // Debug.Log(angle360);
       // //Debug.Log(a);



        // Debug.Log(VarAng);
    }

      float Get_360_angle() {

        NormalizedFromCenter = (PubTo - PubFrom).normalized;
        var angle = Mathf.Atan2(NormalizedFromCenter.x, NormalizedFromCenter.y) * Mathf.Rad2Deg;
        float angle360 = (angle + 180) % 360;
        Debug.Log(angle360);
        return angle360;
   
    }

    float Get_360_angle(Vector3 argFrom, Vector3 argto)
    {
       Vector3 Temp_NormalizedFromCenter = (argFrom -argto  ).normalized;
        var angle = Mathf.Atan2(Temp_NormalizedFromCenter.x, Temp_NormalizedFromCenter.y) * Mathf.Rad2Deg;
        float angle360 = (angle + 180) % 360;
       // Debug.Log(angle360);
        return angle360;
    }

    float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis, bool clockwise = false)
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
        float angle360 =  (signed_angle + 180) % 360;

        return angle360;
    }
    MinimapTest _miniTargets;
    public void INITme_giveemminimap(MinimapTest argMinimap) {
        _miniTargets = argMinimap;
        temptargets = _miniTargets.Get_Final_Enemilocations();
        tempPlayer = _miniTargets.Get_Playerlocations();
        tempCardinal = _miniTargets.Get_Cardinallocations();
    }

    
 void MoveTo_andShootTo(Vector3 argFrom, Vector3 argMoveTo, Vector3 argShootat, bool argL_on, bool arg_R_On) {

         

        float Ang_L = Get_360_angle(argFrom, argMoveTo);

        float Ang_R = Get_360_angle(argFrom, argShootat);

        int tempCommand = 0;
        int tempdebug = 0;
        if (EmergencyBreakOn)

        {
            tempCommand = 911;


            ArduinoNerve.Update_Message(Ang_L, Ang_R, argL_on, arg_R_On, tempCommand, PubDebug);
        }
        else
            if (isSvoTestOn)
        {
            tempCommand = PubCommand;
            if (tempCommand != 311 && tempCommand != 611 && tempCommand != 811) tempCommand = 811;

            if (tempCommand == 311) { 
            
                 ArduinoNerve.Update_Message(PubTestAng_A, PubTestAng_B, argL_on, arg_R_On, tempCommand, PubSvoToTest);
         
            }else
                           if (tempCommand == 611)
            {

                ArduinoNerve.Update_Message(PubTestAng_A, PubTestAng_B, argL_on, arg_R_On, tempCommand, PubSvoToTest);

            }
            else
                           if (tempCommand == 811)
            {

                ArduinoNerve.Update_Message(mouseXdebug_fl, mouseYdebug_fl, argL_on, arg_R_On, tempCommand, PubSvoToTest);

            }
             

        }
        else
        {
            ArduinoNerve.Update_Message(Ang_L, Ang_R, argL_on, arg_R_On, tempCommand, tempdebug);

        }


    }

    void Start()
    {
        ArduinoNerve.InitializeMe(3, 115200);
    }
    public void ResetChainOfActions() {
        Debug.Log("reseting actions");
        can_startWritingToArduino = false;
        CanStartInited = false;

    }
 
    void Update()
    {

        mouseXdebug_fl = Input.mousePosition.x;
        if (mouseXdebug_fl > 3600) mouseXdebug_fl = 3600;
        if (mouseXdebug_fl < 0) mouseXdebug_fl = 0;
        mouseXdebug_fl = mouseXdebug_fl % 360;


        mouseYdebug_fl = Input.mousePosition.y;
        if (mouseYdebug_fl > 3600) mouseYdebug_fl = 3600;
        if (mouseYdebug_fl < 0) mouseYdebug_fl = 0;
        mouseYdebug_fl = mouseYdebug_fl % 360;

        PressLeftOn = false;
        PressRightOn = false;


        if (Input.GetKeyDown(KeyCode.S)) {
            if (!CanStartInited)
            {
                can_startWritingToArduino = true;
                CanStartInited = true;//and neveragain until reset
                ArduinoNerve.AlloStimulation(can_startWritingToArduino);
            }
        }


        if (Input.GetKey(KeyCode.Q))
        {
            PressLeftOn = true;

        }

        if (Input.GetKey(KeyCode.W))
        {
            PressRightOn = true;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EmergencyBreakOn = true;
        }

        if (EmergencyBreakOn)
        {

            targetTime -= Time.deltaTime;

            if (targetTime <= 0.0f)
            {
                // timerEnded();
                EmergencyBreakOn = false;
                targetTime = 2.5f;
            }
        }



        if (!can_startWritingToArduino) return;



        if (PubCardinalIndex_move_07 > 7) PubCardinalIndex_move_07 = 7;
        if (PubCardinalIndex_move_07 < 1) PubCardinalIndex_move_07 = 0;

        if (PubCardinalIndex_shoot_07 > 7) PubCardinalIndex_shoot_07 = 7;
        if (PubCardinalIndex_shoot_07 < 1) PubCardinalIndex_shoot_07 = 0;

        if (PubTestAng_A > 360) PubTestAng_A = 360;
        if (PubTestAng_B > 360) PubTestAng_B = 360;

        if (PubTestAng_A <-360) PubTestAng_A = -360;
        if (PubTestAng_B <-360 ) PubTestAng_B = -360;

        GreenTran.position= tempCardinal[8];
        CapsuleFrom.position= tempCardinal[8];
       // CapsuleAim.position = tempCardinal[PubCardinalIndex_shoot_07];
      //  RedTran.position = tempCardinal[PubCardinalIndex_move_07];

        _curFrom = GreenTran;
        _curtargetMoveDirection = RedTran;
        _curtargetShootAt = CapsuleAim;

       // _curFrom.position = tempCardinal[8];
       //  _curtargetMoveDirection.position = tempCardinal[PubCardinalIndex_move_07];
       //  _curtargetShootAt.position = tempCardinal[PubCardinalIndex_shoot_07];
       // RedTran.position = _curtargetMoveDirection.position;


        MoveTo_andShootTo(_curFrom.position, _curtargetMoveDirection.position, _curtargetShootAt.position, PressLeftOn, PressRightOn);

        //temptargets = _miniTargets.Get_Final_Enemilocations();
        //tempPlayer = _miniTargets.Get_Playerlocations();
        //tempCardinal = _miniTargets.Get_Cardinallocations();


    }
}
