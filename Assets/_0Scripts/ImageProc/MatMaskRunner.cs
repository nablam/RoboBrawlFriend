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
    bool PerspectiveOn = false;
    #endregion

    #region Public_Vars
    public int pubX;
    public HIstogramHandler _histoDisplayer;
    public PerspectiveRectifyer perspectiveMaker;
    public e_BrawlMapType MapType;
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
        webCamTextureToMatHelper = GetComponent<ndicamTextureTomatEventHelper>();
        DisplayQuad = this.gameObject;
        DisplayRenderer = DisplayQuad.GetComponent<Renderer>();

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
        Utils.matToTexture2D(firstmat, texture);
        //***********************************************************************************
        perspectiveMaker.InitiMe_IllUseAppSettings(frameWidth, frameHeight, MapType);

    }

    public void On_ActionMuber(int argActionnumber)
    {
        if (argActionnumber == 0) {
            DoTogglePerspective_perspective();
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


            Draw_Hori_verti_Line_from_MOP(curmat, perspectiveMaker.Get_Drawing_Horizon_and_vertical_MatOfPoints());

            // Draw_TRAPEZOID_PerspLines_MOP(curmat, 200,0,0);
            
            Draw_GameView_from_Rect(curmat, 200, 100, 50, 3);

            Draw_VerticalGridLines(curmat, 20, 60, 50, 3);

            _histoDisplayer.Update_HIstogram_for_ThisMat(curmat, false);
            Utils.matToTexture2D(curmat, texture);

        }
    }
    #endregion

    #region PrivatMethods


    void Draw_Hori_verti_Line_from_MOP(Mat argMat, MatOfPoint argMop)
    {
        Imgproc.line(argMat, argMop.toArray()[0], argMop.toArray()[1], new Scalar(255, 200, 0, 255), 2);
        Imgproc.line(argMat, argMop.toArray()[2], argMop.toArray()[3], new Scalar(255, 200, 0, 255), 2);
    }

     void Draw_TRAPEZOID_PerspLines_MOP(Mat argMat, int argR, int argG, int argB )
    {
        MatOfPoint argMop = perspectiveMaker.Get_Drawing_src_MatOfPoints();
        Imgproc.line(argMat, argMop.toArray()[0], argMop.toArray()[3], new Scalar(argR, argG, argB, 255), 2);
        Imgproc.line(argMat, argMop.toArray()[1], argMop.toArray()[2], new Scalar(argR, argG, argB, 255), 2);
    }

    void Draw_GameView_from_Rect(Mat argMat, int argR, int argG, int argB, int argTHik)
    {
        Rect recttodraw = perspectiveMaker.Get_Drawing_Gameview_Rect();
        Imgproc.rectangle(argMat, recttodraw, new Scalar(argR, argG, argB, 255), argTHik);
    }

    void Draw_VerticalGridLines(Mat argMat, int argR, int argG, int argB, int argTHik) {
       

        int Trim_listCount = perspectiveMaker.GetListOfVertGridPointsforLines().Count;
        Debug.Log(Trim_listCount);
        if (Trim_listCount % 2 != 0) {
            Trim_listCount--;
        }
       // int pindx = 0;
 
        for (int pindx = 0; pindx < Trim_listCount/2; pindx++)
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

    #region PUBLIC_Methods
    public void DoTogglePerspective_perspective()
    {
        PerspectiveOn = !PerspectiveOn;
        Debug.Log("tog perspective is " + PerspectiveOn.ToString());
    }
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
