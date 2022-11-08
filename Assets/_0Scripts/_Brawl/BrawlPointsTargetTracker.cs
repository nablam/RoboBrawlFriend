using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlPointsTargetTracker : MonoBehaviour
{

    [SerializeField]
    public Vector3 PubFrom;
    [SerializeField]
    public Vector3 PubTo;
    [SerializeField]
    public Vector3 NormalizedFromCenter;

    float mouseXdebug_fl;
    float mouseYdebug_fl;
    public float RawMouse_X;
    public float RawMouse_Y;

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

    Vector3 _AgreedPlayerPos;
    Vector3 _Agreed_moveTargetPos;
    Vector3 _Agreed_EnemyTArgetPos;
    Vector3 _moveDir_NotNormalized;
    Vector3 _NORMALIZED_moveDir;
    Vector3 _Enemy_RelativPos_normalized;
    MinimapTest _miniTargets;
   

    public int PubCardinalIndex_move_07;
    public int PubCardinalIndex_shoot_07;
 
    bool EmergencyBreakOn;


    void OnDrawGizmos()
    {

        PubFrom = GreenTran.position;
        PubTo = RedTran.position;


        NormalizedFromCenter = (PubTo - PubFrom).normalized;
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, NormalizedFromCenter);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, NormalizedFromCenter * 100);

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
    public void INITme_giveemminimap(MinimapTest argMinimap)
    {
        _miniTargets = argMinimap;
        temptargets = _miniTargets.Get_Final_Enemilocations();
        tempPlayer = _miniTargets.Get_Playerlocations();
        tempCardinal = _miniTargets.Get_Cardinallocations();


        _moveDir_NotNormalized= _NORMALIZED_moveDir = Vector3.up;
         _Enemy_RelativPos_normalized=Vector3.up;








    }
  
    float Get_360_angle(Vector3 argFrom, Vector3 argto)
    {
        Vector3 Temp_NormalizedFromCenter = (argFrom - argto).normalized;
        var angle = Mathf.Atan2(Temp_NormalizedFromCenter.x, Temp_NormalizedFromCenter.y) * Mathf.Rad2Deg;
        float angle360 = (angle + 180) % 360;
        // Debug.Log(angle360);
        return angle360;
    }

    void Start()
    {
      
    }


    void Update()
    {
        RawMouse_X = Input.mousePosition.x;
        RawMouse_Y = Input.mousePosition.y;
        FectchMousePos(RawMouse_X, RawMouse_Y);
        Fetch_targets();

        _AgreedPlayerPos = _curFrom.position;
        _Agreed_moveTargetPos = _curtargetMoveDirection.position;
        _Agreed_EnemyTArgetPos = _curtargetShootAt.position;

        _moveDir_NotNormalized = (_AgreedPlayerPos - _Agreed_moveTargetPos);
        _NORMALIZED_moveDir = (_AgreedPlayerPos - _Agreed_moveTargetPos).normalized;
        _Enemy_RelativPos_normalized = (_AgreedPlayerPos - _Agreed_EnemyTArgetPos).normalized;


    }
    public Vector3 Get_V3_MoveDir_Normalized() { return this._NORMALIZED_moveDir; }
    public Vector3 Get_V3_EnemyDir_NOrmalized() { return this._Enemy_RelativPos_normalized; }
    public Vector3 Get_V3_MoveDir_NOT_normed() { return this._moveDir_NotNormalized; }
    
    void Fetch_targets()
    {
        GreenTran.position = tempCardinal[8];
        CapsuleFrom.position = tempCardinal[8];
        _curFrom = GreenTran;
        _curtargetMoveDirection = RedTran;
        _curtargetShootAt = CapsuleAim;

    }
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

        //PressLeftOn = false;
        //PressRightOn = false;


        if (PubCardinalIndex_move_07 > 7) PubCardinalIndex_move_07 = 7;
        if (PubCardinalIndex_move_07 < 1) PubCardinalIndex_move_07 = 0;

        if (PubCardinalIndex_shoot_07 > 7) PubCardinalIndex_shoot_07 = 7;
        if (PubCardinalIndex_shoot_07 < 1) PubCardinalIndex_shoot_07 = 0;

        //if (PubTestAng_A > 360) PubTestAng_A = 360;
        //if (PubTestAng_B > 360) PubTestAng_B = 360;

        //if (PubTestAng_A < -360) PubTestAng_A = -360;
        //if (PubTestAng_B < -360) PubTestAng_B = -360;
    }

    void FetchKEyboardInouts()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            EmergencyBreakOn = true;
        }
    }
}


/*
 void BAsicTestMoveShoot()
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


        if (Input.GetKeyDown(KeyCode.S))
        {
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

        if (PubTestAng_A < -360) PubTestAng_A = -360;
        if (PubTestAng_B < -360) PubTestAng_B = -360;

        GreenTran.position = tempCardinal[8];
        CapsuleFrom.position = tempCardinal[8];
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
        //tempCardinal = _miniTargets.Get_Cardinallocations(); }
    }
 */