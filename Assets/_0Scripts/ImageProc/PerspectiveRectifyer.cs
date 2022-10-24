//#define UseHArdCoded
using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rect = OpenCVForUnity.CoreModule.Rect;

public class PerspectiveRectifyer : MonoBehaviour, IMatPerspectivizer
{

    
    #region Private_Vars
    private Point o_ptl;
    private Point o_ptr;
    private Point o_pbr;
    private Point o_pbl;


    private Point correct_ptl;
    private Point correct_ptr;
    private Point correct_pbr;
    private Point correct_pbl;

    private Mat srcRectMat;
    private Mat dstRectMat;
    private List<Mat> UsedMats;
    private List<Point> o_ptsLIST, correct_ptsLIST, _listHorizon_andVertiPoints;
    private MatOfPoint o_pts_MOP, correct_pts_MOP, _Horizon_and_vertical_MOP;
    private Rect GameView_Rect;
    private Rect Fild_Rect;
    private Rect TrakVert_Rect;
    private Rect PlayerAres_Rect;

    private List<Point> ListOfAllVerticalGridPoints;
    private List<Point> ListOfAllHorizontalGridPoints;

    private List<MatOfPoint> vertical_Lines;
    private List<MatOfPoint> horizontal_Lines;
    #endregion
     
    #region Public_Methods
    public void InitiMe_IllUseAppSettings(int argFrameWidth, int argFrameHeight, e_BrawlMapType argMaptype)
    {



        //*********************************************************************************** mid lines on frame received                       
        _listHorizon_andVertiPoints = new List<Point>();
        // first 2 points = horizontal
        _listHorizon_andVertiPoints.Add(new Point(0, argFrameHeight / 2));
        _listHorizon_andVertiPoints.Add(new Point(argFrameWidth, argFrameHeight / 2));
        // next 2 points = vertical
        _listHorizon_andVertiPoints.Add(new Point(argFrameWidth / 2, 0));
        _listHorizon_andVertiPoints.Add(new Point(argFrameWidth / 2, argFrameHeight));

        _Horizon_and_vertical_MOP = new MatOfPoint(_listHorizon_andVertiPoints.ToArray());
        //***********************************************************************************  

#if UseHArdCoded
        o_ptl = new Point(200, 60); /*|*/ o_ptr = new Point(1080, 60);
        /*------------------------------------------------------------*/
        o_pbl = new Point(100, 660);/*|*/ o_pbr = new Point(1180, 660);


        correct_ptl = new Point(150, 60); /*|*/ correct_ptr = new Point(1130, 60);
        /*------------------------------------------------------------*/
        correct_pbl = new Point(150, 660);/*|*/ correct_pbr = new Point(1130, 660);
#else

        float Source_width = AppSettings.Instance.Get_NativeDevice_player().Widh_px; //saw 2300
        float Source_hight = AppSettings.Instance.Get_NativeDevice_player().Height_px; // saw 1080
        float received_frameWidth = argFrameWidth; //saw 1280
        float received_frameHeight = argFrameHeight; // saw 720
        float SourceToOutput_ratio = received_frameWidth / Source_width; //saw .5565217

        float Calculated_game_W = received_frameWidth;//simple 1280
        float Calculated_game_h = 600f; //Mathf.RoundToInt(  SourceToOutput_ratio* Source_hight); //SAW  601


        float Calculated_Margin_Top = (received_frameHeight - Calculated_game_h) / 2; //saw 60
        float Calculated_Margin_Bot = (received_frameHeight - Calculated_Margin_Top - Calculated_game_h); //saw59


        float Up_HORIZON_fromTop = Calculated_Margin_Top;//60
        float Dw_HORIZON_fromTop = Calculated_Margin_Top + Calculated_game_h;// 60+ 601

        float ARBITRARY_topPerspOffset = 200;
        float lowPerspOffset = ARBITRARY_topPerspOffset / 2;


        float lf_VERTICAL_fromleft = (ARBITRARY_topPerspOffset + lowPerspOffset) / 2;
        float rg_VERTICAL_fromleft = Calculated_game_W - lf_VERTICAL_fromleft;
         

        o_ptl = new Point(ARBITRARY_topPerspOffset, Up_HORIZON_fromTop);
        o_ptr = new Point(Calculated_game_W - ARBITRARY_topPerspOffset, Up_HORIZON_fromTop);
        o_pbr = new Point(Calculated_game_W - lowPerspOffset, Dw_HORIZON_fromTop);
        o_pbl = new Point(lowPerspOffset, Dw_HORIZON_fromTop);

        correct_ptl = new Point(lf_VERTICAL_fromleft, Up_HORIZON_fromTop);
        correct_ptr = new Point(rg_VERTICAL_fromleft, Up_HORIZON_fromTop);
        correct_pbr = new Point(rg_VERTICAL_fromleft, Dw_HORIZON_fromTop);
        correct_pbl = new Point(lf_VERTICAL_fromleft, Dw_HORIZON_fromTop);

#endif
        int GameViewExtraWidth = 100;
        int theLitttleExtra_fromLeft = Mathf.RoundToInt(lf_VERTICAL_fromleft - GameViewExtraWidth);
        int GameView_X =  theLitttleExtra_fromLeft;
        int GameView_Y = Mathf.RoundToInt(Up_HORIZON_fromTop);
        int GameView_Width = Mathf.RoundToInt(rg_VERTICAL_fromleft + theLitttleExtra_fromLeft);
        int GameView_Height = Mathf.RoundToInt(Dw_HORIZON_fromTop - Up_HORIZON_fromTop);

        GameView_Rect = new Rect(GameView_X, GameView_Y, GameView_Width, GameView_Height);



        o_ptsLIST = new List<Point>() { o_ptl, o_ptr, o_pbr, o_pbl };
        correct_ptsLIST = new List<Point>() { correct_ptl, correct_ptr, correct_pbr, correct_pbl };

        o_pts_MOP = new MatOfPoint(o_ptsLIST.ToArray());
        correct_pts_MOP = new MatOfPoint(correct_ptsLIST.ToArray());

        UsedMats = new List<Mat>();
        srcRectMat = new Mat(4, 1, CvType.CV_32FC2);
        dstRectMat = new Mat(4, 1, CvType.CV_32FC2);

        UsedMats.Add(srcRectMat);
        UsedMats.Add(dstRectMat);

        vertical_Lines = new List<MatOfPoint>();
        horizontal_Lines = new List<MatOfPoint>();

        float ARBITRARY_Left_VertGris_X = 200;
        float HalfWidth_ofGrid = (argFrameWidth / 2) - ARBITRARY_Left_VertGris_X;
        float Full_with_ofgrid = HalfWidth_ofGrid * 2;

        // float deducted_RightSide_x = received_frameWidth - ARBITRARY_Left_VertGris_X;
        float FileSizeForVertGrid = Full_with_ofgrid;   //deducted_RightSide_x - ARBITRARY_Left_VertGris_X;


        int NumerofHorizontalTilesOnTHisMap = 21;// AppSettings.Instance.Get_InGameRowSize_inTiles(argMaptype);



        float _tileWidth = FileSizeForVertGrid / NumerofHorizontalTilesOnTHisMap;
        //Debug.Log("should get 41.3 ish " + _tileWidth);
      //  float HalfNumberOfHorizontal = NumerofHorizontalTilesOnTHisMap / 2;
        //start at vertMidx, go left 27 0r 17 times  then make a line 17 or 27 times to the right


        float Start_TopPoint_VertGrid_X = ARBITRARY_Left_VertGris_X;//(argFrameWidth / 2) - (HalfNumberOfHorizontal * _tileWidth) - _tileWidth;
        float Start_TOP_Point_VertGrid_Y = Calculated_Margin_Top; // saw 60

        float Start_BotPoint_VertGrid_X = Start_TopPoint_VertGrid_X;
        float Start_BOT_Point_VertGrid_Y = Dw_HORIZON_fromTop; // saw 60 +660

        ListOfAllVerticalGridPoints = new List<Point>();

    

        for (int v = 0; v < NumerofHorizontalTilesOnTHisMap+1; v++)
        {
            float Xoffset = Start_TopPoint_VertGrid_X + ( _tileWidth * v);
          //  Debug.Log(" xoffset" +Xoffset);
            ListOfAllVerticalGridPoints.Add(new Point(Xoffset, Start_TOP_Point_VertGrid_Y));
            ListOfAllVerticalGridPoints.Add(new Point(Xoffset, Start_BOT_Point_VertGrid_Y));


        }


        int IndexOfTopLeftPointForFildRect = 0;
        int IndexOfBotRightPointForFildRect = ListOfAllVerticalGridPoints.Count - 1;
        int Index_ofTopRight_vert_Tracker = 42;


        // use all 22 indecies of grid
        if (argMaptype == e_BrawlMapType.GemGrab)
        {
            IndexOfTopLeftPointForFildRect = 0;
            IndexOfBotRightPointForFildRect = ListOfAllVerticalGridPoints.Count - 1;
            Index_ofTopRight_vert_Tracker = 42;

        }
        else
        if (argMaptype == e_BrawlMapType.Starpark)
        {
            IndexOfTopLeftPointForFildRect = 4;
            IndexOfBotRightPointForFildRect = ListOfAllVerticalGridPoints.Count - 5;
            Index_ofTopRight_vert_Tracker = 38;

        }
        else
        {
            IndexOfTopLeftPointForFildRect = 0;
            IndexOfBotRightPointForFildRect = ListOfAllVerticalGridPoints.Count - 1;
            Index_ofTopRight_vert_Tracker =42;

        }
        Point TopleftFild = ListOfAllVerticalGridPoints[IndexOfTopLeftPointForFildRect];
        Point BotRightFild = ListOfAllVerticalGridPoints[IndexOfBotRightPointForFildRect];
        Fild_Rect = new Rect(TopleftFild, BotRightFild);


        int ExtraPlayerAreaWidth = 15;

        int TrackerTL_x = (int) ListOfAllVerticalGridPoints[Index_ofTopRight_vert_Tracker].x+ ExtraPlayerAreaWidth;
        int TrackerTL_y = (int)ListOfAllVerticalGridPoints[Index_ofTopRight_vert_Tracker].y;
        int trackerwidth = 50;
        int trackerheight = (int)Calculated_game_h;
        TrakVert_Rect = new Rect(TrackerTL_x, TrackerTL_y, trackerwidth, trackerheight);


        float TileThickness = Calculated_game_h / 16;

        int Playerarea_HALF_Thickness = 2; //2 tiles up form horizon and 2 tiles down form horizon
        int PlayerareaTL_x = (int)ListOfAllVerticalGridPoints[IndexOfTopLeftPointForFildRect].x - ExtraPlayerAreaWidth;
        int PlayerareaTL_y = Mathf.RoundToInt( argFrameHeight / 2  - (Playerarea_HALF_Thickness * TileThickness)  );
        int PlayerareaBR_x = (int)ListOfAllVerticalGridPoints[IndexOfBotRightPointForFildRect].x + ExtraPlayerAreaWidth;
        int PlayerareaBR_y = Mathf.RoundToInt(argFrameHeight / 2 + (Playerarea_HALF_Thickness * TileThickness));
        int Playerare_FullWidth = PlayerareaBR_x - PlayerareaTL_x;
        int Playerarea_height = Mathf.RoundToInt(Playerarea_HALF_Thickness*2 * TileThickness);
       // int trackerheight = (int)Calculated_game_h;
        PlayerAres_Rect = new Rect(PlayerareaTL_x, PlayerareaTL_y, Playerare_FullWidth, Playerarea_height);
        Update_src_dst_mRectMats(o_ptsLIST, correct_ptsLIST);
        //throw new System.NotImplementedException();
    }
    public Mat Get_dst_Mat()
    {
        return this.dstRectMat;
    }
    public Mat Get_src_Mat()
    {
        return this.srcRectMat;
    }

    public MatOfPoint Get_Drawing_dst_MatOfPoints()
    {
        return this.correct_pts_MOP;
    }

    public MatOfPoint Get_Drawing_src_MatOfPoints()
    {
        return this.o_pts_MOP;
    }

    #endregion

    public MatOfPoint Get_Drawing_Horizon_and_vertical_MatOfPoints() {
        return this._Horizon_and_vertical_MOP;
    }

    public Rect Get_Drawing_Gameview_Rect() { return this.GameView_Rect; }
    public Rect Get_Drawing_Fild_Rect() { return this.Fild_Rect; }
    public Rect Get_Drawing_Track_Rect() { return this.TrakVert_Rect; }
    public Rect Get_Drawing_PlayerArea_Rect() { return this.PlayerAres_Rect; }
    public List<Point> GetListOfVertGridPointsforLines() {
        return this.ListOfAllVerticalGridPoints;
    }

    void Update()
    {
        //double TrackerTL_x =  ListOfAllVerticalGridPoints[LastXIndex].x;
        //double TrackerTL_y =  ListOfAllVerticalGridPoints[LastXIndex].y;
 
        //double[] arra = new double[4] { TrackerTL_x, TrackerTL_y, 50.0, 600.0 };
        ////TrakVert_Rect = new Rect(TrackerTL_x, TrackerTL_y, trackerwidth, trackerheight);
        //TrakVert_Rect = new Rect(arra);
    }
    #region privateMethods

    void Update_src_dst_mRectMats()
    {
        srcRectMat.put(0, 0,
            o_ptl.x, o_ptl.y,
            o_ptr.x, o_ptr.y,
            o_pbr.x, o_pbr.y,
            o_pbl.x, o_pbl.y);
        dstRectMat.put(0, 0,
            correct_ptl.x, correct_ptl.y,
            correct_ptr.x, correct_ptr.y,
            correct_pbr.x, correct_pbr.y,
            correct_pbl.x, correct_pbl.y);
    }

    void Update_src_dst_mRectMats(List<Point> arg_trap_points, List<Point> arg_corected_points) {

        srcRectMat.put(0, 0, Make_tltr_brBl_xy_array(arg_trap_points));
        dstRectMat.put(0, 0, Make_tltr_brBl_xy_array(arg_corected_points));
    }

    double[] Make_tltr_brBl_xy_array(List<Point> arg_List_points)
    {

        double[] output = new double[8];
        for (int x = 0; x < 4; x++)
        {

            output[x * 2] = arg_List_points[x].x;
            output[x * 2 + 1] = arg_List_points[x].y;
        }
        return output;
    }

#region DiposeMats
    void disposeMyMatsAndTExtures()
    {
        //clean LIST of matofpoints
        if (horizontal_Lines != null)
        {
            for (int mop = 0; mop < horizontal_Lines.Count; mop++)
            {

                if (horizontal_Lines[mop] != null)
                {
                    horizontal_Lines[mop].Dispose();
                }
            }

            vertical_Lines.Clear();
            vertical_Lines = null;
        }

        if (vertical_Lines != null)
        {
            for (int mop = 0; mop < vertical_Lines.Count; mop++)
            {

                if (vertical_Lines[mop] != null)
                {
                    vertical_Lines[mop].Dispose();
                }
            }

            vertical_Lines.Clear();
            vertical_Lines = null;
        }
        //clean matofpoints
        if (_Horizon_and_vertical_MOP != null)
            _Horizon_and_vertical_MOP.Dispose();

        if (o_pts_MOP != null)
            o_pts_MOP.Dispose();
        if (correct_pts_MOP != null)
            correct_pts_MOP.Dispose();
        
        //clean lists
        if (o_ptsLIST != null)
        {
            o_ptsLIST.Clear();
            o_ptsLIST = null;
        }
        if (correct_ptsLIST != null)
        {
            correct_ptsLIST.Clear();
            correct_ptsLIST = null;
        }
        if (_listHorizon_andVertiPoints != null)
        {
            _listHorizon_andVertiPoints.Clear();
            _listHorizon_andVertiPoints = null;
        }
        if (ListOfAllHorizontalGridPoints != null)
        {
            ListOfAllHorizontalGridPoints.Clear();
            ListOfAllHorizontalGridPoints = null;
        }
        if (ListOfAllVerticalGridPoints != null)
        {
            ListOfAllVerticalGridPoints.Clear();
            ListOfAllVerticalGridPoints = null;
        }

        //cleanMats
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
    private void OnDestroy()
    {
        disposeMyMatsAndTExtures();
    }
#endregion
}
