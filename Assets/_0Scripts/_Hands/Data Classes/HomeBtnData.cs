using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBtnData
    
{
    // XY are based on horizontally placed servos.. so E is the X value  DR DL are affected by this Y value
    
    float _xFl, _yFl, _rFl; // THESE ARE WORLDBASEDVALUES
    e_ButtonLocationType _positionType;
    HandData _localPrecalculatedHAndData;
   // e_HandSide _handSide;
    public HomeBtnData(float xFl, float yFl, float rFl, e_ButtonLocationType argPositiontype, ServosKinematicSolver argModel )
    {

        //_handSide = arg_handSide;
        if (argPositiontype == e_ButtonLocationType._3_Center)
        {
           
            YFl_WorldBAsed = 0;
            RFl = rFl;
            _positionType = argPositiontype;
            _localPrecalculatedHAndData = new HandData();
            HandData temp = argModel.get_CalculatedCenterPos();
            XFl_WorldBased = argModel.get_CalculatedCenter_X();
            _localPrecalculatedHAndData.SR = temp.SR;
            _localPrecalculatedHAndData.SL = temp.SL;
            _localPrecalculatedHAndData.SolinoidState = false;

        }
        else

        {
            XFl_WorldBased = xFl;
            YFl_WorldBAsed = yFl;
            RFl = rFl;
            _positionType = argPositiontype;
            _localPrecalculatedHAndData = new HandData();
            HandData temp = argModel.Convert_XY_TO_SvoBiAngs(xFl, yFl);

            _localPrecalculatedHAndData.SR = temp.SR;
            _localPrecalculatedHAndData.SL = temp.SL;
            _localPrecalculatedHAndData.SolinoidState = false;


        }


      
    }

    public float XFl_WorldBased { get => _xFl; set => _xFl = value; }
    public float YFl_WorldBAsed { get => _yFl; set => _yFl = value; }
    public float RFl { get => _rFl; set => _rFl = value; }
    public e_ButtonLocationType PositionType { get => _positionType; set => _positionType = value; }

    public bool Validate_same_location(e_ButtonLocationType argPositiontype) {
        return (argPositiontype == _positionType);
    }

    public HandData Get_precalulatedSLSR() { return this._localPrecalculatedHAndData; }
}
