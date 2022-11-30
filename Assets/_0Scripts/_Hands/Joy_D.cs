using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Joy_D : MonoBehaviour
{

    #region _private_Vars
    // System.Action<float> timerptr = null;
    Vector3 _Local_Fireat_v3_Normed;
    const int Total_Homes = 4;
    HomeBtnData _PRIMARY_HomeBTN_XYR_SlSrSo;
    HomeBtnData _SECOND_HomeBTN_XYR_SlSrSo;
    HomeBtnData _TRECIARY_HomeBTN_XYR_SlSrSo;
    HomeBtnData _MID_HomeBTN_XYR_SlSrSo;
    HomeBtnData _home_BtnPTr;
    HomeBtnData[] BTNZ;
    ServosKinematicSolver _SVO_MODEL;
    e_HandSide _mySide;
    HandData _updatedHandData;
    bool THUMBUP;
    bool TummBeenDownForOverTime = false;
    bool _transitioning = false;
    float _RadiusToUse = 5f;
    float _Cnt_TapTime = 0f;
    float _TimeTHRESH_Tapping = 0.5f;
    float _Cnt_dash = 0.0f;
    float _TimeTHRESH_Dashing = 1f;// 0.4f;use this
    float _Cnt_transit = 0f;
    float _Cnt_Action = 0f;
    float _TimeTHRESH_Transitting = 2f;
    float _CNT_TIMER = 0f;

    int Cnt_shotsfired = 0;
    bool postActionWasSet = false;

    bool _transiUNLOCKED;
    bool _ActioniUNLOCKED;
    float _ShortOrLong;
    float _nextActionTime;
    bool _finishedTransitionAndAction = false;
    bool _inited;
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
        _mySide = e_HandSide.RRIGHT_hand;
        _SVO_MODEL = new ServosKinematicSolver(_mySide);
        _PRIMARY_HomeBTN_XYR_SlSrSo = new HomeBtnData(51f, 0f, _RadiusToUse, e_ButtonLocationType._0_Main, _SVO_MODEL);
        _SECOND_HomeBTN_XYR_SlSrSo = new HomeBtnData(56f, 20f, _RadiusToUse, e_ButtonLocationType._1_SuperFire, _SVO_MODEL);
        _TRECIARY_HomeBTN_XYR_SlSrSo = new HomeBtnData(56f, -20f, _RadiusToUse, e_ButtonLocationType._2_GadgetFire, _SVO_MODEL);
        _MID_HomeBTN_XYR_SlSrSo = new HomeBtnData(60f, 0f, _RadiusToUse, e_ButtonLocationType._3_Center, _SVO_MODEL);
        BTNZ = new HomeBtnData[Total_Homes] { _PRIMARY_HomeBTN_XYR_SlSrSo, _SECOND_HomeBTN_XYR_SlSrSo, _TRECIARY_HomeBTN_XYR_SlSrSo, _MID_HomeBTN_XYR_SlSrSo };
        _home_BtnPTr = _PRIMARY_HomeBTN_XYR_SlSrSo;
        THUMBUP = true;
        _updatedHandData = new HandData();

        _Cnt_dash = 0.0f;
        _TimeTHRESH_Tapping = AppSettings.Instance.TimeThreshold_Tapping;     //.5
        _TimeTHRESH_Dashing = AppSettings.Instance.TimeThreshold_Dashing;     //1
        _TimeTHRESH_Transitting = AppSettings.Instance.TimeThreshold_Transiting;//2
        _Cnt_transit = 0f;
        _RadiusToUse = AppSettings.Instance.Radius_D1;
    }
    private void OnDestroy()
    {

    }
    #endregion

    #region EventHAndlers


    #endregion

    #region _private_methods
    void RunTransTimer()
    {

        if (_transiUNLOCKED)
        {
            _Cnt_transit += Time.deltaTime;
            _ActioniUNLOCKED = false;
            _transitioning = true;
            if (_Cnt_transit > _ShortOrLong)
            {
                _transitioning = false;
                _transiUNLOCKED = false;
                _ActioniUNLOCKED = true;
                _updatedHandData.SolinoidState = true;
                _Cnt_Action = 0;
            }
        }
    }

    void runMiniRoutine()
    {

        if (_ActioniUNLOCKED)
        {
            _Cnt_Action += Time.deltaTime;
            _transiUNLOCKED = false;
            _transitioning = false;
            if (_Cnt_Action > _nextActionTime)
            {
                _updatedHandData.SolinoidState = true;
                _ActioniUNLOCKED = false;
                _updatedHandData.SolinoidState = false;
                _updatedHandData.SL = _home_BtnPTr.Get_precalulatedSLSR().SL;
                _updatedHandData.SR = _home_BtnPTr.Get_precalulatedSLSR().SR;
                _finishedTransitionAndAction = true;
            }
            else
              if (_Cnt_Action > _TimeTHRESH_Tapping)
            {

                _updatedHandData.SolinoidState = false;

                _updatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(
                    _Local_Fireat_v3_Normed, new Vector3(_home_BtnPTr.XFl_WorldBased, _home_BtnPTr.YFl_WorldBAsed, _home_BtnPTr.RFl));

            }
            else
            if (_Cnt_Action > _TimeTHRESH_Tapping && _finishedTransitionAndAction && _Cnt_Action < _TimeTHRESH_Dashing)
            {
                _updatedHandData.SolinoidState = false;
                _updatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(
                    _Local_Fireat_v3_Normed, new Vector3(_home_BtnPTr.XFl_WorldBased, _home_BtnPTr.YFl_WorldBAsed, _home_BtnPTr.RFl));
            }
            else
                  if (_Cnt_Action > _TimeTHRESH_Dashing && _finishedTransitionAndAction)
            {
                _updatedHandData.SolinoidState = true;
                _updatedHandData = _SVO_MODEL.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(
                    _Local_Fireat_v3_Normed, new Vector3(_home_BtnPTr.XFl_WorldBased, _home_BtnPTr.YFl_WorldBAsed, _home_BtnPTr.RFl));

            }
        }
    }
    void Run_updateLocally()
    {
        RunTransTimer();
        runMiniRoutine();

        if (_transitioning)
        {
            //mydisplay.text = "tansi" + _Cnt_transit;
            _updatedHandData.SolinoidState = false;
            _updatedHandData.SL = _home_BtnPTr.Get_precalulatedSLSR().SL;
            _updatedHandData.SR = _home_BtnPTr.Get_precalulatedSLSR().SR;
        }
        else
        {
            //mydisplay.text = "action" + _Cnt_Action;
            _updatedHandData.SolinoidState = true;
        }
        if (_finishedTransitionAndAction || !_inited)
        {
            _updatedHandData.SolinoidState = false;
        }
    }


    #endregion

    #region PUBLIC_Methods
    public void Update_FIREatV3(Vector3 arg_moveVEc)
    {
        _Local_Fireat_v3_Normed = arg_moveVEc.normalized;
    }
    public void Update_HomeLocation(e_ButtonLocationType argNewHome)
    {
        _home_BtnPTr = BTNZ[(int)argNewHome];
        Debug.Log("Home is now " + _home_BtnPTr.PositionType.ToString());
        _transitioning = true;
        _Cnt_transit = 0;
    }

    public void DoTransitAction(float arg_t, float ArgNectActionTime, e_ButtonLocationType arghome)
    {
        _inited = true;
        _ShortOrLong = arg_t;
        _nextActionTime = ArgNectActionTime;
        _transiUNLOCKED = true;
        _transitioning = true;
        _Cnt_transit = 0f;
        _home_BtnPTr = BTNZ[(int)arghome];
        _ActioniUNLOCKED = false;
        _finishedTransitionAndAction = false;
    }

    public void DoForceFireUnload()
    {
        _inited = false;
        _transiUNLOCKED = false;
        _transitioning = true;
        _Cnt_transit = 0f;
        _Cnt_Action = 0;
        _updatedHandData.SolinoidState = false;
        _ActioniUNLOCKED = false;
        _finishedTransitionAndAction = false;
    }
    #endregion

    #region UPDATE
    private void Update()
    {
        Run_updateLocally();
        EventsManagerLib.CALL_Hand_Broadcast(_updatedHandData.SR, _updatedHandData.SL, _updatedHandData.SolinoidState, _mySide);
    }

    #endregion

}

