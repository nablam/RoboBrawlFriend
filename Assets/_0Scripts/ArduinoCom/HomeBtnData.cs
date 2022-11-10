using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBtnData
    
{
    // XY are based on horizontally placed servos.. so E is the X value  DR DL are affected by this Y value
    
    float _xFl, _yFl, _rFl;
    e_ButtonLocationType _positionType;
    HandData _localPrecalculatedHAndData;
    public HomeBtnData(float xFl, float yFl, float rFl, e_ButtonLocationType argPositiontype, ServosKinematicSolver argModel)
    {

        if (argPositiontype == e_ButtonLocationType._3_Center)
        {
           
            YFl = 0;
            RFl = rFl;
            _positionType = argPositiontype;
            _localPrecalculatedHAndData = new HandData();
            HandData temp = argModel.get_CalculatedCenterPos();
            XFl = argModel.get_CalculatedCenter_X();
            _localPrecalculatedHAndData.SR = temp.SR;
            _localPrecalculatedHAndData.SL = temp.SL;
            _localPrecalculatedHAndData.SolinoidState = false;

        }
        else

        {
            XFl = xFl;
            YFl = yFl;
            RFl = rFl;
            _positionType = argPositiontype;
            _localPrecalculatedHAndData = new HandData();
            HandData temp = argModel.Convert_XY_TO_SvoBiAngs(xFl, yFl);

            _localPrecalculatedHAndData.SR = temp.SR;
            _localPrecalculatedHAndData.SL = temp.SL;
            _localPrecalculatedHAndData.SolinoidState = false;


        }


      
    }

    public float XFl { get => _xFl; set => _xFl = value; }
    public float YFl { get => _yFl; set => _yFl = value; }
    public float RFl { get => _rFl; set => _rFl = value; }
    public e_ButtonLocationType PositionType { get => _positionType; set => _positionType = value; }

    public bool Validate_same_location(e_ButtonLocationType argPositiontype) {
        return (argPositiontype == _positionType);
    }

    public HandData Get_precalulatedSLSR() { return this._localPrecalculatedHAndData; }
}
