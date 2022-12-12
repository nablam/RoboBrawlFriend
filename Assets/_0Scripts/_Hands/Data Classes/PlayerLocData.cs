using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocData  
{
    Point InView_Point;
    Vector2 InView_pos_V2;
    Point InMap_Point;
    Vector2 InMap_pos_V2;
    Scalar Mycolor;

    int _local_width_OfDetectionframe;
    int _local_height_OfDetectionframe;

    public PlayerLocData(double screenLoc_X, double screenLoc_Y, Scalar argColor, int argdetectionFrame_width, int arg_detectionFrameHeight)
    {

        _local_width_OfDetectionframe = argdetectionFrame_width;
        _local_height_OfDetectionframe = arg_detectionFrameHeight;
        Mycolor = argColor;    
        InView_Point = new Point(screenLoc_X, screenLoc_Y);
        InMap_Point = new Point(0, 0);
        InView_pos_V2 = new Vector2((float)screenLoc_X, (float)screenLoc_Y);
        InMap_pos_V2 = new Vector2(0, 0);

    }

    public void Update_InViewPoint(double argInviewX, double arInviewY, double arg_inMap_X , double arg_inmap_Y) {

        //        0             -> playerareawidth
        //         ______________
        //        |
        //        |  
        //        |______________
        //
        InView_Point.x = argInviewX;
        InView_Point.y = arInviewY;

        InMap_Point.x = arg_inMap_X;
        InMap_Point.y = arg_inmap_Y;

        InView_pos_V2.x = (float)argInviewX;
        InView_pos_V2.y = (float)arInviewY;
        InMap_pos_V2.x = (float)arg_inMap_X;
        InMap_pos_V2.y = (float)arg_inmap_Y;

    }

    public Point Get_inMapPoint() {
        return this.InMap_Point;
    }

    public Vector2 Get_inMap_Vector2()
    {
        return this.InMap_pos_V2;
    }


}
