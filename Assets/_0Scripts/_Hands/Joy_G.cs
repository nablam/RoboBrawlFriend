using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Joy_G : MonoBehaviour
{

    #region _private_Vars
    Vector3 _Local_move_v3_Normed;

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
    float RadiusToUse = 6f;
    float _T_Cnt_Dashing = 0.0f;
    float _Time_Dashing_THRESH = 1f;// 0.4f;use this
    float _T_Cnt_Transit = 0f;
    float _Time_Transit_THRESH = 2f;
    #endregion

    #region Public_Vars

    #endregion

    #region Enabled_Disable_Awake_Start_Destroy
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

    private void Awake()
    {

    }
    void Start()
    {
        _mySide = e_HandSide.LEFT_hand;
        _SVO_MODEL = new ServosKinematicSolver(_mySide);
        _PRIMARY_HomeBTN_XYR_SlSrSo = new HomeBtnData(56, 0f, RadiusToUse, e_ButtonLocationType._0_Main, _SVO_MODEL);
        _SECOND_HomeBTN_XYR_SlSrSo = new HomeBtnData(60f, 0f, RadiusToUse, e_ButtonLocationType._1_SuperFire, _SVO_MODEL);
        _TRECIARY_HomeBTN_XYR_SlSrSo = new HomeBtnData(62f, 0f, RadiusToUse, e_ButtonLocationType._2_GadgetFire, _SVO_MODEL);
        _MID_HomeBTN_XYR_SlSrSo = new HomeBtnData(60f, 0f, RadiusToUse, e_ButtonLocationType._3_Center, _SVO_MODEL);
        BTNZ = new HomeBtnData[Total_Homes] { _PRIMARY_HomeBTN_XYR_SlSrSo, _SECOND_HomeBTN_XYR_SlSrSo, _TRECIARY_HomeBTN_XYR_SlSrSo, _MID_HomeBTN_XYR_SlSrSo };
        Home_BtnPTr = _PRIMARY_HomeBTN_XYR_SlSrSo;
        THUMBUP = true;
        UpdatedHandData = new HandData();
        _T_Cnt_Dashing = 0.0f;
        _Time_Dashing_THRESH = AppSettings.Instance.TimeThreshold_Dashing; ;// 1f;// 0.4f;use this
        _T_Cnt_Transit = 0f;
        _Time_Transit_THRESH = AppSettings.Instance.TimeThreshold_Transiting;
        RadiusToUse = AppSettings.Instance.Radius_D1;
    }
    private void OnDestroy()
    {

    }
    #endregion

    #region EventHAndlers


    #endregion

    #region _private_methods
    void Run_updateLocallyG()
    {
        if (Transitioning)
        {
            _T_Cnt_Transit += Time.deltaTime;
            UpdatedHandData.SolinoidState = !THUMBUP;

            UpdatedHandData.SL = Home_BtnPTr.Get_precalulatedSLSR().SL;
            UpdatedHandData.SR = Home_BtnPTr.Get_precalulatedSLSR().SR;
            if (_T_Cnt_Transit > _Time_Transit_THRESH) { Transitioning = false; }
        }
        else
        {
            _T_Cnt_Dashing += Time.deltaTime;
            if (!THUMBUP && _T_Cnt_Dashing > _Time_Dashing_THRESH)
            {
                TummBeenDownForOverTime = true;
            }
            UpdatedHandData.SolinoidState = !THUMBUP;

            UpdatedHandData.SL = Home_BtnPTr.Get_precalulatedSLSR().SL;
            UpdatedHandData.SR = Home_BtnPTr.Get_precalulatedSLSR().SR;
            if (TummBeenDownForOverTime)
            {
                UpdatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(
                    _Local_move_v3_Normed, new Vector3(Home_BtnPTr.XFl_WorldBased, Home_BtnPTr.YFl_WorldBAsed, Home_BtnPTr.RFl));
                UpdatedHandData.SolinoidState = !THUMBUP;
            }
        }
    }
    #endregion

    #region PUBLIC_Methods

    public void Update_UseJoystick_OnLocation(e_ButtonLocationType argThisHome, bool argDOit)
    {
        if (!Home_BtnPTr.Validate_same_location(argThisHome))
        {
            Home_BtnPTr = BTNZ[(int)argThisHome];
            Transitioning = true;
            THUMBUP = true;
            _T_Cnt_Dashing = 0f;
            TummBeenDownForOverTime = false;
        }
        if (!Transitioning)
        {
            _T_Cnt_Transit = 0f;
            if (THUMBUP && argDOit)
            {
                THUMBUP = false;
                _T_Cnt_Dashing = 0f;
                TummBeenDownForOverTime = false;
                return;
            }

            if (!THUMBUP && !argDOit)
            {
                THUMBUP = true;
                _T_Cnt_Dashing = 0f;
                TummBeenDownForOverTime = false;
                return;
            }
        }
    }

    public void Update_MoveToV3(Vector3 arg_moveVEc)
    {
        _Local_move_v3_Normed = arg_moveVEc.normalized;
    }
    public void Update_HomeLocation(e_ButtonLocationType argNewHome)
    {
        Home_BtnPTr = BTNZ[(int)argNewHome];
        Debug.Log("Home is now " + Home_BtnPTr.PositionType.ToString());
    }
    #endregion

    #region UPDATE
    private void Update()
    {
        Run_updateLocallyG();
        EventsManagerLib.CALL_Hand_Broadcast(UpdatedHandData.SR, UpdatedHandData.SL, UpdatedHandData.SolinoidState, _mySide);
    }
    #endregion











 
}
