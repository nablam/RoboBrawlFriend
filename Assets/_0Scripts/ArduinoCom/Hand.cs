using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Hand : MonoBehaviour 
{

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
    protected Vector3 _WalkDir_v3_Normed;
    protected Vector3 _FireDir_v3_Normed;
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
    protected IEnumerator R_0_ReCenter_CorrectHome()
    {
        _cur_THUMB_State = e_THUMB_State.RECENTERING;
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;

    }
    protected IEnumerator R_1_ReCEntererNewHome()
    {
        _cur_THUMB_State = e_THUMB_State.RECENTERING;
        yield return new WaitForSeconds(4);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;
    }
    protected IEnumerator R_2_Tapper()
    {
        _cur_THUMB_State = e_THUMB_State.DROPPING;
        yield return new WaitForSeconds(2);//howlong for firetap NOTSAME AS WHEN PART OF DRAG
        _cur_THUMB_State = e_THUMB_State.LIFTING;
        yield return new WaitForSeconds(1);
        _cur_THUMB_State = e_THUMB_State.RECENTERING;
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;
    }

    protected IEnumerator R_3_FasDasher()
    {
        _cur_THUMB_State = e_THUMB_State.DROPPING;
        yield return new WaitForSeconds(1);
        _cur_THUMB_State = e_THUMB_State.DASHING;
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.LIFTING;
        yield return new WaitForSeconds(1);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;
    }


    protected IEnumerator R_4_OpenFollower()
    {
        _cur_THUMB_State = e_THUMB_State.DROPPING;
        yield return new WaitForSeconds(1);
        _cur_THUMB_State = e_THUMB_State.DASHING;
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.FOLLOWING;
    }

    protected IEnumerator R_5_Braker()
    {
        _cur_THUMB_State = e_THUMB_State.LIFTING;
        yield return new WaitForSeconds(t_drop);
        _cur_THUMB_State = e_THUMB_State.RECENTERING;
        yield return new WaitForSeconds(2);
        _cur_THUMB_State = e_THUMB_State.HoveringReadyToSting;

    }

    public virtual void DoHandInit() {
       
       
    }

    public void DOACTIONWalkThumb(e_ButtonLocationType arg_bHomeType, Vector3 arg_v3_Normed) {
        _WalkDir_v3_Normed = arg_v3_Normed;
        if (arg_bHomeType != (e_ButtonLocationType)_cur_HOME_INDEX)
        {
            //set index of rooutine to recenter 
            _curRunningAction_INDEX = 1;
            StartCoroutine(ROUTINES[_curRunningAction_INDEX]);
            _cur_HOME_INDEX = (int)arg_bHomeType;
        }
        else {

            StartCoroutine(R_4_OpenFollower());
        }
         
         

    }

    private void Update()
    {
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
                break;
            case e_THUMB_State.FOLLOWING:

                UpdatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(_WalkDir_v3_Normed);
                UpdatedHandData.SolinoidState = true;
                break;
         
        }

        //updateddata
        
    }

    void DoCenterHover() { 
    
    
    }

    // protected HandData[] 
     void Start()
    {
       // DoHandInit();
        print("baseHANdStart");
    }
    public virtual void CursorToSelected_POI() { }
     void _ToSelected_POI() { }
}
