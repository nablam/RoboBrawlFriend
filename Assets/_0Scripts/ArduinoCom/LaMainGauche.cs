using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LaMainGauche : Hand
{

 
    public override void CursorToSelected_POI()
    {
        throw new System.NotImplementedException();
    }
    public override void DoHandInit()
    {
        _mySide = e_HandSide.LEFT_hand;
        _SVO_MODEL = new ServosKinematicSolver(_mySide);
        _PRIMARY_HomeBTN_XYR_SlSrSo = new HomeBtnData(50f,0f,RadiusToUse, e_ButtonLocationType._0_Main , _SVO_MODEL);
        _SECOND_HomeBTN_XYR_SlSrSo = new HomeBtnData(40f, 0f, RadiusToUse, e_ButtonLocationType._1_SuperFire, _SVO_MODEL);
        _TRECIARY_HomeBTN_XYR_SlSrSo = new HomeBtnData(30f, 0f, RadiusToUse, e_ButtonLocationType._2_GadgetFire, _SVO_MODEL);
        _MID_HomeBTN_XYR_SlSrSo = new HomeBtnData(47f, 0f, RadiusToUse, e_ButtonLocationType._3_Center, _SVO_MODEL);
        BTNZ = new HomeBtnData[Total_Homes] { _PRIMARY_HomeBTN_XYR_SlSrSo, _SECOND_HomeBTN_XYR_SlSrSo, _TRECIARY_HomeBTN_XYR_SlSrSo, _MID_HomeBTN_XYR_SlSrSo };
        ROUTINES = new IEnumerator[TotalRoutines] { R_0_ReCenter_CorrectHome(), R_1_ReCEntererNewHome(), R_2_Tapper(), R_3_FasDasher(), R_4_OpenFollower(), R_5_Braker() };
        ROUTINES_ON = new bool[TotalRoutines] ;
        _curRunningAction_INDEX = 0;
        _cur_HOME_INDEX = 0;
        UpdatedHandData = new HandData();
         //Myway
    }
    public float debLs;
    public float debRs;
    private void LateUpdate()
    {
        tb.text = _cur_THUMB_State.ToString();
        if (_myBuzyRunningRoutine) { tb.color = Color.red; }
        else { tb.color = Color.white; }

        debLs = UpdatedHandData.SL;
        debRs = UpdatedHandData.SR;
        //_MAinDirectionVector_to_BeUSED = _Local_move_v3_Normed;
        Update_hd_bystate(_Local_move_v3_Normed);
        EventsManagerLib.CALL_Hand_Broadcast(UpdatedHandData.SR, UpdatedHandData.SL, UpdatedHandData.SolinoidState, _mySide);
       
    }
    void Start()
    {
        print("Gauche HANdStart");
      
        DoHandInit();
        StartCoroutine(FirstTime());
        

    }
    


}
