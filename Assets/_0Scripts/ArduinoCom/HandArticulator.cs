using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandArticulator   : MonoBehaviour
{
    e_THUMB_State _cur_THUMB_State = 0;
    HandData UpdatedHandData;
    ServosKinematicSolver _SVO_MODEL;

    const int Total_Homes = 4;
    HomeBtnData _PRI_HomeBTN;
    HomeBtnData _SEC_HomeBTN;
    HomeBtnData _TRE_HomeBTN;
    HomeBtnData _MID_HomeBTN; //is calculated by my svo model on start
    HomeBtnData[] BTNZ;
    int _cur_HOME_INDEX = 0;
    e_HandSide _mySide;
    float RadiusToUse = 10f;

    Vector3 _WalkDir_v3_Normed;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    void DoHandInit()
    {
        _mySide = e_HandSide.LEFT_hand;
        _SVO_MODEL = new ServosKinematicSolver(_mySide);
        //_PRI_HomeBTN = new HomeBtnData(50f, 0f, RadiusToUse, e_ButtonLocationType._0_Main, _SVO_MODEL);
        //_SEC_HomeBTN = new HomeBtnData(40f, 0f, RadiusToUse, e_ButtonLocationType._1_SuperFire, _SVO_MODEL);
        //_TRE_HomeBTN = new HomeBtnData(30f, 0f, RadiusToUse, e_ButtonLocationType._2_GadgetFire, _SVO_MODEL);
        //_MID_HomeBTN = new HomeBtnData(47f, 0f, RadiusToUse, e_ButtonLocationType._3_Center, _SVO_MODEL);
        BTNZ = new HomeBtnData[Total_Homes] { _PRI_HomeBTN, _SEC_HomeBTN, _TRE_HomeBTN, _MID_HomeBTN };
        UpdatedHandData = new HandData();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void DOACTIONWalkThumb(e_ButtonLocationType arg_bHomeType, Vector3 arg_v3_Normed)
    {
        



    }
    private void Update_hd_bystate()
    {
        switch (_cur_THUMB_State)
        {
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

              //  UpdatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(_WalkDir_v3_Normed);
                UpdatedHandData.SolinoidState = true;
                break;

        }

        //updateddata

    }
}
