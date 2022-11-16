using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ObjdetectModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using OpenCVForUnityExample;
using Rect = OpenCVForUnity.CoreModule.Rect;
using Range = OpenCVForUnity.CoreModule.Range;

public class SkellyHaarDetector : MonoBehaviour
{
    CascadeClassifier cascade;
    protected static readonly string HAAR_CASCADE_FILENAME = "JackiDetect/haar_Shelly_20_2.xml";
    Mat _grayMat;
    MatOfRect playerShelies;
    public bool ON;
    public int runEvery = 1;
    public double ScalFactor = 1.1;
    public int MinNeighs = 3;
    public int findsizemin = 64;
    public int findsizemax = 96;
    public bool useDefault;
    public int CascadeFlag;
    int _flag;

    public void Dohaardetect(Mat argMat) {
        Imgproc.equalizeHist(argMat, _grayMat);
        if (cascade != null)
        {
            //CASCADE_DO_CANNY_PRUNING = 1;
            //CASCADE_SCALE_IMAGE = 2;
            //CASCADE_FIND_BIGGEST_OBJECT = 4;
            //CASCADE_DO_ROUGH_SEARCH = 8;
            if (CascadeFlag == 1 || CascadeFlag == 2 || CascadeFlag == 4 || CascadeFlag == 8){
                _flag = CascadeFlag;
            }else{
                _flag = 2;
            }


            if (useDefault)
            {
                cascade.detectMultiScale(_grayMat, playerShelies, ScalFactor, MinNeighs, _flag, new Size(findsizemin, findsizemin), new Size());
            }
            else
            {
                cascade.detectMultiScale(_grayMat, playerShelies, ScalFactor, MinNeighs, _flag, new Size(findsizemin, findsizemin), new Size(findsizemax, findsizemax));
            }
        }

        OpenCVForUnity.CoreModule.Rect[] rects = playerShelies.toArray();
        for (int i = 0; i < rects.Length; i++)
        {
            //Debug.Log ("detect playerShelies " + rects [i]);
            // rects[i].width  rects[i].height

             
            Imgproc.rectangle(argMat, new Point(rects[i].x, rects[i].y), new Point(rects[i].x + rects[i].width, rects[i].y + rects[i].height), new Scalar(255, 0, 0, 255), 2);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
