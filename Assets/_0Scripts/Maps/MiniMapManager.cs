using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rect = OpenCVForUnity.CoreModule.Rect;
public class MiniMapManager : MonoBehaviour
{
    Mat imgMat;
    Mat dstMat;
    Mat tempmat;
    Texture2D imgTexture_originalPic;
    Texture2D OCVtexture;
    WaypointBuilder waypointMaker;

    //the map is 310x410
    // each tile is 10x10 so 31x41 tilles
    // the game view has a height of 16 tiles 
    Rect GameView, PlayerArea;
    int GameviewStart_YPos;
    int ConvertedY;
    int ConvertedPX;
    int ConvertedPY;
    int TempOssfet;

    public double Ptx;
    public double Pty;
    public double totalPlayableMap_X;
    public double totalPlayableMap_Y;
    public double totalViewableMap_Y;
    Point testpoint;
    Point testMapPoint;
    double Max_X, Min_X;
    double Max_Y, Min_Y;

    List<Point> waypoints;
       e_BrawlMapName _mapName;
    bool isInited;

  //  public int Live_Player_X = 155;
  //  public int Live_Player_Y = 70;

    
    public int INDX08 = 0;

    List<EnemyData> LocatedAndTrackedEnemies;
    PlayerLocData PlayerData;
    public List<EnemyData> get_enemiesData() { return this.LocatedAndTrackedEnemies; }
    public PlayerLocData Get_PlayerData() { return this.PlayerData; }
    public void Update_LocatedEnemiesInOrder(List<EnemyData> arg_LocatedAndTrackedEnemies) { 
       


        if (arg_LocatedAndTrackedEnemies != null)
        {

            LocatedAndTrackedEnemies = arg_LocatedAndTrackedEnemies;

            for (int i = 0; i < LocatedAndTrackedEnemies.Count; i++)
            {
                EnemyData ed = LocatedAndTrackedEnemies[i];
               
                double argx = ed.GetLoc_point_inView().x;
                double argy = ed.GetLoc_point_inView().y;
           
                double fullH = ed.Get_FrameHeight();
                double fullW = ed.Get_FrameWidth();
         
                double temp_localx = totalPlayableMap_X * (argx / fullW) + Min_X;
                double temp_localy = (GameView.height * (argy / fullH)) + ConvertedY;
               
                ed.SetLocV2_inMap(temp_localx, temp_localy);


            }



        }





    }
    private void OnEnable()
    {
        EventsManagerLib.On_FiieldScrollDist_ += HeardFiledScrolled;
         EventsManagerLib.On_Player_Located += HeardPlayerLocalized;
      //  EventsManagerLib.On_SingleCirle_Detected += HeardSingleCircle;

    }
    private void OnDisable()
    {
       EventsManagerLib.On_FiieldScrollDist_ -= HeardFiledScrolled;
       EventsManagerLib.On_Player_Located -= HeardPlayerLocalized;
       // EventsManagerLib.On_SingleCirle_Detected -= HeardSingleCircle;
    }
 
    void HeardFiledScrolled(double argAvrDist, double argPerventY) {
        double temp = argAvrDist * 20 / 100;
        ConvertedY = (int)temp + GameviewStart_YPos;
    }
 
    void HeardPlayerLocalized(double argRawX, double argRawY, int argRect_X, int argRect_Y, int argRect_W, int argRect_H) {
        double LocalMapPlayer_X = (argRawX / argRect_W) * PlayerArea.width + Min_X;
        double LocalMapPlayer_Y = (argRawY / argRect_H) * PlayerArea.height + (PlayerArea.y - PlayerArea.height + 40);
        PlayerData.Update_InViewPoint(argRawX, argRawY, LocalMapPlayer_X, LocalMapPlayer_Y);
    }
 

    public bool DrawEnemies;

    public void InitiMe_IllUseAppSettings(int argW, int argH, e_BrawlMapName argMapname, int arg_playerDetectionView_WIDTH, int arg_playerDetectionView_HEIGHT )
    {
        waypointMaker = GetComponent<WaypointBuilder>();
        waypoints =  waypointMaker.GeneratePointsForMap( argMapname);
        isInited = true;
        _mapName = argMapname;
        // string Mapname= AppSettings.Instance.get_
        string MapPath = "_minimaps/MiniMap_";
        MapPath += _mapName.ToString();

        imgTexture_originalPic = Resources.Load<Texture2D>(MapPath);
        OCVtexture = new Texture2D(imgTexture_originalPic.width, imgTexture_originalPic.height, TextureFormat.RGBA32, false);
        imgMat = new Mat(imgTexture_originalPic.height, imgTexture_originalPic.width, CvType.CV_8UC4);
        Utils.texture2DToMat(imgTexture_originalPic, imgMat, false);//No flip!
        gameObject.transform.localScale = new Vector3(imgMat.cols(), imgMat.rows(), 1);
        gameObject.GetComponent<Renderer>().material.mainTexture = OCVtexture;
        //tempmat = new Mat(im)
        GameviewStart_YPos = 0;// 2 * 10; //the view startst 2 tiles up for starpark

        double tempmaxY_offsetFromTop = 0;
        if (_mapName == e_BrawlMapName.Starpark)
        {
            PlayerArea = new Rect(70, GameviewStart_YPos + 20, 170, 40);
            GameView = new Rect(0, GameviewStart_YPos, 310, 160);
            tempmaxY_offsetFromTop = 70;


        }
        else
        {
            PlayerArea = new Rect(50, GameviewStart_YPos + 40, 210, 50);
            GameView = new Rect(0, GameviewStart_YPos, 310, 160);
            tempmaxY_offsetFromTop = 40;
        }
        testpoint = new Point(0, 0);
        testMapPoint = new Point(0, 0);

 
        Debug.Log("!!!!!!!!!!!! minimap " + imgTexture_originalPic.width + "x" + imgTexture_originalPic.height + "");

        Max_X = PlayerArea.x + PlayerArea.width;
        Max_Y = imgTexture_originalPic.height - tempmaxY_offsetFromTop;

        Min_X = PlayerArea.x;
        Min_Y = 40;//always no matter the map

        totalPlayableMap_X = Max_X - Min_X;
        totalPlayableMap_Y = Max_Y - Min_Y;
        totalViewableMap_Y = GameView.y;
        LocatedAndTrackedEnemies = new List<EnemyData>();
        PlayerData = new PlayerLocData(155, 70, new Scalar(0, 0, 0, 255),arg_playerDetectionView_WIDTH, arg_playerDetectionView_HEIGHT);

    }
    private void OnDestroy()
    {
        DisposeMatMatTexture(imgMat, dstMat, tempmat, OCVtexture);
    }

    void DisposeMatMatTexture(Mat argRGBmat, Mat argMatimage, Mat argTempMat, Texture2D argTexture2d)
    {

        if (argRGBmat != null) argRGBmat.Dispose();
        if (argMatimage != null) argMatimage.Dispose();
        if (argTempMat != null) argTempMat.Dispose();
        if (argTexture2d != null)
        {
            Texture2D.Destroy(argTexture2d);
            argTexture2d = null;
        }
    }

    void UpdateTestPoint() {
        if (Ptx < Min_X) Ptx = Min_X;
        if (Ptx > Max_X) Ptx = Max_X;
        if (Pty < Min_Y) Pty = Min_Y;
        if (Pty > Max_Y) Pty = Max_Y;
        testpoint.x = Ptx;
        testpoint.y = Pty;
    }

    public void LiveupdateMapPoint(Point argRawZeroBasedPoint, double argMy_totalPlayable_x, double argMy_totalPlayable_y) {

        //  0      30       38                                 //   0       20                  100
        //  0-------|-------|------------|                     //  ---------|--------------------

        //   x = 8 * (20/100)  =  1.6  +30

        double Localx = totalPlayableMap_X * (argRawZeroBasedPoint.x / argMy_totalPlayable_x) + Min_X;

        double localy = GameView.y * (argRawZeroBasedPoint.y / argMy_totalPlayable_y)  ;

        Ptx = Localx;
        Pty = localy;

        testMapPoint.x = Localx;
        testMapPoint.y = Min_Y + GameviewStart_YPos;

       // UpdateTestPoint();
    }
    private void Update()
    {
        if (!isInited) return;

      

        GameView.y = ConvertedY;
        PlayerArea.y = GameView.y+60 ;


        UpdateTestPoint();

        Mat m = imgMat.clone();
        Imgproc.circle(m, PlayerData.Get_inMapPoint(), 10, new Scalar(255, 50, 60, 255), 2);
        Imgproc.circle(m, testpoint, 8, new Scalar(25, 150, 60, 255), 2);
        Imgproc.circle(m, testMapPoint, 8, new Scalar(00, 10, 250, 255), 2);

        if (DrawEnemies)
            if (LocatedAndTrackedEnemies != null)
            {
                if (LocatedAndTrackedEnemies.Count > 0)
                {

                    for (int e = 0; e < LocatedAndTrackedEnemies.Count; e++) {

                        
                            

                           

                        int radius = 10 - (LocatedAndTrackedEnemies[e].Get_threatLevel() * 2);

                        float confidance = LocatedAndTrackedEnemies[e].GetConfidance();
                        double Rvalue = 255;
                        double Gvalue = 255;
                        if (confidance > 51) {
                            float HalfConfidance = confidance - 50;

                            Gvalue = 255 * (HalfConfidance / 50) * 100;
                            Rvalue = 0;

                        }
                        else
                            if (confidance <=50) {
                            float HalfConfidance = confidance - 50;

                            Rvalue = 255 * (HalfConfidance / 50) * 100;
                            Gvalue = 0;

                        }

                        Imgproc.circle(m,
                            LocatedAndTrackedEnemies[e].GetLoc_point_inMap(),
                            radius,
                            new Scalar(Rvalue, Gvalue, 0, 255)
                            ,
                            2) ;

                        Imgproc.line(m,
                            LocatedAndTrackedEnemies[e].GetLoc_point_inMap(),
                            PlayerData.Get_inMapPoint(),
                            new Scalar(Rvalue, Gvalue, 0, 255),
                            1);


                    }

                }
            }

        foreach (Point p in waypoints) {

            Imgproc.circle(m,
                              p,
                               8,
                               new Scalar(140, 15, 54, 255)
                               ,
                               2);

        }


        Imgproc.rectangle(m, GameView, new Scalar(255, 0, 0, 255), 2);
        Imgproc.rectangle(m, PlayerArea, new Scalar(0, 0, 255, 255), 2);
 


        Utils.matToTexture2D(m, OCVtexture, false);//no flip
    }

}
