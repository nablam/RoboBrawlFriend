using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMatPerspectivizer  
{

    public void InitiMe_IllUseAppSettings(int argFrameWidth, int argFrameHeight, e_BrawlMapType argMaptype);
    //the trapezoid form is the source , dst is the rectified ecet
    public MatOfPoint Get_Drawing_src_MatOfPoints();
    public MatOfPoint Get_Drawing_dst_MatOfPoints();

    public Mat Get_src_Mat();
    public Mat Get_dst_Mat();

}
