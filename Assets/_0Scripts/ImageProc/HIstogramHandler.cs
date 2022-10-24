using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnityExample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIstogramHandler : MonoBehaviour
{



    #region Private_Vars
    GameObject HistoDisplayObj;
    Renderer HistDisplayRenderer;
    bool _heardInited;
    int frameWidth, frameHeight;
    int HistDisplayWidth, HistDisplayHeight;
    int[] Range_ = new int[2] { 0, 0 };
    int[] Rangeget_ = new int[2] { 0, 0 };
    Mat HistDisplayMat;
    Texture2D HistDisplaytexture;
    List<Mat> UsedMats;
    List<Texture2D> UsedTextures2D;
    #endregion

    #region Public_Vars
    public int pubX;
    #endregion

    #region Enabled_Disable_Awake_Start_Destroy
    private void OnEnable()
    {
        EventsManagerLib.On_NDI_startedStreaming += OnWebCamTextureToMatHelperInitialized;
        EventsManagerLib.On_ndi_Dispos += OnWebCamTextureToMatHelperDisposed;
        EventsManagerLib.On_ndi_Error += OnWebCamTextureToMatHelperErrorOccurred;
    }
    private void OnDisable()
    {
        EventsManagerLib.On_NDI_startedStreaming -= OnWebCamTextureToMatHelperInitialized;
        EventsManagerLib.On_ndi_Dispos -= OnWebCamTextureToMatHelperDisposed;
        EventsManagerLib.On_ndi_Error -= OnWebCamTextureToMatHelperErrorOccurred;
    }

    private void Awake()
    {
        HistoDisplayObj = this.gameObject;
        HistDisplayRenderer = HistoDisplayObj.GetComponent<Renderer>();
        HistDisplayWidth = (int)HistoDisplayObj.transform.localScale.x;
        HistDisplayHeight = (int)HistoDisplayObj.transform.localScale.y;
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
    public void OnWebCamTextureToMatHelperInitialized(int argW, int argH)
    {
        _heardInited = true;
       // Debug.Log("Histo HEARD Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);
        frameWidth = argW;
        frameHeight = argH;


        HistDisplayMat = new Mat(HistDisplayHeight, HistDisplayWidth, CvType.CV_8UC3, new Scalar(0, 0, 0));
        HistDisplaytexture = new Texture2D(HistDisplayWidth, HistDisplayHeight, TextureFormat.RGBA32, false);




        UsedMats = new List<Mat>();
        UsedMats.Add(HistDisplayMat);
        UsedTextures2D = new List<Texture2D>();
        UsedTextures2D.Add(HistDisplaytexture);


        Utils.matToTexture2D(HistDisplayMat, HistDisplaytexture);
        HistDisplayRenderer.material.mainTexture = HistDisplaytexture;

       

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
        return _heardInited;
    }
    #endregion

    #region UPDATE_NOUPDATE
    //void Update()
    //{
    //    if (IsItPlayingAndUpdated())
    //    {



    //    }
    //}
    #endregion

    #region PrivatMethods
    private void privFunk() { }

    private void SetCamera_fromScreenSize() {

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

    private void SetCamera_from_receivedFrameSize(Mat argFrameMat) {
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

    #region PUBLIC_Methods
    public void PubFunk() { }

    public void Update_HIstogram_for_ThisMat(Mat frame, bool gray) {
        // split the frames in multiple images
        List<Mat> images = new List<Mat>();
        Core.split(frame, images);

        // set the number of bins at 256
        MatOfInt histSize = new MatOfInt(256);
        // only one channel
        MatOfInt channels = new MatOfInt(0);
        // set the ranges
        MatOfFloat histRange = new MatOfFloat(0, 256);

        // compute the histograms for the B, G and R components
        Mat hist_b = new Mat();
        Mat hist_g = new Mat();
        Mat hist_r = new Mat();

        // B component or gray image
        Imgproc.calcHist(images.GetRange(0, 1), channels, new Mat(), hist_b, histSize, histRange, false);

        // G and R components (if the image is not in gray scale)
        if (!gray)
        {
            Imgproc.calcHist(images.GetRange(1, 1), channels, new Mat(), hist_g, histSize, histRange, false);
            Imgproc.calcHist(images.GetRange(2, 1), channels, new Mat(), hist_r, histSize, histRange, false);
        }

        // draw the histogram
        int hist_w = HistDisplayWidth;// 150; // width of the histogram image
        int hist_h = HistDisplayHeight; // height of the histogram image
                                        //  int y= (int)histSize.get(new int[2] { 0, 0 })[0];
        int bin_w = (int)Mathf.RoundToInt(hist_w / (int)histSize.get(new int[2] { 0, 0 })[0]);

        Mat histImage = new Mat(hist_h, hist_w, CvType.CV_8UC3, new Scalar(0, 0, 0));
        // normalize the result to [0, histImage.rows()]
        Core.normalize(hist_b, hist_b, 0, histImage.rows(), Core.NORM_MINMAX, -1, new Mat());

        // for G and R components
        if (!gray)
        {
            Core.normalize(hist_g, hist_g, 0, histImage.rows(), Core.NORM_MINMAX, -1, new Mat());
            Core.normalize(hist_r, hist_r, 0, histImage.rows(), Core.NORM_MINMAX, -1, new Mat());
        }

        // effectively draw the histogram(s)
        for (int i = 1; i < histSize.get(0, 0)[0]; i++)
        {
            Range_[0] = i - 1; Range_[1] = 0;
            Rangeget_[0] = i; Rangeget_[1] = 0;
            int A = 0; int B = 0;
            int g1 = 0; int g2 = 0;
            int r1 = 0; int r2 = 0;
            A = Mathf.RoundToInt((int)hist_b.get(Range_)[0]);
            B = Mathf.RoundToInt((int)hist_b.get(Rangeget_)[0]);
            Imgproc.line(histImage, new Point(bin_w * (i - 1), hist_h - A), new Point(bin_w * (i), hist_h - B), new Scalar(255, 0, 0), 2, 8, 0);
            // G and R components (if the image is not in gray scale)
            if (!gray)
            {

                g1 = Mathf.RoundToInt((int)hist_g.get(Range_)[0]);
                g2 = Mathf.RoundToInt((int)hist_g.get(Rangeget_)[0]);
                r1 = Mathf.RoundToInt((int)hist_r.get(Range_)[0]);
                r2 = Mathf.RoundToInt((int)hist_r.get(Rangeget_)[0]);
                Imgproc.line(histImage, new Point(bin_w * (i - 1), hist_h - g1), new Point(bin_w * (i), hist_h - g2), new Scalar(0, 255, 0), 2, 8, 0);
                Imgproc.line(histImage, new Point(bin_w * (i - 1), hist_h - r1), new Point(bin_w * (i), hist_h - r2), new Scalar(0, 0, 255), 2, 8, 0);
            }
  
        }

        // display the histogram...
        Utils.matToTexture2D(histImage, HistDisplaytexture);
    }
    #endregion



}
