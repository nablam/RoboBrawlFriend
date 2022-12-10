using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnityExample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rect = OpenCVForUnity.CoreModule.Rect;

public class MatMaskRunner : MonoBehaviour
{

    /// <summary>
    /// temp poit to locate on map
    /// </summary>
    /// 
    public double Ptx;
    public double Pty;
    Point testpoint;
    double Max_X, Min_X;
    double Max_Y,Min_Y;
    double total_Playable_X, total_Playable_Y;
    void UpdateTestPoint()
    {
        if (Ptx < Min_X) Ptx = Min_X;
        if (Ptx > Max_X) Ptx = Max_X;
        if (Pty < Min_Y) Pty = Min_Y;
        if (Pty > Max_Y) Pty = Max_Y;
        testpoint.x = Ptx;
        testpoint.y = Pty;
        // testpoint = new Point(0, 0);
    }

    #region Private_Vars
    GameObject DisplayQuad;
    Renderer DisplayRenderer;
    ndicamTextureTomatEventHelper webCamTextureToMatHelper;
    int frameWidth, frameHeight;
    Mat rgbMat;
    Mat grayMat;
    Mat maskmat;

    Texture2D texture;
    Texture2D texture2;

    List<Mat> UsedMats;
    List<Texture2D> UsedTextures2D;
    bool PerspectiveOn = true;
    FieldTracker _fildTracker;
    ImageErodeDialateFiler _imgDialateErodeFilter;
    bool Toggle_FilterTraking = false;
    KmeanMod _kmeans;
    e_BrawlMapType MapType;
    PerspectiveRectifyer perspectiveMaker;
    SkellyHaarDetector _shellyHaar;
    ShellyBlobDetector _blobber;
    EnemyDetector _enemyDetector;

    #endregion

    #region Public_Vars
    public int lineThikness = 3;
    public bool DoDrawGameView;
    public bool DoDrawCrossLines;
    public bool DoDrawGrid;
    public bool DoDrawField;
    public bool DoDrawPlayarea;
    public bool DoDrawTrackearea;
    public bool DoDrawTrackearea_points;
    public bool DoDrawTrapezoid;
    public bool DoDrawKMEANS;
    public bool DoDrawUpdateHisto;
    public bool DoUseCom;
    public bool DoDrawBlobs;
    public bool DoDrawEnemyCircles;

    public HIstogramHandler _histoDisplayer;
      
  
    public int argK=4;
    public int argCount = 100;
    public double argEpsil = 1.0d;
    public int argAttempts = 1;
    public MiniMapManager MyMinimap;
    public BrawlBrain Brain;
    public SimpleComm _comm;
    public e_BrawlMapName MymapName;
    // public MatsOfROICoordinates _coordinates;
    //  public RectsAndPointsMaker p_maker;
    #endregion

    #region Enabled_Disable_Awake_Start_Destroy
    private void OnEnable()
    {
        EventsManagerLib.On_NDI_startedStreaming += OnWebCamTextureToMatHelperInitialized;
        EventsManagerLib.On_ndi_Dispos += OnWebCamTextureToMatHelperDisposed;
        EventsManagerLib.On_ndi_Error += OnWebCamTextureToMatHelperErrorOccurred;
        EventsManagerLib.On_DoAction_i += On_ActionMuber;
    }
    private void OnDisable()
    {
        EventsManagerLib.On_NDI_startedStreaming -= OnWebCamTextureToMatHelperInitialized;
        EventsManagerLib.On_ndi_Dispos -= OnWebCamTextureToMatHelperDisposed;
        EventsManagerLib.On_ndi_Error -= OnWebCamTextureToMatHelperErrorOccurred;
        EventsManagerLib.On_DoAction_i += On_ActionMuber;
    }

    private void Awake()
    {
        testpoint = new Point(0, 0);
        webCamTextureToMatHelper = GetComponent<ndicamTextureTomatEventHelper>();
        DisplayQuad = this.gameObject;
        DisplayRenderer = DisplayQuad.GetComponent<Renderer>();
        perspectiveMaker = GetComponent<PerspectiveRectifyer>();
        _fildTracker = GetComponent<FieldTracker>();
        _shellyHaar = GetComponent<SkellyHaarDetector>();
        _blobber = GetComponent<ShellyBlobDetector>();
        _imgDialateErodeFilter = GetComponent<ImageErodeDialateFiler>();
        _kmeans = GetComponent<KmeanMod>();
        _enemyDetector = GetComponent<EnemyDetector>();

    }
    void Start()
    {
        webCamTextureToMatHelper.InitializeMePleaze();

    }
    private void OnDestroy()
    {
        disposeMyMatsAndTExtures();
    }
    #endregion

    #region EventHAndlers
    public void OnWebCamTextureToMatHelperInitialized(int argW, int argH)
    {

        //Debug.Log("HEARD Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);

        frameWidth = argW;
        frameHeight = argH;
        texture = new Texture2D(frameWidth, frameHeight, TextureFormat.RGBA32, false);
        texture2 = new Texture2D(frameWidth, frameHeight, TextureFormat.RGBA32, false);


        rgbMat = new Mat(frameHeight, frameWidth, CvType.CV_8UC3);
        grayMat = new Mat(frameHeight, frameWidth, CvType.CV_8SC1);
        maskmat = new Mat(frameHeight, frameWidth, CvType.CV_8U, Scalar.all(0));

        UsedMats = new List<Mat>();
        UsedMats.Add(rgbMat);
        UsedMats.Add(grayMat);
        UsedMats.Add(maskmat);
        UsedTextures2D = new List<Texture2D>();
        UsedTextures2D.Add(texture);
        UsedTextures2D.Add(texture2);

        DisplayRenderer.material.mainTexture = texture;

        Mat firstmat = webCamTextureToMatHelper.GetMat();
        //Utils.matToTexture2D(firstmat, texture);
        //***********************************************************************************
        // figgureout maptype from map name :TODO : solo map stuff
        //***********************************************************************************

        if (MymapName == e_BrawlMapName.Starpark)
        {
            MapType = e_BrawlMapType.Starpark;
        }
        else 
        {
            MapType = e_BrawlMapType.GemGrab;
        }

        perspectiveMaker.InitiMe_IllUseAppSettings(frameWidth, frameHeight, MapType);

        MyMinimap.InitiMe_IllUseAppSettings(frameWidth, frameHeight, MymapName);

        Brain.INITme_giveemminimap(MyMinimap, _comm, DoUseCom);

        _blobber.InitializeBoloer(perspectiveMaker.Get_Drawing_PlayerArea_Rect().width, perspectiveMaker.Get_Drawing_PlayerArea_Rect().height);

        _enemyDetector.InitMe(perspectiveMaker.Get_Drawing_Fild_Rect().width, perspectiveMaker.Get_Drawing_Fild_Rect().height);
    }

    public void On_ActionMuber(int argActionnumber)
    {
        if (argActionnumber == 0)
        {
            DoTogglePerspective_perspective();
        }
        else
            if (argActionnumber == 1)
        {
            Do_reset_field_Tracker_();
        }
        else
            if (argActionnumber == 2) {
            Toggle_FilterTraking = !Toggle_FilterTraking;
        }
        else
            if (argActionnumber == 3)
        {
            Do_resetBrainChain();
        }
    }

    public void OnWebCamTextureToMatHelperDisposed()
    {
        Debug.Log("Temp OnDisposed ");

        disposeMyMatsAndTExtures();

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

        if (UsedTextures2D != null)
        {
            for (int i = 0; i < UsedTextures2D.Count; i++)
            {

                if (UsedTextures2D[i] != null)
                {
                    Texture2D.Destroy(UsedTextures2D[i]);
                    UsedTextures2D[i] = null;
                }
            }
        }
    }

    public void OnWebCamTextureToMatHelperErrorOccurred(int errorCode)
    {
        Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
    }
    #endregion

    #region CanRunMutex
    bool IsItPlayingAndUpdated()
    {
        return webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame();
    }
    #endregion




    #region UPDATE
    void Update()
    {
        if (IsItPlayingAndUpdated())
        {
            Mat curmat = webCamTextureToMatHelper.GetMat();
            //Draw_TRAPEZOID_PerspLines_MOP(curmat, 0, 250, 0);

            if (PerspectiveOn)
            {

                Mat perspectiveTransform = Imgproc.getPerspectiveTransform(perspectiveMaker.Get_src_Mat(), perspectiveMaker.Get_dst_Mat());
                Imgproc.warpPerspective(curmat, curmat, perspectiveTransform, new Size(curmat.cols(), curmat.rows()));
            }

   


            Mat rgbMat_Tracker = curmat.submat(perspectiveMaker.Get_Drawing_Track_Rect());
            Mat rgbMat_Player = curmat.submat(perspectiveMaker.Get_Drawing_PlayerArea_Rect());
            Mat rgbMat_Field = curmat.submat(perspectiveMaker.Get_Drawing_Fild_Rect());

            if(DoDrawUpdateHisto)
            _histoDisplayer.Update_HIstogram_for_ThisMat(curmat, false);

            if(DoDrawKMEANS)
            _kmeans.DoKmeans(rgbMat_Player,   argK,  argCount,  argEpsil,  argAttempts);

            if (Toggle_FilterTraking) _imgDialateErodeFilter.Do_Image_manipulation(rgbMat_Tracker);

            _fildTracker.TrackRoi(rgbMat_Tracker, DoDrawTrackearea_points);
            ///
            //NotYEt
            ///
            ///  _shellyHaar.Dohaardetect(rgbMat_Player);

            _blobber.DoRunBloberGreen(rgbMat_Player, DoDrawBlobs);


            _enemyDetector.UpdateFindEnemyCircles(rgbMat_Field, DoDrawEnemyCircles);

            rgbMat_Tracker.copyTo(curmat.submat(perspectiveMaker.Get_Drawing_Track_Rect()));
            rgbMat_Player.copyTo(curmat.submat(perspectiveMaker.Get_Drawing_PlayerArea_Rect()));
            rgbMat_Field.copyTo(curmat.submat(perspectiveMaker.Get_Drawing_Fild_Rect()));

            if (DoDrawGameView) Draw_GameView_from_Rect(curmat, 250, 20, 2, lineThikness);
            if (DoDrawCrossLines) Draw_Hori_verti_Line_from_MOP(curmat, perspectiveMaker.Get_Drawing_Horizon_and_vertical_MatOfPoints());
            if (DoDrawGrid) Draw_VerticalGridLines(curmat, 20, 60, 50, 1);
            if (DoDrawField) Draw_Field_from_Rect(curmat, 0, 128, 250, lineThikness);
            if (DoDrawPlayarea) Draw_PlayerArea_from_Rect(curmat, 255, 1, 0, lineThikness);
            if (DoDrawTrackearea) Draw_track_from_Rect(curmat, 200, 100, 50, 1);
            if (DoDrawTrapezoid) Draw_TRAPEZOID_PerspLines_MOP(curmat, 200, 100, 50);

            Rect recttodraw = perspectiveMaker.Get_Drawing_Gameview_Rect();

            Max_X = recttodraw.x + recttodraw.width;
            Max_Y = recttodraw.y+ recttodraw.height;
            Min_X = recttodraw.x;
            Min_Y = recttodraw.y;

            total_Playable_X = Max_X - Min_X;
            total_Playable_Y = Max_Y - Min_Y;

            UpdateTestPoint();

            MyMinimap.LiveupdateMapPoint(testpoint, total_Playable_X, total_Playable_Y);
            Imgproc.circle(curmat, testpoint, 8, new Scalar(25, 150, 60, 255), 2);

            Utils.matToTexture2D(curmat, texture, false);

        }
    }
    #endregion

    #region PrivatMethods

    void DoTogglePerspective_perspective()
    {
        PerspectiveOn = !PerspectiveOn;
        Debug.Log("tog perspective is " + PerspectiveOn.ToString());
    }

    void Do_reset_field_Tracker_()
    {

        _fildTracker.ResetTracking();
    }

    void Do_resetBrainChain() {
        Brain.ResetComm();
    
    }


    #region SubRegionDraw
    void Draw_Hori_verti_Line_from_MOP(Mat argMat, MatOfPoint argMop)
    {
        Imgproc.line(argMat, argMop.toArray()[0], argMop.toArray()[1], new Scalar(255, 200, 0, 255), 2);
        Imgproc.line(argMat, argMop.toArray()[2], argMop.toArray()[3], new Scalar(255, 200, 0, 255), 2);
    }

    void Draw_TRAPEZOID_PerspLines_MOP(Mat argMat, int argR, int argG, int argB)
    {
        MatOfPoint argMop = perspectiveMaker.Get_Drawing_src_MatOfPoints();
        Imgproc.line(argMat, argMop.toArray()[0], argMop.toArray()[3], new Scalar(argR, argG, argB, 255), lineThikness);
        Imgproc.line(argMat, argMop.toArray()[1], argMop.toArray()[2], new Scalar(argR, argG, argB, 255), lineThikness);
    }

    void Draw_GameView_from_Rect(Mat argMat, int argR, int argG, int argB, int argTHik)
    {
        Rect recttodraw = perspectiveMaker.Get_Drawing_Gameview_Rect();
        Imgproc.rectangle(argMat, recttodraw, new Scalar(argR, argG, argB, 255), argTHik);
    }


    void Draw_Field_from_Rect(Mat argMat, int argR, int argG, int argB, int argTHik)
    {
        Rect recttodraw = perspectiveMaker.Get_Drawing_Fild_Rect();
        Imgproc.rectangle(argMat, recttodraw, new Scalar(argR, argG, argB, 255), argTHik);
    }

    void Draw_track_from_Rect(Mat argMat, int argR, int argG, int argB, int argTHik)
    {
        Rect recttodraw = perspectiveMaker.Get_Drawing_Track_Rect();
        Imgproc.rectangle(argMat, recttodraw, new Scalar(argR, argG, argB, 255), argTHik);
    }

    void Draw_PlayerArea_from_Rect(Mat argMat, int argR, int argG, int argB, int argTHik)
    {
        Rect recttodraw = perspectiveMaker.Get_Drawing_PlayerArea_Rect();
        Imgproc.rectangle(argMat, recttodraw, new Scalar(argR, argG, argB, 255), argTHik);
    }

    void Draw_VerticalGridLines(Mat argMat, int argR, int argG, int argB, int argTHik)
    {


        int Trim_listCount = perspectiveMaker.GetListOfVertGridPointsforLines().Count;
        //Debug.Log(Trim_listCount);
        if (Trim_listCount % 2 != 0)
        {
            Trim_listCount--;
        }
        // int pindx = 0;

        for (int pindx = 0; pindx < Trim_listCount / 2; pindx++)
        {
            //Imgproc.line(argMat, perspectiveMaker.GetListOfVertGridPointsforLines()[pindx * 2], perspectiveMaker.GetListOfVertGridPointsforLines()[(pindx * 2) + 1], new Scalar(argR, argG, argB, 255), 2);
            // if (pindx > 0 && pindx < (Trim_listCount / 2) - 1) continue;


            Imgproc.line(argMat, perspectiveMaker.GetListOfVertGridPointsforLines()[pindx * 2], perspectiveMaker.GetListOfVertGridPointsforLines()[(pindx * 2) + 1], new Scalar(argR, argG, argB, 255), 2);

            // if (pindx > 0) continue;
            // Debug.Log(perspectiveMaker.GetListOfVertGridPointsforLines()[pindx * 2]);
            // Debug.Log(perspectiveMaker.GetListOfVertGridPointsforLines()[(pindx * 2) + 1]);


        }

    }
    #endregion
    #endregion

    #region PUBLIC_Methods
  

    #endregion


    #region Unused
    private void privFunk() { }
    private void SetCamera_fromScreenSize()
    {

        float widthScale = (float)Screen.width / frameWidth;
        float heightScale = (float)Screen.height / frameHeight;
        if (widthScale < heightScale)
        {
            Camera.main.orthographicSize = (frameWidth * (float)Screen.height / (float)Screen.width) / 2;
        }
        else
        {
            Camera.main.orthographicSize = frameHeight / 2;
        }
    }
    private void SetCamera_from_receivedFrameSize(Mat argFrameMat)
    {
        float width = argFrameMat.width();
        float height = argFrameMat.height();

        float widthScale = (float)Screen.width / width;
        float heightScale = (float)Screen.height / height;
        if (widthScale < heightScale)
        {
            Camera.main.orthographicSize = (width * (float)Screen.height / (float)Screen.width) / 2;
        }
        else
        {
            Camera.main.orthographicSize = height / 2;
        }

    }
    #endregion
}
