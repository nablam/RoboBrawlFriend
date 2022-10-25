using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rect = OpenCVForUnity.CoreModule.Rect;

public class MinimapTest : MonoBehaviour
{
    // public double THRESH = 0.8;
    // public double MAXVAL = 1.0;

    //public const int THRESH_BINARY = 0;
    //public const int THRESH_BINARY_INV = 1;
    //public const int THRESH_TRUNC = 2;
    //public const int THRESH_TOZERO = 3;
    //public const int THRESH_TOZERO_INV = 4;
    //public const int THRESH_MASK = 7;
    //public const int THRESH_OTSU = 8;
    //public const int THRESH_TRIANGLE = 16;

    // public int ttype012347816 = 0;


    //Select a Texture in the Inspector to change to
    //public Texture m_Texture;

    Mat imgMat;
    Mat dstMat;
    Mat tempmat;
    Texture2D imgTexture_originalPic;
    Texture2D OCVtexture;

    //the map is 310x410
    // each tile is 10x10 so 31x41 tilles
    // the game view has a height of 16 tiles 
    Rect GameView, PlayerArea;
    int GameviewStart_YPos;
    public int ConvertedY;
    public int ConvertedPX;
    public int ConvertedPY;
    public int TempOssfet;

    public double Ptx;
    public double Pty;

    Point PlayerPoint;
    Point[] enemiepoints;
    Vector3[] enmeyV3;
    Vector3 PlaerV3;

    Point[] CARDINALpOINTSpoints;
    Vector3[] caRDINALv3;
    Point fromPt, ToPt;
    Vector3 FromV3, Tov3;
    e_BrawlMapName _mapName;
    bool isInited;

    public int Live_Player_X=155;
    public int Live_Player_Y=70;

    public int fromPt_X;
    public int fromPt_Y;

    public int ToPt_X;
    public int ToPt_Y;



    private void OnEnable()
    {
        EventsManagerLib.On_NDI_startedStreaming += OnWebCamTextureToMatHelperInitialized;
        //EventsManagerLib.On_ndi_Dispos += OnWebCamTextureToMatHelperDisposed;
        //EventsManagerLib.On_ndi_Error += OnWebCamTextureToMatHelperErrorOccurred;
    }
    private void OnDisable()
    {
        EventsManagerLib.On_NDI_startedStreaming -= OnWebCamTextureToMatHelperInitialized;
        //EventsManagerLib.On_ndi_Dispos -= OnWebCamTextureToMatHelperDisposed;
        //EventsManagerLib.On_ndi_Error -= OnWebCamTextureToMatHelperErrorOccurred;
    }
    public void OnWebCamTextureToMatHelperInitialized(int argW, int argH) {
        //InitializeMatsandTextures();
        //isInited = true;
    }
    public void UpdateLocationSimpleVomit(double argVomit)
    {


        double temp = ConvertIngameViewYtoMinimapY(argVomit * -1);
        ConvertedY = (int)temp + GameviewStart_YPos;
        //  Debug.Log("vomitt "+ temp);

    }


    public void UpdateLocationSimpleVomit_Player(double argVomitx, double argvomity)
    {

        double tempx = ConvertIngameViewYtoMinimapY(argVomitx);
        ConvertedPX = (int)tempx;
        // Debug.Log("vomitt " + tempx);
        PlayerPoint.x = tempx + 60;
        PlayerPoint.y = PlayerArea.y + 20;// + argvomity/2;

    }
    double ConvertIngameViewYtoMinimapY(double argGameY)
    {


        return argGameY * 20 / 100;
    }

    double ConvertIngameViewYtoMinimapX(double argGameX)
    {


        return argGameX * 50;
    }
    public int INDX08 = 0;
    public void UpdateLocationSimpleVomit_Enemies(double argVomitx, double argVomity)
    {

        double tempy = argVomity * (310 / 720);// ConvertIngameViewYtoMinimapY(argvomity);
        double tempx = argVomitx * (410 / 1280); //ConvertIngameViewYtoMinimapX(argVomitx);

        Ptx = tempx;

        Pty = tempy;
        Point p1 = new Point(tempx, tempy);
        enemiepoints[0] = p1;
        //enemiepoints[0].x = tempx;
        //enemiepoints[0].y = tempy;

    }
    public double CENTERXY = 155;
    public double CXY_offset = 100;
    public void InitiMe_IllUseAppSettings(int argW, int argH, e_BrawlMapName argMapname) {
        
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

        GameView = new Rect(0, GameviewStart_YPos, 310, 160);
        PlayerArea = new Rect(50, GameviewStart_YPos + 40, 210, 50);
        PlayerPoint = new Point(Live_Player_X, Live_Player_Y);
        PlaerV3 = new Vector3((float)PlayerPoint.x, (float)PlayerPoint.y, 0);
        Debug.Log("!!!!!!!!!!!! minimap " + imgTexture_originalPic.width + "x" + imgTexture_originalPic.height + "");
        enemiepoints = new Point[4];
        enemiepoints[0] = new Point(100, 70);



        enemiepoints[1] = new Point(210, 70);
        enemiepoints[2] = new Point(100, 100);
        enemiepoints[3] = new Point(210, 100);
        enmeyV3 = new Vector3[4];
        enmeyV3[0] = new Vector3((float)enemiepoints[0].x, (float)enemiepoints[0].y, 0);
        enmeyV3[1] = new Vector3((float)enemiepoints[1].x, (float)enemiepoints[1].y, 0);
        enmeyV3[2] = new Vector3((float)enemiepoints[2].x, (float)enemiepoints[2].y, 0);
        enmeyV3[3] = new Vector3((float)enemiepoints[3].x, (float)enemiepoints[3].y, 0);



        CARDINALpOINTSpoints=new Point[9];

       
        CARDINALpOINTSpoints[0] = new Point(CENTERXY, CENTERXY+ CENTERXY- CXY_offset);
        CARDINALpOINTSpoints[1] = new Point(CENTERXY + CENTERXY- CXY_offset, CENTERXY + CENTERXY- CXY_offset);
        CARDINALpOINTSpoints[2] = new Point(CENTERXY + CENTERXY - CXY_offset, CENTERXY );
        CARDINALpOINTSpoints[3] = new Point(CENTERXY +CENTERXY- CXY_offset, CENTERXY - CENTERXY+ CXY_offset);
        CARDINALpOINTSpoints[4] = new Point(CENTERXY, CENTERXY - CENTERXY + CXY_offset);
        CARDINALpOINTSpoints[5] = new Point(CENTERXY - CENTERXY+ CXY_offset, CENTERXY  -CENTERXY+ CXY_offset);
        CARDINALpOINTSpoints[6] = new Point(CENTERXY - CENTERXY+ CXY_offset, CENTERXY  );
        CARDINALpOINTSpoints[7] = new Point(CENTERXY - CENTERXY+ CXY_offset, CENTERXY +CENTERXY- CXY_offset);

        CARDINALpOINTSpoints[8] = new Point(CENTERXY , CENTERXY ); //NEUTRAL
        caRDINALv3 = new Vector3[9];
        for (int I = 0; I < 9; I++) {
            caRDINALv3[I] = new Vector3((float)CARDINALpOINTSpoints[I].x, (float)CARDINALpOINTSpoints[I].y, 0);
        }
    }
    public bool DrawEnemies;

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



    //void Start()
    //{

    //    InitializeMatsandTextures();

    //}

    // Update is called once per frame
    void Update()
    {
        if (!isInited) return;




        GameView.y = ConvertedY;
        PlayerArea.y = GameView.y + 40;

        fromPt.x = fromPt_X;
        fromPt.y = fromPt_Y;
        ToPt.x = ToPt_X;
        ToPt.y = ToPt_Y;
        PlayerPoint.x = Live_Player_X;
        PlayerPoint .y= Live_Player_Y;

        UpdateVector3s();





        //Imgproc.threshold(imgMat, imgMat, THRESH, MAXVAL, ttype012347816);//threshold = 0.8

        //Imgproc.rectangle(imgMat, new Point(0, 200), new Point(200,100), new Scalar(255, 0, 0, 255), 2); GameView

        Mat m = imgMat.clone();
        Imgproc.circle(m, PlayerPoint, 10, new Scalar(255, 50, 60, 255), 2);

        if(DrawEnemies)
        if (enemiepoints != null)
        {
            if (enemiepoints.Length > 0)
            {
                Imgproc.circle(m, enemiepoints[0], 4, new Scalar(0, 255, 60, 255), -1);
                Imgproc.circle(m, enemiepoints[1], 4, new Scalar(0, 255, 60, 255), -1);
                Imgproc.circle(m, enemiepoints[2], 4, new Scalar(0, 255, 60, 255), -1);
                Imgproc.circle(m, enemiepoints[3], 4, new Scalar(0, 255, 60, 255), -1);
            }
        }

        if (CARDINALpOINTSpoints != null && CARDINALpOINTSpoints.Length==9) 
        {
            if(INDX08>8) INDX08 = 8;
            if (INDX08 < 1) INDX08 = 0;

            Imgproc.circle(m, CARDINALpOINTSpoints[INDX08], 4, new Scalar(78, 25, 180, 255), -1);
            //Imgproc.circle(m, CARDINALpOINTSpoints[1], 4, new Scalar(78, 25, 180, 255), -1);
            //Imgproc.circle(m, CARDINALpOINTSpoints[2], 4, new Scalar(78, 25, 180, 255), -1);
            //Imgproc.circle(m, CARDINALpOINTSpoints[3], 4, new Scalar(78, 25, 180, 255), -1);
            //Imgproc.circle(m, CARDINALpOINTSpoints[4], 4, new Scalar(78, 25, 180, 255), -1);
            //Imgproc.circle(m, CARDINALpOINTSpoints[5], 4, new Scalar(78, 25, 180, 255), -1);
            //Imgproc.circle(m, CARDINALpOINTSpoints[6], 4, new Scalar(78, 25, 180, 255), -1);
            //Imgproc.circle(m, CARDINALpOINTSpoints[7], 4, new Scalar(78, 25, 180, 255), -1); 

            Imgproc.circle(m, CARDINALpOINTSpoints[0], 3, new Scalar(100, 100, 180, 255), 1);
        }



        Imgproc.rectangle(m, GameView, new Scalar(255, 0, 0, 255), 2);
        Imgproc.rectangle(m, PlayerArea, new Scalar(0, 0, 255, 255), 2);


        Imgproc.circle(m, fromPt, 3, new Scalar(20, 255, 40, 255), 2);
        Imgproc.circle(m, ToPt, 3, new Scalar(255, 20, 40, 255), 2);


        Utils.matToTexture2D(m, OCVtexture, false);//no flip

    }


    void UpdateVector3s() {

        PlaerV3.x = (float)PlayerPoint.x;
        PlaerV3.y= (float)PlayerPoint.y;

        for (int indx = 0; indx < enemiepoints.Length; indx++) {

            enmeyV3[indx].x = (float)enemiepoints[indx].x; enmeyV3[indx].y = (float)enemiepoints[indx].y;
        }

        for (int I = 0; I < CARDINALpOINTSpoints.Length; I++)
        {
            caRDINALv3[I].x = (float)CARDINALpOINTSpoints[I].x;
            caRDINALv3[I].y = (float)CARDINALpOINTSpoints[I].y;
        }

    }

    public Vector3[] Get_Final_Enemilocations() { return this.enmeyV3; }
    public Vector3[] Get_Cardinallocations() { return this.caRDINALv3; }
    public Vector3 Get_Playerlocations() { return this.PlaerV3; }
}
/*
 
   void InitializeMatsandTextures()
    {
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

        GameView = new Rect(0, GameviewStart_YPos, 310, 160);
        PlayerArea = new Rect(50, GameviewStart_YPos + 40, 210, 50);
        PlayerPoint = new Point(0, 0);
        Debug.Log("!!!!!!!!!!!! minimap " + imgTexture_originalPic.width + "x" + imgTexture_originalPic.height + "");
        enemiepoints = new Point[4];
        enemiepoints[0] = new Point(0, 0);



        enemiepoints[1] = new Point(0, 0);
        enemiepoints[2] = new Point(0, 0);
        enemiepoints[3] = new Point(0, 0);




    }
 */