using OpenCVForUnity.CoreModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData  
{
    Point ScreenPointLoc;
    double ScreenLoc_X;
    double ScreenLoc_Y;
    float Confidence;
    float TimeStamp_Created;
    float TimeStamp_LocatedAt;
    int MilisSincLast;
    Scalar Mycolor;
    int _id;
    bool isRegistered;

    public EnemyData(double screenLoc_X, double screenLoc_Y, float confidence,int argId, Scalar argColor)
    {
        _id = argId;
        ScreenLoc_X = screenLoc_X;
        ScreenLoc_Y = screenLoc_Y;
        Mycolor = argColor;
        Confidence = confidence;
        TimeStamp_Created = Time.time;
        TimeStamp_LocatedAt = Time.time;
        MilisSincLast = 0;
        ScreenPointLoc = new Point(screenLoc_X, screenLoc_Y);
    }

    public void UpdateData(double arg_screenLoc_X, double arg_screenLoc_Y) {
        isRegistered = true;
        float Temp_now = Time.time;
         
        float TimeSinceLast = Temp_now - TimeStamp_LocatedAt;
        TimeStamp_LocatedAt = Temp_now;
        ScreenPointLoc.x = arg_screenLoc_X;
        ScreenPointLoc.y = arg_screenLoc_Y;

        if (TimeSinceLast > 3) Confidence = 0f;
        else {

           Confidence= 100- ((TimeSinceLast / 3) * 100f);

        }

       // Debug.Log(TimeSinceLast + "  " + Confidence);
    }

    public Point GetLoc() { 
        return this.ScreenPointLoc;
    }

    public Scalar GetColor()
    {
        return this.Mycolor;
    }

    public int GetID()
    {
        return this._id;
    }

    public float probeExpiration() {

        float Temp_now = Time.time;


        return (Temp_now - TimeStamp_LocatedAt);
    }

      void RestMe() {

      
        ScreenLoc_X = 99999;
        ScreenLoc_Y = 99999;
         
        Confidence = 100;
        TimeStamp_Created = Time.time;
        TimeStamp_LocatedAt = Time.time;
        MilisSincLast = 0;
        ScreenPointLoc.x = ScreenLoc_X;
        ScreenPointLoc.y = ScreenLoc_Y;
        isRegistered = false;

    }

    public bool Get_Registered() {return isRegistered  ; }
    public void Set_un_Registered() { RestMe(); }
}
