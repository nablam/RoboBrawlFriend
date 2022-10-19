using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnityExample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rect = OpenCVForUnity.CoreModule.Rect;

public class MaskTester : MonoBehaviour
{


    ndicamTextureTomatEventHelper webCamTextureToMatHelper;
    int frameWidth, frameHeight;
    int HistDisplayWidth, HistDisplayHeight;
    Mat rgbMat;
    Mat grayMat;
    Mat maskmat;
    Mat HistDisplayMat;
    Texture2D texture;
    Texture2D texture2;
    Texture2D HistDisplaytexture;
    List<Mat> UsedMats;
    List<Texture2D> UsedTextures2D;
    public Point pt ;
    public int radius = 80;
    public int thickness = 12;
    public bool ShowOriginal;
    public bool ShowMasked;

    public GameObject HistoDisplayObj;
    Renderer HistDisplayRenderer;

    int[] Range_ = new int[2] { 0, 0 };
    int[] Rangeget_ = new int[2] { 0, 0 };
    int[] Range_B= new int[2] { 0,0};
    int[] Range_G = new int[2] { 0, 0 };
    int[] Range_R = new int[2] { 0, 0 };

    int[] Range_Bget = new int[2] { 0, 0 };
    int[] Range_Gget = new int[2] { 0, 0 };
    int[] Range_Rget = new int[2] { 0, 0 };

    Rect Roirect;
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

    void Start()
    {
        webCamTextureToMatHelper = gameObject.GetComponent<ndicamTextureTomatEventHelper>();
        HistDisplayRenderer = HistoDisplayObj.GetComponent<Renderer>();
        HistDisplayWidth = (int)HistoDisplayObj.transform.localScale.x;
        HistDisplayHeight = (int)HistoDisplayObj.transform.localScale.y;
        webCamTextureToMatHelper.Initialize();
    }
    public void OnWebCamTextureToMatHelperInitialized(int argW, int argH)
    {
        //  Imgproc.rectangle(whiteSmilyOnCanvas, new Point(pt.x - radius, pt.y + radius), new Point(pt.x + radius, pt.y ), new Scalar(255, 255, 255), -1, 8, 0);//half square masking the top of the outkine. leaves a smily


        Roirect = new Rect(new Point(pt.x - radius, pt.y + radius), new Point(pt.x + radius, pt.y));



        Debug.Log("HelperInitialized  histx" + HistDisplayWidth + "y" + HistDisplayHeight);
        HistDisplayMat = new Mat(HistDisplayHeight, HistDisplayWidth, CvType.CV_8UC3, new Scalar(0,0,0));
        HistDisplaytexture = new Texture2D(HistDisplayWidth, HistDisplayHeight, TextureFormat.RGBA32, false);
        
        Utils.matToTexture2D(HistDisplayMat, HistDisplaytexture);
        HistDisplayRenderer.material.mainTexture = HistDisplaytexture;

        Mat webCamTextureMat = webCamTextureToMatHelper.GetMat();

        frameWidth = webCamTextureMat.cols();
        frameHeight = webCamTextureMat.rows();
        texture = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);
        texture2 = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);
     //   Utils.matToTexture2D(webCamTextureMat, texture);
        Utils.matToTexture2D(webCamTextureMat, texture, false);

        gameObject.GetComponent<Renderer>().material.mainTexture = texture;



        gameObject.transform.localScale = new Vector3(webCamTextureMat.cols(), webCamTextureMat.rows(), 1);

        Debug.Log("Screen.width " + Screen.width + " Screen.height " + Screen.height + " Screen.orientation " + Screen.orientation);




        float width = webCamTextureMat.width();
        float height = webCamTextureMat.height();

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

        rgbMat = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC3);
        grayMat = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8SC1);
        maskmat= new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8U, Scalar.all(0));

        UsedMats = new List<Mat>();
        UsedMats.Add(rgbMat);
        UsedMats.Add(grayMat);
        UsedMats.Add(maskmat);
        UsedMats.Add(HistDisplayMat);
        UsedTextures2D = new List<Texture2D>();
        UsedTextures2D.Add(texture);
        UsedTextures2D.Add(texture2);
        UsedTextures2D.Add(HistDisplaytexture);





         

    }


    private void showHistogram(Mat frame, bool gray)
    {
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
        Imgproc.calcHist(images.GetRange(0,1), channels, new Mat(), hist_b, histSize, histRange, false);

        // G and R components (if the image is not in gray scale)
        if (!gray)
        {
            Imgproc.calcHist(images.GetRange(1, 1), channels, new Mat(), hist_g, histSize, histRange, false);
            Imgproc.calcHist(images.GetRange(2, 1), channels, new Mat(), hist_r, histSize, histRange, false);
        }

        // draw the histogram
        int hist_w = HistDisplayWidth ;// 150; // width of the histogram image
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
            //Range_B
            //Range_Bget
            //Range_G
            //Range_Gget 
            //Range_R
            //Range_Rget

            int A= 0; int B = 0;

            int g1 = 0;int g2 = 0;

            int r1 = 0; int r2 = 0;
            A = Mathf.RoundToInt((int)hist_b.get(Range_)[0]);
            B = Mathf.RoundToInt((int)hist_b.get(Rangeget_)[0]);

            g1 = Mathf.RoundToInt((int)hist_g.get(Range_)[0]);
            g2 = Mathf.RoundToInt((int)hist_g.get(Rangeget_)[0]);

            r1 = Mathf.RoundToInt((int)hist_r.get(Range_)[0]);
            r2 = Mathf.RoundToInt((int)hist_r.get(Rangeget_)[0]);
            Imgproc.line(histImage, new Point(bin_w * (i - 1), hist_h - A),new Point(bin_w * (i), hist_h - B), new Scalar(255, 0, 0), 2, 8, 0);
            // G and R components (if the image is not in gray scale)
            if (!gray)
            {
                Imgproc.line(histImage, new Point(bin_w * (i - 1), hist_h - g1),new Point(bin_w * (i), hist_h - g2), new Scalar(0, 255, 0), 2, 8,0);
                Imgproc.line(histImage, new Point(bin_w * (i - 1), hist_h - r1),new Point(bin_w * (i), hist_h - r2), new Scalar(0, 0, 255), 2, 8,0);
            }
            //            Imgproc.line(histImage, new Point(bin_w * (i - 1), hist_h - Mathf.Round(hist_b.get(i - 1, 0)[0])),
            //new Point(bin_w * (i), hist_h - Mathf.Round(hist_b.get(i, 0)[0])), new Scalar(255, 0, 0), 2, 8, 0);
            //            // G and R components (if the image is not in gray scale)
            //            if (!gray)
            //            {
            //                Imgproc.line(histImage, new Point(bin_w * (i - 1), hist_h - Math.round(hist_g.get(i - 1, 0)[0])),
            //                        new Point(bin_w * (i), hist_h - Mathf.Round(hist_g.get(i, 0)[0])), new Scalar(0, 255, 0), 2, 8,
            //                        0);
            //                Imgproc.line(histImage, new Point(bin_w * (i - 1), hist_h - Math.round(hist_r.get(i - 1, 0)[0])),
            //                        new Point(bin_w * (i), hist_h - Mathf.Round(hist_r.get(i, 0)[0])), new Scalar(0, 0, 255), 2, 8,
            //                        0);
            //            }
        }

        // display the histogram...
        //Image histImg = Utils.mat2Image(histImage);
        //updateImageView(histogram, histImg);
        Utils.matToTexture2D(histImage, HistDisplaytexture);

    }

    public void OnWebCamTextureToMatHelperDisposed()
    {
        Debug.Log("OnWebCamTextureToMatHelperDisposed");

        disposeMyMatsAndTExtures();

    }
    void disposeMyMatsAndTExtures() {
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
    private void OnDestroy()
    {
        disposeMyMatsAndTExtures();
    }
    public void OnWebCamTextureToMatHelperErrorOccurred(int errorCode)
    {
        //WebCamTextureToMatHelper.ErrorCode
        Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
    }
    // Update is called once per frame

    void DoMaskOnThisChunk(Mat argMat) {
     
        using (Mat whiteSmilyOnCanvas = new Mat(argMat.rows(), argMat.cols(), CvType.CV_8U, Scalar.all(0)))
        {
            Mat Untemperedoriginal = argMat.clone();

            // Draw the circle on that mask (set thickness to -1 to fill the circle)
            //Imgproc.circle(m, pt, radius, new Scalar(255, 255, 255), thickness, 8, 0);
            Imgproc.circle(whiteSmilyOnCanvas, pt, radius, new Scalar(255, 255, 255), -1, 8, 0); //outer disk full white
            Imgproc.circle(whiteSmilyOnCanvas, pt, radius- thickness, new Scalar(0, 0, 0), -1, 8, 0);//innerdisc fulll black
            Imgproc.rectangle(whiteSmilyOnCanvas,new Point(pt.x- radius, pt.y), new Point(pt.x + radius, pt.y - radius), new Scalar(0, 0,0), -1, 8, 0);//half square masking the top of the outkine. leaves a smily

            Mat _NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly = new Mat();
            argMat.copyTo(_NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly, whiteSmilyOnCanvas);


            Mat thresh = new Mat();
            Imgproc.threshold(whiteSmilyOnCanvas, thresh, 1, 255, Imgproc.THRESH_BINARY);

            if (ShowOriginal) { 
                Utils.matToTexture2D(Untemperedoriginal, texture); 
            } else {

                if (ShowMasked)
                {
                    Utils.matToTexture2D(_NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly, texture);
                }
                else {
                    Utils.matToTexture2D(whiteSmilyOnCanvas, texture);
                }
               
            }
            _NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly.Dispose();


        }

    }

    void DoMaskOnThisChunk_andHisto(Mat argMat)
    {

        using (Mat whiteSmilyOnCanvas = new Mat(argMat.rows(), argMat.cols(), CvType.CV_8U, Scalar.all(0)))
        {
            Mat Untemperedoriginal = argMat.clone();

            // Draw the circle on that mask (set thickness to -1 to fill the circle)
            //Imgproc.circle(m, pt, radius, new Scalar(255, 255, 255), thickness, 8, 0);
             Imgproc.circle(whiteSmilyOnCanvas, pt, radius, new Scalar(255, 255, 255), -1, 8, 0); //outer disk full white
            Imgproc.circle(whiteSmilyOnCanvas, pt, radius - thickness, new Scalar(0, 0, 0), -1, 8, 0);//innerdisc fulll black
           Imgproc.rectangle(whiteSmilyOnCanvas, new Point(pt.x - radius, pt.y), new Point(pt.x + radius, pt.y - radius), new Scalar(0, 0, 0), -1, 8, 0);//half square masking the top of the outkine. leaves a smily
            Roirect.x =(int) pt.x - radius-2; //new Rect(new Point(pt.x - radius, pt.y + radius), new Point(pt.x + radius, pt.y));
            Roirect.y = (int)pt.y -2 ;
            Roirect.width =(int) radius * 2 +4 ;
            Roirect.height = (int)radius + 4 ;

            //Imgproc.rectangle(whiteSmilyOnCanvas, Roirect, new Scalar(255, 0, 0), 1);
            Mat _NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly = new Mat();
            argMat.copyTo(_NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly, whiteSmilyOnCanvas);


            Mat thresh = new Mat();
            Imgproc.threshold(whiteSmilyOnCanvas, thresh, 1, 255, Imgproc.THRESH_BINARY);

            Mat MatToShow = Untemperedoriginal;

            if (ShowOriginal)
            {
                MatToShow = Untemperedoriginal;
            }
            else
            {

                if (ShowMasked)
                {
                    MatToShow = _NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly; 
                }
                else
                {
                    MatToShow = whiteSmilyOnCanvas;
                }

            }

            Mat arena = Untemperedoriginal.submat(Roirect);
            showHistogram(arena, false);
           // arena.copyTo(Untemperedoriginal.submat(Roirect));
            Utils.matToTexture2D(MatToShow, texture);
            _NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly.Dispose();


        }

    }
    void Update()
    {
        if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
        {
              Mat fullMat= webCamTextureToMatHelper.GetMat();
            DoMaskOnThisChunk_andHisto(fullMat);

            //showHistogram(fullMat, false);
            //Imgproc.circle(HistDisplayMat, new Point(100, 100), 20, new Scalar(255, 0, 0), 10);
           // Utils.matToTexture2D(HistDisplayMat, HistDisplaytexture, false );
        }
    }
}