using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboHand  
{
   
   // bool _myThumbIsDown = false; //not touching
    ThunbRelPos _MainPos;
    SvosBiAng _MainPosBiang;
    ThunbRelPos _SecondaryPos;
    SvosBiAng _SecondaryPosBiang;
    ThunbRelPos _ThirdPos;
    SvosBiAng _ThirdPosBiang;
    ThunbRelPos _CenterPos; //is calculated by my svo model
    SvosBiAng _CenterPosBiang;

    ThunbRelPos[] _relPoses;
    SvosBiAng[] _biangs;
    int CUR_Index;
    e_HandSide _HandHANDSide;
    VectorToServo _mySvoModel;
    ThunbRelPos _CUR_SELECTED_POI;

    SvosBiAng _CUR_localBiang;

    e_HandState HandState;


    void PreCalculate_BiangPoses()
    {

      
        _MainPosBiang = new SvosBiAng();
        _SecondaryPosBiang = new SvosBiAng();
        _ThirdPosBiang = new SvosBiAng();
        _CenterPosBiang = new SvosBiAng();



        PopulateThisBiang(_MainPosBiang, _MainPos);
        PopulateThisBiang(_SecondaryPosBiang, _SecondaryPos);
        PopulateThisBiang(_ThirdPosBiang, _ThirdPos);
        PopulateThisBiang(_CenterPosBiang, _CenterPos);

        _relPoses = new ThunbRelPos[4] { _MainPos, _SecondaryPos, _ThirdPos, _CenterPos };
        _biangs = new SvosBiAng[4] { _MainPosBiang, _SecondaryPosBiang, _ThirdPosBiang, _CenterPosBiang };


    }

    void PopulateThisBiang(SvosBiAng argSvobiang, ThunbRelPos argRelPos ) {
        SvosBiAng temp= _mySvoModel.Convert_XY_TO_SvoBiAngs(argRelPos.XFl, argRelPos.YFl);
        argSvobiang.SL = temp.SL;
        argSvobiang.SR = temp.SR;
        argSvobiang.SolinoidState =false;
    }
    public RoboHand(e_HandSide argSide)
    {

        float RadiusToUse = 10f;
        _HandHANDSide = argSide;

        if (_HandHANDSide == e_HandSide.LEFT_hand) {
            _MainPos = new ThunbRelPos(50f, 0f, RadiusToUse, e_ButtonLocationType.Main);
            _SecondaryPos = new ThunbRelPos(40f, 0f, RadiusToUse, e_ButtonLocationType.SuperFire);
            _ThirdPos = new ThunbRelPos(36f, 0f, RadiusToUse, e_ButtonLocationType.GadgetFire);
        }
        else {
            _MainPos = new ThunbRelPos(50f, -18f, RadiusToUse, e_ButtonLocationType.Main);
            _SecondaryPos = new ThunbRelPos(50f, 18f, RadiusToUse, e_ButtonLocationType.SuperFire);
            _ThirdPos = new ThunbRelPos(65f, 0f, RadiusToUse, e_ButtonLocationType.GadgetFire);
        }

       // _myThumbIsDown = false;


        _mySvoModel = new VectorToServo(argSide);
        _CenterPos = _mySvoModel.get_CalculatedCenterPos();
        _CUR_SELECTED_POI = _CenterPos;
        CUR_Index = 3;
        PreCalculate_BiangPoses();
        HandState = e_HandState.Hovering;
        //_localBiang = new SvosBiAng();
    }

    public SvosBiAng Convert_XY_TO_SvoBiAngs(float arg_x_0_based_Axis, float arg_y) {

        return  _mySvoModel.Convert_XY_TO_SvoBiAngs(arg_x_0_based_Axis, arg_y);
    }

    public SvosBiAng Manage_Navigation_using_SelectedPOI(Vector3 argVEctor)
    {



        //if (!_myThumbIsDown) NowICanDrag = false;
        //else {
        //    NowICanDrag = true;
        //}
        //Vector3 DirFromSelectedPOI= new Vector3(argVEctor.normalized.x * _CUR_SELECTED_POI.RFl + _CUR_SELECTED_POI.XFl , argVEctor.normalized.y * _CUR_SELECTED_POI.RFl + _CUR_SELECTED_POI.YFl);
        //// if cant drag yet cuz thum is up from a recenter
        //if (NowICanDrag == false)
        //{
        //    _myThumbIsDown = true;
        //    RunTimer();
        //    _localBiang = _mySvoModel.Convert_XY_TO_SvoBiAngs(_CUR_SELECTED_POI.XFl, _CUR_SELECTED_POI.YFl);
        //    _localBiang.SolinoidState = _myThumbIsDown;
        //}
        //// set thumbs down 
        //// wait 
        //// set dragto  
        //else
        //{
        //    _localBiang.SolinoidState = true;
        //    _localBiang = _mySvoModel.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(DirFromSelectedPOI);

        //}


        if (RoutineWasStarted)
        {

            
            
            Set_localBiang_AccordingToStateTimerRoutine_orDynamic(argVEctor);
           


        }
        else {


            Vector3 DirFromSelectedPOI = new Vector3(argVEctor.normalized.x * _CUR_SELECTED_POI.RFl + _CUR_SELECTED_POI.XFl, argVEctor.normalized.y * _CUR_SELECTED_POI.RFl + _CUR_SELECTED_POI.YFl);
            _CUR_localBiang = _mySvoModel.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(DirFromSelectedPOI);
            _CUR_localBiang.SolinoidState = true;

        }
      

        


        return _CUR_localBiang;
    }
    public void Set_POI(e_ButtonLocationType arg_PoiType) {
        CUR_Index = (int)arg_PoiType;
        switch (arg_PoiType) {


            case e_ButtonLocationType.Main:
                _CUR_SELECTED_POI = _MainPos;
               // CUR_Index = 0;
                break;
             
            case e_ButtonLocationType.SuperFire:
                _CUR_SELECTED_POI = _SecondaryPos;
              //  CUR_Index = 1;

                break;
            case e_ButtonLocationType.GadgetFire:
                _CUR_SELECTED_POI = _ThirdPos;
               // CUR_Index = 2;
                break;
            case e_ButtonLocationType.Center:
                _CUR_SELECTED_POI = _CenterPos;
              //  CUR_Index = 3;
                break;
        }
    
    
    }


    void Set_localBiang_AccordingToStateTimerRoutine_orDynamic(Vector3 argVEctor)
    {

        RunStateChanger();


        switch (HandState)
        {


            case e_HandState.Centering:
                _CUR_localBiang = _biangs[CUR_Index];
                _CUR_localBiang.SolinoidState = false;
                // CUR_Index = 0;
                break;

            case e_HandState.Touching:
                _CUR_localBiang = _biangs[CUR_Index];
                _CUR_localBiang.SolinoidState = true;
                //  CUR_Index = 1;

                break;
            case e_HandState.Dragging:

                Vector3 DirFromSelectedPOI = new Vector3(argVEctor.normalized.x * _CUR_SELECTED_POI.RFl + _CUR_SELECTED_POI.XFl, argVEctor.normalized.y * _CUR_SELECTED_POI.RFl + _CUR_SELECTED_POI.YFl);
                _CUR_localBiang = _mySvoModel.Convert_Vector_fromCelectedpoint_andRadiusSvoBiAngs(DirFromSelectedPOI);                
                _CUR_localBiang.SolinoidState = true;
                // CUR_Index = 2;
                break;
            case e_HandState.Hovering:
                _CUR_localBiang = _biangs[CUR_Index]; //same as centering
                _CUR_localBiang.SolinoidState = false;
                //  CUR_Index = 3;
                break;
        }


    }
    //public SvosBiAng CursorToSelectdCenter_forResetThumbsup() {
    //   // _myThumbIsDown = false;
    //   // NowICanDrag = false;

    //    _CUR_localBiang= _mySvoModel.Convert_XY_TO_SvoBiAngs(_CUR_SELECTED_POI.XFl, _CUR_SELECTED_POI.YFl);
    //   // _CUR_localBiang.SolinoidState = _myThumbIsDown;
    //    return _CUR_localBiang;
    //}

    public void StartRoutineTurnKey_() {
        HandState = e_HandState.Centering;
        RoutineWasStarted = true;
    }



    bool RoutineWasStarted = false;


    void RunStateChanger() {
        RoutineWasStarted = true;
        if (HandState == e_HandState.Centering)
        {

            RunTimer_CEntering();
        }
        else
            if (HandState == e_HandState.Touching) {
            RunTimer_Touching();
        }
    }


  //  public float max_time = 2f;//  0.25f;
  //  public float time = 0.0f;
  //  uint time_mul = 0;
  ////  bool NowICanDrag;

  //  void RunTimer()
  //  {
  //      // Calculate time.
  //      // time = (Time.time - (max_time * time_mul));
  //      time += Time.deltaTime;
  //     // Debug.Log(time);
  //      // Time is up.
  //      if (time >= max_time)
  //      {
  //          // Do whatever:

  //         // NowICanDrag = true;
  //          // Reset time.
  //          time = 0.0f;
  //         // time_mul++;
  //      }
  //  }

    public float max_time_Centering = 2f;//  0.25f;
    public float time_Centering = 0.0f;
    void RunTimer_CEntering()
    {
        time_Centering += Time.deltaTime;
        // Debug.Log(time);
        // Time is up.
        if (time_Centering >= max_time_Centering)
        {
            // Do whatever:
            HandState = e_HandState.Touching;
            // NowICanDrag = true;
            // Reset time.
            max_time_Centering = 0.0f;
            // time_mul++;
        }
    }


    public float max_time_Touching = 2f;//  0.25f;
    public float time_Touching = 0.0f;
    void RunTimer_Touching()
    {
        time_Touching += Time.deltaTime;
        // Debug.Log(time);
        // Time is up.
        if (time_Touching >= max_time_Touching)
        {
            // Do whatever:
            HandState = e_HandState.Dragging;
            RoutineWasStarted = false;
            // NowICanDrag = true;
            // Reset time.
            time_Touching = 0.0f;
            // time_mul++;
        }
    }

    public float max_time_Dragging = 2f;//  0.25f;
    public float time_Dragging = 0.0f;
    void RunTimer_Dragging()
    {
        time_Dragging += Time.deltaTime;
        // Debug.Log(time);
        // Time is up.
        if (time_Dragging >= max_time_Dragging)
        {
            // Do whatever:

            // NowICanDrag = true;
            // Reset time.
            time_Dragging = 0.0f;
            // time_mul++;
        }
    }


    public float max_time_Hovering = 2f;//  0.25f;
    public float time_Hovering = 0.0f;
    void RunTimer_Hovering()
    {
        time_Hovering += Time.deltaTime;
        // Debug.Log(time);
        // Time is up.
        if (time_Hovering >= max_time_Hovering)
        {
            // Do whatever:

            // NowICanDrag = true;
            // Reset time.
            time_Hovering = 0.0f;
            // time_mul++;
        }
    }

    public float Get_AllTimers() { return max_time_Centering + max_time_Touching + max_time_Dragging; }
}
