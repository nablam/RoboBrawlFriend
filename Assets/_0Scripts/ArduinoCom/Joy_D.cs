using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Joy_D : MonoBehaviour
{
    Vector3 _Local_Fireat_v3_Normed;


    const int Total_Homes = 4;
    HomeBtnData _PRIMARY_HomeBTN_XYR_SlSrSo;
    HomeBtnData _SECOND_HomeBTN_XYR_SlSrSo;
    HomeBtnData _TRECIARY_HomeBTN_XYR_SlSrSo;
    HomeBtnData _MID_HomeBTN_XYR_SlSrSo;
    HomeBtnData Home_BtnPTr;
    HomeBtnData[] BTNZ;
    ServosKinematicSolver _SVO_MODEL;
    e_HandSide _mySide;
    HandData UpdatedHandData;
    bool THUMBUP;
    bool TummBeenDownForOverTime = false;
    bool Transitioning = false;
    float _RadiusToUse = 5f;
    float _Cnt_dash = 0.0f;
    float _TimeTHRESH_Dashing = 1f;// 0.4f;use this
    float _Cnt_transit = 0f;
    float _TimeTHRESH_Transitting = 2f;

    public TMP_Text mydisplay;
    void Start()
    {
        _mySide = e_HandSide.RRIGHT_hand;
        _SVO_MODEL = new ServosKinematicSolver(_mySide);
        _PRIMARY_HomeBTN_XYR_SlSrSo = new HomeBtnData(60f, 0f, _RadiusToUse, e_ButtonLocationType._0_Main, _SVO_MODEL);
        _SECOND_HomeBTN_XYR_SlSrSo = new HomeBtnData(56f, 20f, _RadiusToUse, e_ButtonLocationType._1_SuperFire, _SVO_MODEL);
        _TRECIARY_HomeBTN_XYR_SlSrSo = new HomeBtnData(56f, -20f, _RadiusToUse, e_ButtonLocationType._2_GadgetFire, _SVO_MODEL);
        _MID_HomeBTN_XYR_SlSrSo = new HomeBtnData(60f, 0f, _RadiusToUse, e_ButtonLocationType._3_Center, _SVO_MODEL);
        BTNZ = new HomeBtnData[Total_Homes] { _PRIMARY_HomeBTN_XYR_SlSrSo, _SECOND_HomeBTN_XYR_SlSrSo, _TRECIARY_HomeBTN_XYR_SlSrSo, _MID_HomeBTN_XYR_SlSrSo };
        Home_BtnPTr = _PRIMARY_HomeBTN_XYR_SlSrSo;
        THUMBUP = true;
        UpdatedHandData = new HandData();

        _Cnt_dash = 0.0f;
        _TimeTHRESH_Dashing = AppSettings.Instance.TimeThreshold_Dashing;
        _Cnt_transit = 0f;
        _TimeTHRESH_Transitting = AppSettings.Instance.TimeThreshold_Transiting;
        _RadiusToUse = AppSettings.Instance.Radius_D1;
    }

    int Cnt_shotsfired = 0;
    public void Update_TwiddleStick_atThisHome(e_ButtonLocationType argThisHome, bool argDOit)
    {
        if (!Home_BtnPTr.Validate_same_location(argThisHome))
        {
            Home_BtnPTr = BTNZ[(int)argThisHome];

            if (!THUMBUP) {
                //consider this a tap up fire cound
                Cnt_shotsfired++;
            }
            Transitioning = true;
            THUMBUP = true;
            _Cnt_dash = 0f;
            TummBeenDownForOverTime = false;
        }
        if (!Transitioning)
        {
            _Cnt_transit = 0f;
            if (THUMBUP && argDOit)
            {
                THUMBUP = false;
                _Cnt_dash = 0f;
                TummBeenDownForOverTime = false;

                return;
            }

            if (!THUMBUP && TummBeenDownForOverTime && !argDOit)  //been toushing screen for less than dashtime annd I get a false
            {
                THUMBUP = true;
                _Cnt_dash = 0f;
                TummBeenDownForOverTime = false;
                //Shotfired EVENT
                Cnt_shotsfired++;
                return;
            }

            if (!THUMBUP && !TummBeenDownForOverTime && !argDOit)  //been toushing screen for less than dashtime annd I get a false
            {
                THUMBUP = true;
                _Cnt_dash = 0f;
                TummBeenDownForOverTime = false;
                //Shotfired EVENT
                Cnt_shotsfired++;
                return;
            }
        }
    }
    private void Update()
    {
        Run_updateLocally();
        mydisplay.text = Home_BtnPTr.PositionType.ToString() + " " + Cnt_shotsfired.ToString() ;

        EventsManagerLib.CALL_Hand_Broadcast(UpdatedHandData.SR, UpdatedHandData.SL, UpdatedHandData.SolinoidState, _mySide);
    }

    void Run_updateLocally()
    {
        if (Transitioning)
        {
            _Cnt_transit += Time.deltaTime;
            UpdatedHandData.SolinoidState = !THUMBUP;

            UpdatedHandData.SL = Home_BtnPTr.Get_precalulatedSLSR().SL;
            UpdatedHandData.SR = Home_BtnPTr.Get_precalulatedSLSR().SR;
            if (_Cnt_transit > _TimeTHRESH_Transitting) { Transitioning = false; }
        }
        else
        {
            _Cnt_dash += Time.deltaTime;
            if (!THUMBUP && _Cnt_dash > _TimeTHRESH_Dashing)
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
        Transitioning = true;
        _Cnt_transit = 0;


    }

}
