using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Joy_D : MonoBehaviour
{
    Vector3 _Local_Fireat_v3_Normed;

    float RadiusToUse = 5f;
    const int Total_Homes = 4;
    HomeBtnData _PRIMARY_HomeBTN_XYR_SlSrSo;
    HomeBtnData _SECOND_HomeBTN_XYR_SlSrSo;
    HomeBtnData _TRECIARY_HomeBTN_XYR_SlSrSo;
    HomeBtnData _MID_HomeBTN_XYR_SlSrSo; //is calculated by my svo model on start
    HomeBtnData Home_BtnPTr;
    HomeBtnData[] BTNZ;
    ServosKinematicSolver _SVO_MODEL;
    e_HandSide _mySide;
    HandData UpdatedHandData;
    bool THUMBUP;
    bool TummBeenDownForOverTime = false;
    bool Transitioning = false;
    void Start()
    {
        _mySide = e_HandSide.RRIGHT_hand;
        _SVO_MODEL = new ServosKinematicSolver(_mySide);
        _PRIMARY_HomeBTN_XYR_SlSrSo = new HomeBtnData(56f, 20f, RadiusToUse, e_ButtonLocationType._0_Main, _SVO_MODEL);
        _SECOND_HomeBTN_XYR_SlSrSo = new HomeBtnData(56f, 20f, RadiusToUse, e_ButtonLocationType._1_SuperFire, _SVO_MODEL);
        _TRECIARY_HomeBTN_XYR_SlSrSo = new HomeBtnData(56f, 20f, RadiusToUse, e_ButtonLocationType._2_GadgetFire, _SVO_MODEL);
        _MID_HomeBTN_XYR_SlSrSo = new HomeBtnData(60f, 0f, RadiusToUse, e_ButtonLocationType._3_Center, _SVO_MODEL);
        BTNZ = new HomeBtnData[Total_Homes] { _PRIMARY_HomeBTN_XYR_SlSrSo, _SECOND_HomeBTN_XYR_SlSrSo, _TRECIARY_HomeBTN_XYR_SlSrSo, _MID_HomeBTN_XYR_SlSrSo };
        Home_BtnPTr = _PRIMARY_HomeBTN_XYR_SlSrSo;
        THUMBUP = true;
        UpdatedHandData = new HandData();
    }
    float _TimCnt = 0.0f;
    float _T_TwiddleTimeThresh = 1f;// 0.4f;use this
    float _TransiTimeCnt = 0f;
    float _TranTimeThreshold = 2f;
    // Update is called once per frame

    public RotatorWithKEYbard RK;
    public TMP_Text mydisplay;

    public void Update_TwiddleStick_atThisHome(e_ButtonLocationType argThisHome, bool argDOit)
    {
        if (!Home_BtnPTr.Validate_same_location(argThisHome))
        {
            Home_BtnPTr = BTNZ[(int)argThisHome];
            Transitioning = true;
            THUMBUP = true;
            _TimCnt = 0f;
            TummBeenDownForOverTime = false;
        }
        if (!Transitioning)
        {
            _TransiTimeCnt = 0f;
            if (THUMBUP && argDOit)
            {
                THUMBUP = false;
                _TimCnt = 0f;
                TummBeenDownForOverTime = false;
                return;
            }

            if (!THUMBUP && !argDOit)
            {
                THUMBUP = true;
                _TimCnt = 0f;
                TummBeenDownForOverTime = false;
                return;
            }
        }
    }
    public float myx, myy;
    private void Update()
    {
        myx = RK.newX;
        myy = RK.newY;
         Run_updateLocally();

        //  UpdatedHandData = _SVO_MODEL.Convert_XY_TO_SvoBiAngs(myy*10, myx*10);
    //    UpdatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(RK.TheDIr, new Vector3(Home_BtnPTr.XFl, Home_BtnPTr.YFl, 0));
        mydisplay.text = Home_BtnPTr.PositionType.ToString();

        EventsManagerLib.CALL_Hand_Broadcast(UpdatedHandData.SR, UpdatedHandData.SL, UpdatedHandData.SolinoidState, _mySide);
    }

    void Run_updateLocally()
    {
        if (Transitioning)
        {
            _TransiTimeCnt += Time.deltaTime;
            UpdatedHandData.SolinoidState = !THUMBUP;

            UpdatedHandData.SL = Home_BtnPTr.Get_precalulatedSLSR().SL;
            UpdatedHandData.SR = Home_BtnPTr.Get_precalulatedSLSR().SR;
            if (_TransiTimeCnt > _TranTimeThreshold) { Transitioning = false; }
        }
        else
        {
            _TimCnt += Time.deltaTime;
            if (!THUMBUP && _TimCnt > _T_TwiddleTimeThresh)
            {
                TummBeenDownForOverTime = true;
            }
            UpdatedHandData.SolinoidState = !THUMBUP;

            UpdatedHandData.SL = Home_BtnPTr.Get_precalulatedSLSR().SL;
            UpdatedHandData.SR = Home_BtnPTr.Get_precalulatedSLSR().SR;
            if (TummBeenDownForOverTime)
            {
                UpdatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(
                    _Local_Fireat_v3_Normed, new Vector3(Home_BtnPTr.XFl_WorldBased, Home_BtnPTr.YFl_WorldBAsed, Home_BtnPTr.RFl));
                UpdatedHandData.SolinoidState = !THUMBUP;
            }
        }
    }
    public void Update_FIREatV3(Vector3 arg_moveVEc)
    {

        _Local_Fireat_v3_Normed = arg_moveVEc.normalized;
    }
    public void Update_HomeLocation(e_ButtonLocationType argNewHome)
    {

        Home_BtnPTr = BTNZ[(int)argNewHome];
        Debug.Log("Home is now " + Home_BtnPTr.PositionType.ToString());


    }

}
