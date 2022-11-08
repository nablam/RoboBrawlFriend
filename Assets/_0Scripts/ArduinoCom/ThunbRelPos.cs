using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunbRelPos  
{
    // XY are based on horizontally placed servos.. so E is the X value  DR DL are affected by this Y value
    
    float _xFl, _yFl, _rFl;
    e_ButtonLocationType _positionType;

    public ThunbRelPos(float xFl, float yFl, float rFl, e_ButtonLocationType _argPositiontype)
    {
        XFl = xFl;
        YFl = yFl;
        RFl = rFl;
        _positionType = _argPositiontype;
    }

    public float XFl { get => _xFl; set => _xFl = value; }
    public float YFl { get => _yFl; set => _yFl = value; }
    public float RFl { get => _rFl; set => _rFl = value; }
    public e_ButtonLocationType PositionType { get => _positionType; set => _positionType = value; }
}
