using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rect = OpenCVForUnity.CoreModule.Rect;

public class MatsOfROICoordinates : MonoBehaviour
{


    #region Private_Vars
    //
    List<Point> _listHorizon_andVertiPoints;
    MatOfPoint _Horizon_and_vertical;

    List<Point> _PerspPoints_lst;
    MatOfPoint _Perspective_MOP;

    List<Point> _Squared_Biv_Points_lst;
    MatOfPoint _squared_Biv_MOP;

    //Mat src_trapezoid_Mat;
    //Mat dst_BirdsEyeView_Mat;
    Mat src_trapezoid_Mat = new Mat(4, 1, CvType.CV_32FC2);
    Mat dst_BirdsEyeView_Mat = new Mat(4, 1, CvType.CV_32FC2);
    MatOfPoint2f src2f, dst2f;

    //***********************************************************************************

    MatOfRect _AllRects;
    List<Rect> _list_AllRects;

     Point o_ptl;
     Point o_ptr;
     Point o_pbr;
     Point o_pbl;


     Point correct_ptl;
     Point correct_ptr;
     Point correct_pbr;
     Point correct_pbl;
    Mat srcRectMat;
    #endregion

    #region Enabled_Disable_Awake_Start_Destroy 
    //
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    private void OnDestroy()
    {
        CleanupResources();
    }
    private void Awake()
    {
        Utils.setDebugMode(true, false);
    }

    void Start()
    {

    }
    #endregion

    #region EventHAndlers 
    //
    #endregion

    #region CanRunMutex
    //
    #endregion

    #region Cleanup
    //
    void CleanupResources() {

        //clean matofpoints
        if (_Horizon_and_vertical != null)
            _Horizon_and_vertical.Dispose();

        if (_AllRects != null)
            _AllRects.Dispose();
        if (_Perspective_MOP != null)
            _Perspective_MOP.Dispose();
        if (_squared_Biv_MOP != null)
            _squared_Biv_MOP.Dispose();


        //clean lists
        if (_list_AllRects != null) {
            _list_AllRects.Clear();
            _list_AllRects = null;
        }

        if (_listHorizon_andVertiPoints != null)
        {
            _listHorizon_andVertiPoints.Clear();
            _listHorizon_andVertiPoints = null;
        }
        if (_PerspPoints_lst != null)
        {
            _PerspPoints_lst.Clear();
            _PerspPoints_lst = null;
        }
        if (_Squared_Biv_Points_lst != null)
        {
            _Squared_Biv_Points_lst.Clear();
            _Squared_Biv_Points_lst = null;
        }
        // 
        //cleanMats

        if (src_trapezoid_Mat != null)
            src_trapezoid_Mat.Dispose();

        if (dst_BirdsEyeView_Mat != null)
            dst_BirdsEyeView_Mat.Dispose();




    }
    #endregion

    #region PrivatMethods 
    //
    void DebugVars() { }

    double[] Make_tltr_brBl_xy_array(List<Point> arg_List_points) {

        double[] output = new double[8];
        for (int x = 0; x < 4; x++) {

            output[x * 2] = arg_List_points[x].x;
            output[x * 2 +1] = arg_List_points[x].y;
        }
        return output;
    }
    void Fill_Perspective_Mats(List<Point> arg_trap_points, List<Point> arg_corected_points) {

        src_trapezoid_Mat.put(0, 0, Make_tltr_brBl_xy_array(arg_trap_points));
        dst_BirdsEyeView_Mat.put(0, 0, Make_tltr_brBl_xy_array(arg_corected_points));

    }
    #endregion

    #region PUBLIC_Methods 
    //
    public void InitiMe_IllUseAppSettings(int argFrame_w, int argFrame_h)
    {

        Debug.Log("coordinator inited at " + argFrame_w + " " + argFrame_h);
        //*********************************************************************************** mid lines on frame received                       
        _listHorizon_andVertiPoints = new List<Point>();
        // first 2 points = horizontal
        _listHorizon_andVertiPoints.Add(new Point(0, argFrame_h / 2));
        _listHorizon_andVertiPoints.Add(new Point(argFrame_w, argFrame_h / 2));
        // next 2 points = vertical
        _listHorizon_andVertiPoints.Add(new Point(argFrame_w/2,0));
        _listHorizon_andVertiPoints.Add(new Point(argFrame_w / 2, argFrame_h));

        _Horizon_and_vertical = new MatOfPoint(_listHorizon_andVertiPoints.ToArray());
        //***********************************************************************************  


        float Source_width = AppSettings.Instance.Get_NativeDevice_player().Widh_px; //saw 2300
        float Source_hight = AppSettings.Instance.Get_NativeDevice_player().Height_px; // saw 1080
        float received_frameWidth = argFrame_w; //saw 1280
        float received_frameHeight = argFrame_h; // saw 720
        float SourceToOutput_ratio = received_frameWidth/ Source_width; //saw .5565217

        float Calculated_game_W = received_frameWidth;//simple 1280
        float Calculated_game_h = 600f; //Mathf.RoundToInt(  SourceToOutput_ratio* Source_hight); //SAW  601


        float Calculated_Margin_Top =  (received_frameHeight - Calculated_game_h) / 2; //saw 60
        float Calculated_Margin_Bot = (received_frameHeight - Calculated_Margin_Top - Calculated_game_h); //saw59

        //Debug.Log("1280 / 2300 = .5565217 " );
        //Debug.Log(received_frameWidth+ " / " + Source_width + "=" + SourceToOutput_ratio);
        //Debug.Log("  .5565217 * 1080 = 601.04");
        //Debug.Log(SourceToOutput_ratio + " * " + Source_hight + "=" + Calculated_game_h);
        //Debug.Log("  the game screen view is 1280 by 600 ish ");
        //Debug.Log("  vert margin top: " + Calculated_Margin_Top);
        //int sanityVertical = Calculated_Margin_Top + Calculated_game_h + Calculated_Margin_Bot;
        //Debug.Log(Calculated_Margin_Top + " + " + Calculated_game_h + " + " + Calculated_Margin_Bot + " = " + sanityVertical);
        //Debug.Log("  sanity : " + received_frameHeight + " should = "+ sanityVertical);
        //***********************************************************************************++

        //Top perspective points are 200 off Edges of view


       // int WANTED_Field_W = 940;// makes 40 px tiles *21
        float Up_HORIZON_fromTop = Calculated_Margin_Top;//60
        float Dw_HORIZON_fromTop = Calculated_Margin_Top + Calculated_game_h;// 60+ 601

        float topPerspOffset = 200;
        float lowPerspOffset = topPerspOffset / 2;


        float lf_VERTICAL_fromleft = (topPerspOffset + lowPerspOffset) / 2;
        float rg_VERTICAL_fromleft = Calculated_game_W- lf_VERTICAL_fromleft;
        Debug.Log(rg_VERTICAL_fromleft);

         o_ptl=new Point(topPerspOffset, Up_HORIZON_fromTop);
         o_ptr = new Point(Calculated_game_W- topPerspOffset, Up_HORIZON_fromTop);
         o_pbr = new Point(Calculated_game_W- lowPerspOffset, Dw_HORIZON_fromTop);
         o_pbl = new Point(lowPerspOffset, Dw_HORIZON_fromTop);

        _PerspPoints_lst = new List<Point>();

        _PerspPoints_lst.Add(o_ptl);
        _PerspPoints_lst.Add(o_ptr);
        _PerspPoints_lst.Add(o_pbr);
        _PerspPoints_lst.Add(o_pbl);

        _Perspective_MOP = new MatOfPoint(_PerspPoints_lst.ToArray());





         correct_ptl = new Point(lf_VERTICAL_fromleft, Up_HORIZON_fromTop);
         correct_ptr = new Point(rg_VERTICAL_fromleft, Up_HORIZON_fromTop);
        correct_pbr = new Point(rg_VERTICAL_fromleft, Dw_HORIZON_fromTop);
         correct_pbl = new Point(lf_VERTICAL_fromleft, Dw_HORIZON_fromTop);
        _Squared_Biv_Points_lst = new List<Point>();

        _Squared_Biv_Points_lst.Add(correct_ptl);
        _Squared_Biv_Points_lst.Add(correct_ptr);
        _Squared_Biv_Points_lst.Add(correct_pbr);
        _Squared_Biv_Points_lst.Add(correct_pbl);

        _squared_Biv_MOP = new MatOfPoint(_Squared_Biv_Points_lst.ToArray());

        src2f = new MatOfPoint2f(_PerspPoints_lst.ToArray());
        dst2f = new MatOfPoint2f(_Squared_Biv_Points_lst.ToArray());



        //src_trapezoid_Mat.put(0, 0, Make_tltr_brBl_xy_array(arg_trap_points));
        //dst_BirdsEyeView_Mat.put(0, 0, Make_tltr_brBl_xy_array(arg_corected_points));
        src_trapezoid_Mat.put(0, 0, o_ptl.x, o_ptl.y, o_ptr.x, o_ptr.y, o_pbr.x, o_pbr.y, o_pbl.x, o_pbl.y);
        //src_trapezoid_Mat.put(0, 0,1, 2, 3, 4, 5, 6,7,8);
        dst_BirdsEyeView_Mat.put(0, 0, correct_ptl.x, correct_ptl.y, correct_ptr.x, correct_ptr.y, correct_pbr.x, correct_pbr.y, correct_pbl.x, correct_pbl.y);
      

        // Fill_Perspective_Mats(_PerspPoints_lst, _Squared_Biv_Points_lst);



        _list_AllRects = new List<Rect>();
        //rect Lefj Joy



        _AllRects = new MatOfRect();

    }

    public MatOfPoint Get_HorizonPoints_andVErticals() {
        return this._Horizon_and_vertical;
    }

    public MatOfPoint Get_TrapezoidPoints()
    {
        return this._Perspective_MOP;
    }

    public MatOfPoint Get_squarePoints()
    {
        return this._squared_Biv_MOP;
    }

    public MatOfPoint2f Get_Source_Trapezoid() { return this.src2f; }
    public MatOfPoint2f Get_Source_Rectified() { return this.dst2f; }
    #endregion


    #region UpdateMethod
    void Update()
    {

    }

    #endregion
  

     

}
