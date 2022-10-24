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
    public bool MatIsGray;

    int[] Range_ = new int[2] { 0, 0 };
    int[] Rangeget_ = new int[2] { 0, 0 };
    int[] Range_B= new int[2] { 0,0};
    int[] Range_G = new int[2] { 0, 0 };
    int[] Range_R = new int[2] { 0, 0 };

    int[] Range_Bget = new int[2] { 0, 0 };
    int[] Range_Gget = new int[2] { 0, 0 };
    int[] Range_Rget = new int[2] { 0, 0 };

    Rect Roirect;

    public int MedoanBlur=10;
    public double doctorPepper =  2;
    public double minimalDistance  =150;
    public double Parmisan1 =  152.28;
    public double Parmisan2 =   61.2;
      int minimalRAdius = 40;//45;
      int maximalRAdius = 45;//56;
    public int circlethicknessoffset = 2;
    Mat circles;
    Mat grayMatcircles;
    Point[] cirCeners;
    List<Point> FoundCurCircles;
    List<double> Found_rhos;
    // RectsAndPointsMaker p_maker;
   // MatsOfROICoordinates m_coor;
    public bool PerspectiveOn;
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
        // p_maker = gameObject.GetComponent<RectsAndPointsMaker>();
      //  m_coor = gameObject.GetComponent<MatsOfROICoordinates>();
         HistDisplayRenderer = HistoDisplayObj.GetComponent<Renderer>();
        HistDisplayWidth = (int)HistoDisplayObj.transform.localScale.x;
        HistDisplayHeight = (int)HistoDisplayObj.transform.localScale.y;
        //webCamTextureToMatHelper.InitializeMePleaze();
        circles = new Mat();
        grayMatcircles = new Mat();
        minimalRAdius = radius - circlethicknessoffset;//45;
          maximalRAdius = radius + circlethicknessoffset;//56;
        FoundCurCircles = new List<Point>();
        Found_rhos = new List<double>();
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

            Imgproc.line(histImage, new Point(bin_w * (i - 1), hist_h - A),new Point(bin_w * (i), hist_h - B), new Scalar(255, 0, 0), 2, 8, 0);
            // G and R components (if the image is not in gray scale)
            if (!gray)
            {


                g1 = Mathf.RoundToInt((int)hist_g.get(Range_)[0]);
                g2 = Mathf.RoundToInt((int)hist_g.get(Rangeget_)[0]);

                r1 = Mathf.RoundToInt((int)hist_r.get(Range_)[0]);
                r2 = Mathf.RoundToInt((int)hist_r.get(Rangeget_)[0]);

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
            if (MatIsGray) {
                Imgproc.cvtColor(arena, arena, Imgproc.COLOR_BGR2GRAY);
            }
            FindCircle(Untemperedoriginal);
            showHistogram(arena, MatIsGray);
           // arena.copyTo(Untemperedoriginal.submat(Roirect));
            Utils.matToTexture2D(MatToShow, texture);
            _NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly.Dispose();


        }

    }
    void Update()
    {
        if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
        {
            FoundCurCircles.Clear();
            Found_rhos.Clear();
            Mat fullMat= webCamTextureToMatHelper.GetMat();




            if (PerspectiveOn)
            {
            //    Mat perspectiveTransform = Imgproc.getPerspectiveTransform(p_maker.GetSrcPTS(), p_maker.GetDstPTS());
             //   Imgproc.warpPerspective(fullMat, fullMat, perspectiveTransform, new Size(fullMat.cols(), fullMat.rows()));
            }
            DrawPointsOn(fullMat);

            Utils.matToTexture2D(fullMat, texture);


        }
    }

    void FindCircle(Mat argMat) {

        minimalRAdius = radius - circlethicknessoffset;//45;
        maximalRAdius = radius + circlethicknessoffset;//56;
        Mat grayMatcircles = new Mat();
        Imgproc.cvtColor(argMat, grayMatcircles, Imgproc.COLOR_RGBA2GRAY);

        // Imgproc.GaussianBlur(arena, arena, new Size(PublicBlurArenaFacro, PublicBlurArenaFacro), PublicBlurArenaSigma);

        Imgproc.medianBlur(grayMatcircles, grayMatcircles, MedoanBlur);
        Mat circles = new Mat();

        Imgproc.HoughCircles(grayMatcircles, circles, Imgproc.CV_HOUGH_GRADIENT, doctorPepper, minimalDistance, Parmisan1, Parmisan2, minimalRAdius, maximalRAdius);
        // Imgproc.HoughCircles(grayMat, circles, Imgproc.CV_HOUGH_GRADIENT, 2, 10, 160, 50, 10, 40);
        Point ptCircle = new Point();
        int numberOfCirclesFonud = 1;// circles.cols();
        // Debug.Log("found " + numberOfCirclesFonud);
        if (circles.cols() > 1)
        {
            Debug.Log("found " + circles.cols());
            for (int i = 0; i < numberOfCirclesFonud; i++)
            {
                double[] data = circles.get(0, i);
                pt.x = data[0];
                pt.y = data[1];
                //ptCircle.x = data[0] + ARoiX;
                //  ptCircle.y = data[1] + ARoiY;
                //  double rho = data[2];
                  Imgproc.circle(argMat, ptCircle, radius, new Scalar(10, 10, 100, 255), 5);
                //minimap.UpdateLocationSimpleVomit_Enemies(ptCircle.x, ptCircle.y);
            }
        }
        Utils.matToTexture2D(argMat, texture);
        // circles.Dispose();
        //grayMatcircles.Dispose();
    }

    void DrawPointsOn(Mat argMat) {

        //Imgproc.circle(argMat, p_maker.o_ptl, 10, new Scalar(255, 0, 0, 255), 2);
        //Imgproc.circle(argMat, p_maker.o_ptr, 10, new Scalar(255, 0, 0, 255), 2);
        //Imgproc.circle(argMat, p_maker.o_pbr, 10, new Scalar(255, 0, 0, 255), 2);
        //Imgproc.circle(argMat, p_maker.o_pbl, 10, new Scalar(255, 0, 0, 255), 2);

        //Imgproc.circle(argMat, p_maker.correct_ptl, 10, new Scalar(255, 200, 0, 255), 2);
        //Imgproc.circle(argMat, p_maker.correct_ptr, 10, new Scalar(255, 200, 0, 255), 2);
        //Imgproc.circle(argMat, p_maker.correct_pbr, 10, new Scalar(255, 200, 0, 255), 2);
        //Imgproc.circle(argMat, p_maker.correct_pbl, 10, new Scalar(255, 200, 0, 255), 2);

        //Imgproc.drawMarker(argMat, p_maker.p0, new Scalar(255, 200, 0, 255), 2);
        //Imgproc.drawMarker(argMat, p_maker.p1, new Scalar(255, 200, 0, 255), Imgproc.MARKER_CROSS);
        //Imgproc.line(argMat, p_maker.p2, p_maker.p3,  new Scalar(255, 200, 0, 255), 2);

        //Imgproc.line(argMat, p_maker.PTrim_tl, p_maker.PTrim_bl, new Scalar(255, 200, 0, 255), 2);
        //Imgproc.line(argMat, p_maker.PTrim_tr, p_maker.PTrim_br, new Scalar(255, 200, 0, 255), 2);

        //Imgproc.rectangle(argMat, p_maker.Rect_TrimmedPerspective, new Scalar(255, 200, 200, 255), 3);
        //Imgproc.rectangle(argMat, p_maker.Rect_TrackLeft, new Scalar(25, 240, 200, 255), 3);

    }
}

/*
 using (Mat circles = new Mat())
            {
                //Imgproc.HoughCircles(grayMat, circles, Imgproc.CV_HOUGH_GRADIENT, 2, 10, 160, 50, 10, 40);
                Imgproc.HoughCircles(grayMat, circles, Imgproc.CV_HOUGH_GRADIENT, doctorPepper, minimalDistance, Parmisan1, Parmisan2, minimalRAdius, maximalRAdius);

                Point ptLOCAL = new Point();

                for (int i = 0; i < circles.cols(); i++)
                {
                    double[] data = circles.get(0, i);
                    ptLOCAL.x = data[0];
                    ptLOCAL.y = data[1];
                    double rho = data[2];
                    Found_rhos.Add(rho);


                    FoundCurCircles.Add(new Point(ptLOCAL.x, ptLOCAL.y));
                }

                //draw foun circles with coresponding rho
                //for (int c = 0; c < FoundCurCircles.Count; c++)
                //{
                //    if (c > 0)
                //    {
                //        break;
                //    }
                //    Debug.Log("first FOund out of " + FoundCurCircles.Count);
                //    Imgproc.circle(fullMat, FoundCurCircles[c], (int)Found_rhos[c], new Scalar(255, 90, 0, 255), 5);
                //}

                //place mask at circle 
                for (int c = 0; c < FoundCurCircles.Count; c++)
                {
                    if (c > 0)
                    {
                        break;
                    }
                    // Debug.Log("first FOund out of " + FoundCurCircles.Count);
                    //  Imgproc.circle(fullMat, FoundCurCircles[c], radius, new Scalar(255, 90, 0, 255), 5);
                    using (Mat whiteSmilyOnCanvas = new Mat(fullMat.rows(), fullMat.cols(), CvType.CV_8U, Scalar.all(0)))
                    {
                        Mat Untemperedoriginal = fullMat.clone();

                        // Draw the circle on that mask (set thickness to -1 to fill the circle)
                        //Imgproc.circle(m, pt, radius, new Scalar(255, 255, 255), thickness, 8, 0);+
                        Imgproc.rectangle(whiteSmilyOnCanvas, new Point(0,0), new Point(frameWidth, frameHeight), new Scalar(0, 0, 0), -1, 8, 0);//half square masking the top of the outkine. leaves a smily

                        Imgproc.circle(whiteSmilyOnCanvas, FoundCurCircles[c], radius, new Scalar(255, 255, 255), -1, 8, 0); //outer disk full white
                        Imgproc.circle(whiteSmilyOnCanvas, FoundCurCircles[c], radius - thickness, new Scalar(0, 0, 0), -1, 8, 0);//innerdisc fulll black
                        Imgproc.rectangle(whiteSmilyOnCanvas, new Point(FoundCurCircles[c].x - radius, FoundCurCircles[c].y), new Point(FoundCurCircles[c].x + radius, pt.y - radius), new Scalar(0, 0, 0), -1, 8, 0);//half square masking the top of the outkine. leaves a smily

                        Mat _NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly = new Mat();
                        fullMat.copyTo(_NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly, whiteSmilyOnCanvas);

.
                        Mat thresh = new Mat();
                        Imgproc.threshold(whiteSmilyOnCanvas, thresh, 1, 255, Imgproc.THRESH_BINARY);


                        Mat arena = Untemperedoriginal.submat(Roirect);
                        if (MatIsGray)
                        {
                            Imgproc.cvtColor(arena, arena, Imgproc.COLOR_BGR2GRAY);
                        }

                        _NEW_BlackCanvas_withStampedPixcelsOnSmilyOnly.Dispose();


                    }

                }


                Utils.matToTexture2D(fullMat, texture);
            }

 */