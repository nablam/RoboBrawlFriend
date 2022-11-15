using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public  class Hand : MonoBehaviour 
{
    public TMP_Text tb;

    //protected Vector3 _MAinDirectionVector_to_BeUSED; //during satrt, each hand will set this to either move or fire vector
    public void Update_MoveandFireVEctors(Vector3 arg_moveVEc, Vector3 argFireVec) {

        _Local_move_v3_Normed = arg_moveVEc.normalized;
        _Local_Fire_v3_Normed = argFireVec.normalized;
    }
    protected Vector3 _Local_move_v3_Normed;
    protected Vector3 _Local_Fire_v3_Normed;

    protected bool _myBuzyRunningRoutine = false;
    protected bool _myWasInited= false;

    protected bool _myThumbIsDown = false; //not touching
    protected e_HandSide _mySide;
    protected float RadiusToUse = 10f;

    protected HomeBtnData _PRIMARY_HomeBTN_XYR_SlSrSo;
    protected HomeBtnData _SECOND_HomeBTN_XYR_SlSrSo;
    protected HomeBtnData _TRECIARY_HomeBTN_XYR_SlSrSo;
    protected HomeBtnData _MID_HomeBTN_XYR_SlSrSo; //is calculated by my svo model on start
    protected HomeBtnData[] BTNZ;
    protected ServosKinematicSolver _SVO_MODEL;
    protected const int Total_Homes = 4;
    protected int _cur_HOME_INDEX = 0;
    protected e_THUMB_State _cur_THUMB_State = 0;
    protected float t_drop = 0.2f;
    protected float t_lift = 0.2f;
    protected float t_drag = 0.3f;
    protected float t_centerin = 0.4f;

    protected const int TotalRoutines = 6;
    protected IEnumerator[] ROUTINES;
    protected bool[] ROUTINES_ON;
    protected int _curRunningAction_INDEX=0;

    protected HandData UpdatedHandData;
 //   protected Vector3 _WalkDir_v3_Normed;
    //protected Vector3 _FireDir_v3_Normed;
    /* public void Start_1()
    {
        if (!Fun1isRunning)
        {
            StartCoroutine(DoRoutine_A());
            print("start1");
        }
        else { print("no can start1 is stillrunning"); }
    }
    public void Cancel_1()
    {
        if (Fun1isRunning)
        {
            StopAllCoroutines();
            Fun1isRunning = false;
            print("stopping 1");
            _mystate = State.Hovering;
        }
        else { print("no can start1 is stillrunning"); }
    }*/


    protected IEnumerator FirstTime()
    {
        _myBuzyRunningRoutine = true;

        _cur_THUMB_State = e_THUMB_State.RECENTERING;
        _cur_HOME_INDEX = 0;//*******************************************************HOmeindex first set
                            //may not need . but worried about Update method firing and bonk slsr-------
        UpdatedHandData.SL = BTNZ[_cur_HOME_INDEX].Get_precalulatedSLSR().SL;
        UpdatedHandData.SR = BTNZ[_cur_HOME_INDEX].Get_precalulatedSLSR().SR;
        UpdatedHandData.SolinoidState = false;
        //these three line up here----------------------------------------------------------------------
        yield return new WaitForSeconds(4);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;
        _myBuzyRunningRoutine = false;
        _myWasInited = true;
    }
    protected IEnumerator R_0_ReCenter_CorrectHome()
    {
        _myBuzyRunningRoutine = true;
        _cur_THUMB_State = e_THUMB_State.RECENTERING;
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;
        _curRunningAction_INDEX = 0;
        _myBuzyRunningRoutine = false;

    }
    protected IEnumerator R_1_ReCEntererNewHome()
    {
        _myBuzyRunningRoutine = true;
        _cur_THUMB_State = e_THUMB_State.RECENTERING;
        yield return new WaitForSeconds(4);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;
        _curRunningAction_INDEX = 0;
        _myBuzyRunningRoutine = false;
    }
    protected IEnumerator R_2_Tapper()
    {
        _myBuzyRunningRoutine = true;
        _cur_THUMB_State = e_THUMB_State.DROPPING;
        yield return new WaitForSeconds(2);//howlong for firetap NOTSAME AS WHEN PART OF DRAG
        _cur_THUMB_State = e_THUMB_State.LIFTING;
        UpdatedHandData.SL = BTNZ[_cur_HOME_INDEX].Get_precalulatedSLSR().SL;
        UpdatedHandData.SR = BTNZ[_cur_HOME_INDEX].Get_precalulatedSLSR().SR;
        UpdatedHandData.SolinoidState = false;
        yield return new WaitForSeconds(1);
        _cur_THUMB_State = e_THUMB_State.RECENTERING;
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;
        _curRunningAction_INDEX = 0;
        _myBuzyRunningRoutine = false;
    }

    protected IEnumerator R_3_FasDasher()
    {
        _myBuzyRunningRoutine = true;
        _cur_THUMB_State = e_THUMB_State.DROPPING;
        yield return new WaitForSeconds(1);
        _cur_THUMB_State = e_THUMB_State.DASHING;
        if (_mySide == e_HandSide.LEFT_hand) {
         //   UpdatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(_Local_move_v3_Normed);
        } else {
           // UpdatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(_Local_Fire_v3_Normed);
        }
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.LIFTING;
        UpdatedHandData.SolinoidState = false;
        yield return new WaitForSeconds(1);
        _cur_THUMB_State = e_THUMB_State.RECENTERING;
        yield return new WaitForSeconds(1);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;
        _curRunningAction_INDEX = 0;
        _myBuzyRunningRoutine = false;
    }


    protected IEnumerator R_4_OpenFollower()
    {
        _myBuzyRunningRoutine = true;
        _cur_THUMB_State = e_THUMB_State.DROPPING;
        yield return new WaitForSeconds(1);
        _cur_THUMB_State = e_THUMB_State.DASHING;
        if (_mySide == e_HandSide.LEFT_hand)
        {
           // UpdatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(_Local_move_v3_Normed);
        }
        else
        {
          //  UpdatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(_Local_Fire_v3_Normed);
        }
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.FOLLOWING;
        _curRunningAction_INDEX = 0;
        _myBuzyRunningRoutine = false;
    }

    protected IEnumerator R_5_Braker()
    {
        _myBuzyRunningRoutine = true;
        _cur_THUMB_State = e_THUMB_State.LIFTING;
        yield return new WaitForSeconds(1);
        _cur_THUMB_State = e_THUMB_State.RECENTERING;
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;
        _curRunningAction_INDEX = 0;
        _myBuzyRunningRoutine = false;
    }

    //========================================New Coreoutine system SA Single actions
    protected IEnumerator SA_DropThumb()
    {
        _cur_THUMB_State = e_THUMB_State.DROPPING;
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;

    }
    IEnumerator C1()
    {
        print("c1 w1");
        yield return new WaitForSeconds(1);
        print("c1 fin1");
        print("c1 aboit 2");
        yield return new WaitForSeconds(2);
        print("c1 fFIN 1DONE");
    }
    IEnumerator C2()
    {
        yield return StartCoroutine(C1());
        print("c2 W1");
        yield return new WaitForSeconds(1);
        print("c2 fin1");
        print("c2 aboit 2");
        yield return new WaitForSeconds(2);
        print("c2 finic1DONE ");
    }
    IEnumerator C3()
    {
        print("c3 starts and .....");
        yield return StartCoroutine(C2());
        print(".... alldone");


    }
    public virtual void DoHandInit()
    {


    }

    public virtual void Init_curstate()
    {


    }
    public void QUICKHOVER()
    {
        if (_myBuzyRunningRoutine||!_myBuzyRunningRoutine) {
            StopAllCoroutines();
            _myBuzyRunningRoutine = false;
        }
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;

    }

    public void DOACTIONWalkThumb(e_ButtonLocationType arg_bHomeType )
    {
       
        if (_myBuzyRunningRoutine) return;

        if ((int)arg_bHomeType != _cur_HOME_INDEX)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 1;
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
            _cur_HOME_INDEX = (int)arg_bHomeType;
        }
        else
        if (_cur_THUMB_State == e_THUMB_State.HoveringReadyToSting && _curRunningAction_INDEX == 1)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 4; //openfollow
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }
        else
        if (_cur_THUMB_State == e_THUMB_State.HoveringReadyToSting && _curRunningAction_INDEX != 4)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 4; //openfollow
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }
    public void DOACTION_CLOSEWALK() //still keep updating the vector
    {
        
        if (_myBuzyRunningRoutine) return;


        if (_cur_THUMB_State == e_THUMB_State.FOLLOWING && _curRunningAction_INDEX != 5)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 5; //breaker
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }
    //*********************************************************************************************
    public void DOACTION_FireTap(e_ButtonLocationType arg_bHomeType )
    {
         
        if (_myBuzyRunningRoutine) return;

        if ((int)arg_bHomeType != _cur_HOME_INDEX)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 1;
            _cur_HOME_INDEX = (int)arg_bHomeType;
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }else

        if (_cur_THUMB_State == e_THUMB_State.HoveringReadyToSting && _curRunningAction_INDEX != 2)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 2; //Tapper
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }

    public void DOACTION_FastFire(e_ButtonLocationType arg_bHomeType )
    {
         
        if (_myBuzyRunningRoutine) return;

        if ((int)arg_bHomeType !=  _cur_HOME_INDEX)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 1;
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
            _cur_HOME_INDEX = (int)arg_bHomeType;
        }
        else
        if (_cur_THUMB_State == e_THUMB_State.HoveringReadyToSting && _curRunningAction_INDEX != 3)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 3; //Fastdasher
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }
    public void DOACTION_FireWait(e_ButtonLocationType arg_bHomeType )
    {
         
        if (_myBuzyRunningRoutine) return;

        if ((int)arg_bHomeType != _cur_HOME_INDEX)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 1;
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
            _cur_HOME_INDEX = (int)arg_bHomeType;
        }
        else
        if (_cur_THUMB_State == e_THUMB_State.RECENTERING && _curRunningAction_INDEX != 4)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 4; //openfollower
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }
    public void DOACTION_CLOSEFirewait()
    {
         
        if (_myBuzyRunningRoutine) return;


        if (_cur_THUMB_State == e_THUMB_State.FOLLOWING && _curRunningAction_INDEX != 5)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 5; //breaker
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }


    //private void Update()
    //{
    //    Update_hd_bystate();
    //    EventsManagerLib.CALL_Hand_Broadcast(UpdatedHandData.SR, UpdatedHandData.SL, UpdatedHandData.SolinoidState, _mySide);
    //}
    protected void Update_hd_bystate(Vector3 argThisDir)
    {
        UpdatedHandData.SL = BTNZ[_cur_HOME_INDEX].Get_precalulatedSLSR().SL;
        UpdatedHandData.SR = BTNZ[_cur_HOME_INDEX].Get_precalulatedSLSR().SR;
        switch (_cur_THUMB_State) {
            case e_THUMB_State.IDLE:
                UpdatedHandData.SolinoidState = false;
                break;
            case e_THUMB_State.RECENTERING:

                UpdatedHandData.SolinoidState = false;
                break;
            case e_THUMB_State.HoveringReadyToSting:
 
                UpdatedHandData.SolinoidState = false;
                break;
            case e_THUMB_State.DROPPING:

                UpdatedHandData.SolinoidState = true;
                break;

            case e_THUMB_State.LIFTING:
                UpdatedHandData.SolinoidState = false;
                break;
            case e_THUMB_State.DASHING:
                UpdatedHandData.SolinoidState = true;
                break;
            case e_THUMB_State.FOLLOWING:

              //  UpdatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(argThisDir);
                UpdatedHandData.SolinoidState = true;
                break;
         
        }

        //updateddata canbe broadcast or grabbed
        
    }

    void DoCenterHover() { 
    
    
    }

    // protected HandData[] 
     void Start()
    {
        _cur_THUMB_State = e_THUMB_State.IDLE;
       // DoHandInit();
        print("baseHANdStart");
    }
    public virtual void CursorToSelected_POI() { }
     void _ToSelected_POI() { }
}
/*
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 *   public void DOACTIONWalkThumb(e_ButtonLocationType arg_bHomeType, Vector3 arg_v3_Normed) {
        _WalkDir_v3_Normed = arg_v3_Normed;
        if (_myBuzyRunningRoutine) return;
     
        if (arg_bHomeType != (e_ButtonLocationType)_cur_HOME_INDEX)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 1;
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
            _cur_HOME_INDEX = (int)arg_bHomeType;
        }

        if (_cur_THUMB_State == e_THUMB_State.HoveringReadyToSting) {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 4; //openfollow
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }
    public void DOACTION_CLOSEWALK( Vector3 arg_v3_Normed) //still keep updating the vector
    {
        _WalkDir_v3_Normed = arg_v3_Normed;
        if (_myBuzyRunningRoutine) return;

      
        if (_cur_THUMB_State == e_THUMB_State.FOLLOWING)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 5; //breaker
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }
    //*********************************************************************************************
    public void DOACTION_FireTap(e_ButtonLocationType arg_bHomeType, Vector3 arg_v3_Normed)
    {
        _FireDir_v3_Normed = arg_v3_Normed;
        if (_myBuzyRunningRoutine) return;

        if (arg_bHomeType != (e_ButtonLocationType)_cur_HOME_INDEX)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 1;
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
            _cur_HOME_INDEX = (int)arg_bHomeType;
        }

        if (_cur_THUMB_State == e_THUMB_State.HoveringReadyToSting)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 2; //Tapper
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }

    public void DOACTION_FastFire(e_ButtonLocationType arg_bHomeType, Vector3 arg_v3_Normed)
    {
        _FireDir_v3_Normed = arg_v3_Normed;
        if (_myBuzyRunningRoutine) return;

        if (arg_bHomeType != (e_ButtonLocationType)_cur_HOME_INDEX)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 1;
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
            _cur_HOME_INDEX = (int)arg_bHomeType;
        }

        if (_cur_THUMB_State == e_THUMB_State.HoveringReadyToSting)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 3; //Fastdasher
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }
    public void DOACTION_FireWait(e_ButtonLocationType arg_bHomeType, Vector3 arg_v3_Normed)
    {
        _FireDir_v3_Normed = arg_v3_Normed;
        if (_myBuzyRunningRoutine) return;

        if (arg_bHomeType != (e_ButtonLocationType)_cur_HOME_INDEX)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 1;
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
            _cur_HOME_INDEX = (int)arg_bHomeType;
        }

        if (_cur_THUMB_State == e_THUMB_State.HoveringReadyToSting)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 4; //openfollower
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }
    public void DOACTION_CLOSEFirewait(Vector3 arg_v3_Normed) //still keep updating the vector
    {
        _FireDir_v3_Normed = arg_v3_Normed;
        if (_myBuzyRunningRoutine) return;


        if (_cur_THUMB_State == e_THUMB_State.FOLLOWING)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 5; //breaker
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
        }

    }
 * 
 * 
 * 
 * 
 * 
 * 
 *
 
 
 
 
 
 
 
 
 */