using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandData
{


    float _svoR, _svoL;
    bool _solinoidState;

    // base will always be sr=60 and SL=120
    public HandData()
    {
        _svoR = 60f;
        _svoL = 120f;
        _solinoidState = false;
    }
    

    public float SR { get => _svoR;  set => _svoR = value; }
    public float SL { get => _svoL;  set => _svoL = value; }
    public bool SolinoidState { get => _solinoidState; set => _solinoidState = value; }
}






//    TOP view  
//              
//               SO
//     x                     x       
//     xx                   xx       
//      xx                 xx       
//     ┌─xx────┐     ┌────xx─┐       
//     │  xxx  │     │   xx  │       
//     │       │     │       │       
//     │       │     │       │       
//     │       │     │       │       
//     │       │     │       │       
//     │       │     │       │       
//     │       │     │       │       
//     └───────┘     └───────┘       
//                                   
//        SL            SR 