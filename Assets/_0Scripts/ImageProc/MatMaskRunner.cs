using OpenCVForUnity.CoreModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnityExample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    #endregion

    #region Public_Vars
    public int pubX;
    public HIstogramHandler _histoDisplayer;
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
        webCamTextureToMatHelper = GetComponent<ndicamTextureTomatEventHelper>();
        DisplayQuad = this.gameObject;
        DisplayRenderer= DisplayQuad.GetComponent<Renderer>();

    }
    void Start()
    {
        webCamTextureToMatHelper.Initialize();

    }
    private void OnDestroy()
    {
        disposeMyMatsAndTExtures();
    }
    #endregion

    #region EventHAndlers
    public void OnWebCamTextureToMatHelperInitialized(int argW, int argH)
    {

        Debug.Log("HEARD Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);
 
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
            Utils.matToTexture2D(curmat, texture);

        }
    }
    #endregion

    #region PrivatMethods
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

    #region PUBLIC_Methods
    public void PubFunk() { }
    #endregion
}
