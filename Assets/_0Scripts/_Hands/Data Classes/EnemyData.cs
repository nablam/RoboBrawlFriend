using OpenCVForUnity.CoreModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData  
{
    Point InView_Point;
    double ScreenLoc_X;
    double ScreenLoc_Y;
    float Confidence;
    float TimeStamp_Created;
    float TimeStamp_LocatedAt;
    int MilisSincLast;
    Scalar Mycolor;
    int _id;
    bool isRegistered;
    double DistToPlayer;
    int ThreatLevel = 0; //maxdanger
    int _local_width_OfDetectionframe;
    int _local_height_OfDetectionframe;

    Vector2 InView_pos_V2;
    Point InMap_Point;
    Vector2 InMap_pos_V2;

    public EnemyData(double screenLoc_X, double screenLoc_Y, float confidence,int argId, Scalar argColor, int argdetectionFrame_width, int arg_detectionFrameHeight)
    {

        _id = argId;
        ScreenLoc_X = screenLoc_X;
        ScreenLoc_Y = screenLoc_Y;
        _local_width_OfDetectionframe = argdetectionFrame_width;
        _local_height_OfDetectionframe = arg_detectionFrameHeight;

        Mycolor = argColor;
        Confidence = confidence;
        TimeStamp_Created = Time.time;
        TimeStamp_LocatedAt = Time.time;
        MilisSincLast = 0;
        InView_Point = new Point(screenLoc_X, screenLoc_Y);
        InMap_Point = new Point(0, 0);
        InView_pos_V2 = new Vector2((float)screenLoc_X, (float)screenLoc_Y);
        InMap_pos_V2 = new Vector2(0,0);

    }

    public void UpdateData(double arg_screenLoc_X, double arg_screenLoc_Y) {

        InView_pos_V2.x = (float)arg_screenLoc_X;
        InView_pos_V2.y = (float)arg_screenLoc_Y;
        isRegistered = true;
        float Temp_now = Time.time;
         
        float TimeSinceLast = Temp_now - TimeStamp_LocatedAt;
        TimeStamp_LocatedAt = Temp_now;
        InView_Point.x = arg_screenLoc_X;
        InView_Point.y = arg_screenLoc_Y;

        if (TimeSinceLast > 3) Confidence = 0f;
        else {

           Confidence= 100- ((TimeSinceLast / 2) * 100f);

        }

       // Debug.Log(TimeSinceLast + "  " + Confidence);
    }

    public Point GetLoc_point_inView() { 
        return this.InView_Point;
    }


    public Point GetLoc_point_inMap()
    {
        return this.InMap_Point;
    }

    public Vector2 GetLocV2_inView()
    {
        return this.InView_pos_V2;
    }

    public Vector2 GetLocV2_inMap()
    {
        return this.InMap_pos_V2;
    }
    public void SetLocV2_inMap(double arg_mapLocalx, double arg_mapLocaly)
    {

        

        this.InMap_pos_V2.x = (float)arg_mapLocalx;
        this.InMap_pos_V2.y = (float)arg_mapLocaly;
        this.InMap_Point.x = arg_mapLocalx;
        this.InMap_Point.y = arg_mapLocaly;
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
        InView_Point.x = ScreenLoc_X;
        InView_Point.y = ScreenLoc_Y;
        isRegistered = false;

    }

    public bool Get_Registered() {return isRegistered  ; }
    public void Set_un_Registered() { RestMe(); }

    public float GetConfidance() { return this.Confidence; }

    public void SetDistToplayer(double argDistToPlayer) { DistToPlayer = argDistToPlayer; }

    public double GetDistToPlayer() { return this.DistToPlayer; }

    public void Set_threat(int argThreat) { ThreatLevel = argThreat; }
    public int Get_threatLevel() { return this.ThreatLevel; }

    public int Get_FrameWidth() { return this._local_width_OfDetectionframe; }

    public int Get_FrameHeight() { return this._local_height_OfDetectionframe; }
}
