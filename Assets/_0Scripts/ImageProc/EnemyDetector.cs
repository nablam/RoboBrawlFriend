using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    #region Private_Vars
    int frameWidth, frameHeight;
    Mat rgbMat;
    Mat grayMat;
    Mat maskmat;
    List<Mat> UsedMats;
    const int TotalDetected = 8;
   EnemyData nmy_0, nmy_1, nmy_2, nmy_3, nmy_4, nmy_5, nmy_6, nmy_7;
    List<EnemyData> AllEnemieData;
    List<EnemyData> NMY_DATA_NUMTOTRACK;
   // List<EnemyData> Un_Registered;
   Point PlayerUnconfuse;
    Point e0, e1, e2, e3, e4, e5, e6, e7;
    Point[] NMIES;
   // Point[] CirclsDetected;
   // int FoundInFrame = 0;
    bool[] hasbeenLocated;
  //  bool detectedM_confusion = false;
    #endregion
    #region Public_Vars
    const float TILEside = 42.5f; 
     public float UnconfuseDistToplayer = 50;
     int NumEnemiesToTrack=4;
    public float maxDeviation = TILEside/2;
    public int MedoanBlur = 10;
    public double doctorPepper = 2;
    public double minimalDistance = 150;
    public double Parmisan1 = 152.28;
    public double Parmisan2 = 61.2;
    int minimalRAdius = 40;//45;
    int maximalRAdius = 45;//56;
    public int circlethicknessoffset = 2;
    List<Point> List_Clean_detected_Circles;
    int _cur_ActiveNodes = 0;
    int _cur_INActiveNodes = 0;

    EnemyData Theclosest_registeredNode;
    #endregion


    #region Enabled_Disable_Awake_Start_Destroy
    private void OnEnable()
    {
        EventsManagerLib.On_Player_Located += HeardPlayerLocalized;
    }
    private void OnDisable()
    {
        EventsManagerLib.On_Player_Located -= HeardPlayerLocalized;
    }

    private void Awake()
    {

    }
    void Start()
    {

    }
    private void OnDestroy()
    {
        disposeMyMatsAndTExtures();
    }
    #endregion

    #region EventHAndlers

    void HeardPlayerLocalized(double argVomitx, double argvomity)
    {    
        PlayerUnconfuse.x = argVomitx-5;
        PlayerUnconfuse.y = argvomity+210;// + argvomity/2;
    }
    void disposeMyMatsAndTExtures()
    {
        if (UsedMats != null)
        {
            foreach (Mat m in UsedMats)
            {
                if (m != null)
                    m.Dispose();
            }
        }
 
    }

    
    #endregion

    public void InitMe(int argFildWidth, int argFieldHeight)
    {
        frameHeight = argFieldHeight;
        frameWidth = argFildWidth;
        Debug.Log("HEARD field.width " + argFildWidth + " field.height " + argFieldHeight  );



        rgbMat = new Mat(frameHeight, frameWidth, CvType.CV_8UC3);
        grayMat = new Mat(frameHeight, frameWidth, CvType.CV_8SC1);
        maskmat = new Mat(frameHeight, frameWidth, CvType.CV_8U, Scalar.all(0));

        UsedMats = new List<Mat>();
        UsedMats.Add(rgbMat);
        UsedMats.Add(grayMat);
        UsedMats.Add(maskmat);
        e0= new Point(0, 0);  e1= new Point(0, 0);  e2= new Point(0, 0);  e3= new Point(0, 0);  e4= new Point(0, 0);  e5 = new Point(0, 0); e6 = new Point(0, 0); e7 = new Point(0, 0);
        NMIES = new Point[8] { e0, e1, e2, e3, e4, e5, e6, e7 };
        hasbeenLocated = new bool[8];
        PlayerUnconfuse = new Point(-10, -10);
       // CirclsDetected = new Point[8] { new Point(-3399, -10),new Point(-3399, -10),new Point(-3399, -10),new Point(-3399, -10),new Point(-3399, -10),new Point(-3399, -10),new Point(-3399, -10),new Point(-3399, -10)};
        nmy_0 = new EnemyData(3399 ,3399, 100, 0, new Scalar(0,0,0));//black
        nmy_1 = new EnemyData(3399, 3399, 100, 1,new Scalar(64, 64, 64));//dark
        nmy_2 = new EnemyData(3399, 3399, 100,2, new Scalar(160, 160, 160));//light
        nmy_3 = new EnemyData(3399, 3399, 100,3, new Scalar(255, 255, 255));//white

        nmy_4 = new EnemyData(3399, 3399, 100,4, new Scalar(255, 255, 0));//yellow
        nmy_5 = new EnemyData(3399, 3399, 100,5, new Scalar(255, 192, 0));//orange    
        nmy_6 = new EnemyData(3399, 3399, 100,6, new Scalar(244, 115,120));//pink
        nmy_7 = new EnemyData(3399, 3399, 100,7, new Scalar(112, 46, 160));//purple

         
        AllEnemieData = new List<EnemyData>() { nmy_0 , nmy_1, nmy_2, nmy_3, nmy_4, nmy_5, nmy_6, nmy_7 };

        NMY_DATA_NUMTOTRACK = new List<EnemyData>();

        for (int x = 0; x < NumEnemiesToTrack; x++) {
            NMY_DATA_NUMTOTRACK.Add(AllEnemieData[x]);
        }
        Debug.Log(NMY_DATA_NUMTOTRACK.Count);
        List_Clean_detected_Circles = new List<Point>();
    }

    //void ClearDetected() {
    //    for (int i = 0; i < TotalDetected; i++)
    //    {

    //        CirclsDetected[i].x = 0; CirclsDetected[i].y = 0;
    //    }

    //}

    void ClearLocated()
    {
    
        for (int i = 0; i < TotalDetected; i++)
        {

            hasbeenLocated[i] = false;
        }
    }

    // Update is called once per frame
    public void UpdateFindEnemyCircles(Mat argrgbaMat, bool argDoDraw)
    {
        if (NumEnemiesToTrack > 4) NumEnemiesToTrack = 4;
        if (NumEnemiesToTrack < 1) NumEnemiesToTrack = 1;
        if (maxDeviation > 20f) maxDeviation = 20f;
        if (maxDeviation < 0.01f) maxDeviation = 0.01f;
        if (UnconfuseDistToplayer < 0.01f) UnconfuseDistToplayer = 0.01f;
        if (UnconfuseDistToplayer > 2000f) UnconfuseDistToplayer = 2000f;
        
        if (PlayerUnconfuse.x < 0) return; //notreceived playerloc


         Imgproc.circle(argrgbaMat, PlayerUnconfuse, (int)UnconfuseDistToplayer, new Scalar(0, 100, 100, 255), 8);

   

      
        Imgproc.cvtColor(argrgbaMat, grayMat, Imgproc.COLOR_RGBA2GRAY);
      //  Imgproc.medianBlur(grayMat, grayMat, MedoanBlur);
        using (Mat circles = new Mat())
        {
            Imgproc.HoughCircles(grayMat, circles, Imgproc.CV_HOUGH_GRADIENT, doctorPepper, minimalDistance, Parmisan1, Parmisan2, minimalRAdius, maximalRAdius);
            Point pt = new Point();
            int numberOfCirclesFonud = circles.cols();
          
            List_Clean_detected_Circles.Clear();
          
           // Debug.Log(numberOfCirclesFonud);
            for (int i = 0; i < circles.cols(); i++)
            {

              
                    double[] data = circles.get(0, i);
                    pt.x = data[0];
                    pt.y = data[1];
                if ( IS_VALID_Point(pt) )
                {
                    //  CirclsDetected[i].x = pt.x;
                    //  CirclsDetected[i].y = pt.y;
                    double rho = data[2];
                    List_Clean_detected_Circles.Add(pt);
                    if (argDoDraw)
                        Imgproc.circle(argrgbaMat, pt, (int)rho, new Scalar(255, 0, 0, 255), 5);
                    EventsManagerLib.CALL_SingleCirle_Detected_evnt(pt.x, pt.y);
                }
                else {
                    print("too close");
                }
             
            }
        }

       
        int Num_registered = 0;
        int Num_Un_registered = 0;
        for (int x = 0; x < NMY_DATA_NUMTOTRACK.Count; x++) {
            if (NMY_DATA_NUMTOTRACK[x].Get_Registered() == true)
            {
                Num_registered++;
            }
            else {
                Num_Un_registered++;
            }
        }

   
        string deb1 = " regs " + Num_registered;
        EventsManagerLib.CALL_debug1(deb1);


        string deb2 = " un " + Num_Un_registered;
        EventsManagerLib.CALL_debug2(deb2);


        string deb3 = " circs " + List_Clean_detected_Circles.Count;
        EventsManagerLib.CALL_debug3(deb3);


        _cur_ActiveNodes = Num_registered;
        _cur_INActiveNodes = Num_Un_registered;

        //  if (Input.GetKeyDown(KeyCode.I))
        //  {
        //      nmy_0.UpdateData(50, 50);
        //  }
        //  else
        //          if (Input.GetKeyDown(KeyCode.O))
        //  {

        //  }
        //  else
        //          if (Input.GetKeyDown(KeyCode.L))
        //  {

        //  }
        //  else
        //if (Input.GetKeyDown(KeyCode.K))
        //  {

        //  }







        // if (List_Clean_detected_Circles.Count > NumEnemiesToTrack || List_Clean_detected_Circles.Count==0) return;




        // register new or update existing

        for (int f = 0; f < List_Clean_detected_Circles.Count; f++) {

          
             Point pt=  List_Clean_detected_Circles[f];
            if (IS_VALID_Point(pt))
            {
                if (_cur_ActiveNodes == 0)
                {
                    EnemyData New_node = FindFirst_Unregistered();
                    RegisterNode(pt, New_node);

                }

                else
                {

                    int indexCloseestRegiterd = 999;
                    double _nearestDistance = 99999;

                    for (int c = 0; c < NMY_DATA_NUMTOTRACK.Count; c++)
                    {

                        if (NMY_DATA_NUMTOTRACK[c].Get_Registered() == true)
                        {
                            double distTome = DistBetweenTwoPonta(pt, NMY_DATA_NUMTOTRACK[c].GetLoc());
                            if (distTome < _nearestDistance)
                            {
                                _nearestDistance = distTome;
                                Theclosest_registeredNode = NMY_DATA_NUMTOTRACK[c];
                                indexCloseestRegiterd = c;
                            }


                        }
                    }

                    if (_nearestDistance > 120)
                    {

                        EnemyData New_node = FindFirst_Unregistered();
                        RegisterNode(pt, New_node);
                    }
                    else
                    {

                        Theclosest_registeredNode.UpdateData(pt.x, pt.y);
                    }



                }


            }



         


        }

        for (int c = 0; c < NMY_DATA_NUMTOTRACK.Count; c++)
        {

            if (NMY_DATA_NUMTOTRACK[c].Get_Registered() == true)
            {
                if (NMY_DATA_NUMTOTRACK[c].probeExpiration() > 5) {

                    Un_RegisterNode(NMY_DATA_NUMTOTRACK[c]);


                }
                
                


            }
        }



        if (Num_registered > 0)
        {

            foreach (EnemyData ed in NMY_DATA_NUMTOTRACK)
            {

                if(ed.Get_Registered()==true)
                Imgproc.circle(argrgbaMat, ed.GetLoc(), 50, ed.GetColor(), 14);

            }
        }

        //Imgproc.putText (rgbaMat, "W:" + rgbaMat.width () + " H:" + rgbaMat.height () + " SO:" + Screen.orientation, new Point (5, rgbaMat.rows () - 10), Imgproc.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar (255, 255, 255, 255), 2, Imgproc.LINE_AA, false);


    }

    EnemyData FindFirst_Unregistered() {
        int indexOfFirstavailable_Unregistered = 0;
        for (int n = 0; n<  NMY_DATA_NUMTOTRACK.Count; n++) {
            int reversindex = NMY_DATA_NUMTOTRACK.Count - 1 - n;
            if (NMY_DATA_NUMTOTRACK[reversindex].Get_Registered() == false) {
                indexOfFirstavailable_Unregistered = reversindex;
            }
        
        }

        return NMY_DATA_NUMTOTRACK[indexOfFirstavailable_Unregistered];
    }

    void RegisterNode(Point argPt, EnemyData argEnemydata) {

        argEnemydata.UpdateData(argPt.x, argPt.y);
        _cur_ActiveNodes++; _cur_INActiveNodes--;
    }

    void Un_RegisterNode( EnemyData argEnemydata)
    {

        argEnemydata.Set_un_Registered();
        _cur_ActiveNodes--; _cur_INActiveNodes++;
    }



    double DistBetweenTwoPonta(Point argFrom, Point argTo) {

        double x1 = argFrom.x;
        double y1 = argFrom.y;

        double x2 = argTo.x;
        double y2 = argTo.y;

        double tempMinDist = Math.Abs( Math.Sqrt((y2 - y1) * (y2 - y1) + (x2 - x1) * (x2 - x1)));
        return tempMinDist;

    }

    bool IS_VALID_Point(Point argpt) {
        bool tert = false;
        bool is_inYrange = false;
        bool isInXrange = false;
        if (argpt.x > PlayerUnconfuse.x - UnconfuseDistToplayer && argpt.x < PlayerUnconfuse.x + UnconfuseDistToplayer) {
            isInXrange = true;
        }

        if (isInXrange) {
            if (argpt.y > PlayerUnconfuse.y - UnconfuseDistToplayer && argpt.y < PlayerUnconfuse.y + UnconfuseDistToplayer)
            {
                is_inYrange = true;
            }

        }

        if (isInXrange && is_inYrange) {
            tert = true;
        }

        return !tert;

    
    }

}
