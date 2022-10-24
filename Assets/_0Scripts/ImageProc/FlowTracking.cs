

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.VideoModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UnityUtils.Helper;
using Rect = OpenCVForUnity.CoreModule.Rect;
using OpenCVForUnityExample;

public class FlowTracking : MonoBehaviour
{
    /*
           * param image Input 8-bit or floating-point 32-bit, single-channel image.
           * param corners Output vector of detected corners.
           * param maxCorners Maximum number of corners to return. If there are more corners than are found, the strongest of them is returned.
           * { code maxCorners &lt;= 0}  implies that no limit on the maximum is set and all detected corners are returned.

           * param qualityLevel Parameter characterizing the minimal accepted quality of image corners.
           * The parameter value is multiplied by the best corner quality measure, which is the minimal eigenvalue
           * (see #cornerMinEigenVal ) or the Harris function response (see #cornerHarris ). The corners with the
           * quality measure less than the product are rejected. For example, if the best corner has the
           * quality measure = 1500, and the qualityLevel= 0.01, then all the corners with the quality measure
           * less than 15 are rejected.
           * param minDistance Minimum possible Euclidean distance between the returned corners.
           * CV_8UC1 and the same size as image ), it specifies the region in which the corners are detected.
           * pixel neighborhood.See cornerEigenValsAndVecs.
           * or #cornerMinEigenVal.
            */

    public int Pub_MaxCorners = 40;
    public double Pub_Quality = 0.025;
    public int Pub_MinDist = 20;
    /// <summary>
    /// The mat op flow this.
    /// </summary>
    Mat matOpFlowThis;

    /// <summary>
    /// The mat op flow previous.
    /// </summary>
    Mat matOpFlowPrev;

    /// <summary>
    /// The i GFFT max.
    /// </summary>
    int iGFFTMax = 40;
    double iGFQuality = 0.05;
    double iGFMinDist = 20;

    public double RoiX = 1102.6;
    public double RoiY = 289.2;
    public double RoiW = 70.6;
    public double RoiH = 409;


    public double PRoiX = 230;
    public double PRoiY = 354;
    public double PRoiW = 820;
    public double PRoiH = 96;

    public double ARoiX = 220.8;
    public double ARoiY = 125.1;
    public double ARoiW = 840;
    public double ARoiH = 580;

    Rect RectTestMeasure;
    public double TRoiX = 0;
    public double TRoiY = 0;
    public double TRoiW = 100;
    public double TRoiH = 100;
    int frameWidth, frameHeight;
    Rect RectRoi;
    Scalar RoiDims;

    Rect RectPlayerarea;

    Rect RectArena;
    public double doctorPepper = 1; //2;
    public double minimalDistance = 50;//150;
    public double Parmisan1 = 8;//152.28;
    public double Parmisan2 = 20;//61.2;
    public int minimalRAdius = 40;//45;
    public int maximalRAdius = 45;//56;
    int _margin = 0;
    int MaxCirclesToTrack = 6; //figured 3v3 , and a way to eliminate myself or us that to haar 
    Point[] cirCeners;
    int numberOfPointsToPutInVolatileArray = 0;
    Point[] VolatileCircs;
    Mat circles;
    Mat grayMatcircles;
    /// <summary>
    /// The MO pcorners.
    /// </summary>
    MatOfPoint MOPcorners;

    /// <summary>
    /// The m MO p2fpts this.
    /// </summary>
    MatOfPoint2f mMOP2fptsThis;

    /// <summary>
    /// The m MO p2fpts previous.
    /// </summary>
    MatOfPoint2f mMOP2fptsPrev;

    /// <summary>
    /// The m MO p2fpts safe.
    /// </summary>
    MatOfPoint2f mMOP2fptsSafe;

    /// <summary>
    /// The m MOB status.
    /// </summary>
    MatOfByte mMOBStatus;

    /// <summary>
    /// The m MO ferr.
    /// </summary>
    MatOfFloat mMOFerr;

    /// <summary>
    /// The color red.
    /// </summary>
    Scalar colorRed = new Scalar(255, 0, 0, 255);

    /// <summary>
    /// The i line thickness.
    /// </summary>
    int iLineThickness = 3;

    /// <summary>
    /// The texture.
    /// </summary>
    Texture2D texture;

    /// <summary>
    /// The web cam texture to mat helper.
    /// </summary>
    ndicamTextureTomatEventHelper webCamTextureToMatHelper;

    public double CumulAvrYdisp;

    double NumPoints;
    double cumulativeYdisplacementForFrame;
    double AVRYdisplacementForFrame;

    public MinimapTest minimap;







    const int MAX_NUM_OBJECTS = 50;

    const int MIN_OBJECT_AREA = 20 * 20;


    Mat rgbMat;
    Mat thresholdMat;
    Mat hsvMat;



    ColorObject mygreen = new ColorObject("myPlayer");
    /*
     setHSVmin(new Scalar(60, 82, 82));
            setHSVmax(new Scalar(68, 255, 255));
     */
    public Scalar PublicScalarmin = new Scalar(148, 82, 82, 3);
    public Scalar PublicScalarMax = new Scalar(151, 255, 255, 8);

    public double PublicBlurFacro = 15;
    public double PublicBlurSigma = 0;

    public int Order = 0;

    public bool DoDialate = false;
    public bool DoBLur = true;
    public bool DoEnrode = false;
    //50 0 167.2 2    59 248.2 255 10

    public double PublicBlurArenaFacro = 15;
    public double PublicBlurArenaSigma = 0;

    public int MedianBlur = 20;



    private void OnEnable()
    {
        EventsManagerLib.On_NDI_startedStreaming += OnWebCamTextureToMatHelperInitialized;
        EventsManagerLib.On_ndi_Dispos += OnWebCamTextureToMatHelperDisposed;
        EventsManagerLib.On_ndi_Error += OnWebCamTextureToMatHelperErrorOccurred;
        EventsManagerLib.On_reTrack += ResetTrackingVertical;
    }
    private void OnDisable()
    {
        EventsManagerLib.On_NDI_startedStreaming -= OnWebCamTextureToMatHelperInitialized;
        EventsManagerLib.On_ndi_Dispos -= OnWebCamTextureToMatHelperDisposed;
        EventsManagerLib.On_ndi_Error -= OnWebCamTextureToMatHelperErrorOccurred;
        EventsManagerLib.On_reTrack -= ResetTrackingVertical;
    }


    // Use this for initialization
    void Start()
    {


        circles = new Mat();
        grayMatcircles = new Mat();

        RectRoi = new Rect((int)RoiX, (int)RoiY, (int)RoiW, (int)RoiH);
        RectPlayerarea = new Rect((int)PRoiX, (int)PRoiY, (int)PRoiW, (int)PRoiH);
        RectArena = new Rect((int)ARoiX, (int)ARoiY, (int)ARoiW, (int)ARoiH);
        RectTestMeasure= new Rect((int)TRoiX, (int)TRoiY, (int)TRoiW, (int)TRoiH);
        RoiDims = new Scalar(0, 0, 0, 0);


        cirCeners = new Point[MaxCirclesToTrack];

        for (int p = 0; p < MaxCirclesToTrack; p++)
        {
            cirCeners[p] = new Point(0, 0);
        }




        webCamTextureToMatHelper = gameObject.GetComponent<ndicamTextureTomatEventHelper>();


        //  webCamTextureToMatHelper.InitializeMePleaze();
    }

    /// <summary>
    /// Raises the web cam texture to mat helper initialized event.
    /// </summary>
    public void OnWebCamTextureToMatHelperInitialized(int argW, int argH)
    {
        Debug.Log("OnWebCamTextureToMatHelperInitialized");

        Mat webCamTextureMat = webCamTextureToMatHelper.GetMat();

        frameWidth = webCamTextureMat.cols();
        frameHeight = webCamTextureMat.rows();
        texture = new Texture2D(webCamTextureMat.cols(), webCamTextureMat.rows(), TextureFormat.RGBA32, false);
        Utils.matToTexture2D(webCamTextureMat, texture);

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

        matOpFlowThis = new Mat();
        matOpFlowPrev = new Mat();
        MOPcorners = new MatOfPoint();
        mMOP2fptsThis = new MatOfPoint2f();
        mMOP2fptsPrev = new MatOfPoint2f();
        mMOP2fptsSafe = new MatOfPoint2f();
        mMOBStatus = new MatOfByte();
        mMOFerr = new MatOfFloat();



        rgbMat = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC3);
        thresholdMat = new Mat();
        hsvMat = new Mat();

    }

    /// <summary>
    /// Raises the web cam texture to mat helper disposed event.
    /// </summary>
    public void OnWebCamTextureToMatHelperDisposed()
    {
        Debug.Log("OnWebCamTextureToMatHelperDisposed");

        if (texture != null)
        {
            Texture2D.Destroy(texture);
            texture = null;
        }

        if (matOpFlowThis != null)
            matOpFlowThis.Dispose();
        if (matOpFlowPrev != null)
            matOpFlowPrev.Dispose();
        if (MOPcorners != null)
            MOPcorners.Dispose();
        if (mMOP2fptsThis != null)
            mMOP2fptsThis.Dispose();
        if (mMOP2fptsPrev != null)
            mMOP2fptsPrev.Dispose();
        if (mMOP2fptsSafe != null)
            mMOP2fptsSafe.Dispose();
        if (mMOBStatus != null)
            mMOBStatus.Dispose();
        if (mMOFerr != null)
            mMOFerr.Dispose();


        if (rgbMat != null)
            rgbMat.Dispose();
        if (thresholdMat != null)
            thresholdMat.Dispose();
        if (hsvMat != null)
            hsvMat.Dispose();

        if (circles != null)
            circles.Dispose();
        if (grayMatcircles != null)
            grayMatcircles.Dispose();
    }

    /// <summary>
    /// Raises the web cam texture to mat helper error occurred event.
    /// </summary>
    /// <param name="errorCode">Error code.</param>
    public void OnWebCamTextureToMatHelperErrorOccurred(int errorCode)
    {
        //WebCamTextureToMatHelper.ErrorCode
        Debug.Log("OnWebCamTextureToMatHelperErrorOccurred " + errorCode);
    }

    // Update is called once per frame
    void Update()
    {
        if (Pub_MaxCorners < 1) Pub_MaxCorners = 1;
        if (Pub_MaxCorners > 500) Pub_MaxCorners = 500;

        if (Pub_Quality < 0.001) Pub_Quality = 0.001;
        if (Pub_Quality > 1) Pub_Quality = 1;

        if (Pub_MinDist < 1) Pub_MinDist = 1;
        if (Pub_MinDist > 1000) Pub_MinDist = 1000;



        RectRoi.x = (int)RoiX;
        RectRoi.y = (int)RoiY;
        RectRoi.width = (int)RoiW;
        RectRoi.height = (int)RoiH;

        RectPlayerarea.x = (int)PRoiX;
        RectPlayerarea.y = (int)PRoiY;
        RectPlayerarea.width = (int)PRoiW;
        RectPlayerarea.height = (int)PRoiH;

        RectArena.x = (int)ARoiX;
        RectArena.y = (int)ARoiY;
        RectArena.width = (int)ARoiW;
        RectArena.height = (int)ARoiH;


        RectTestMeasure.x = (int)TRoiX;
        RectTestMeasure.y = (int)TRoiY;
        RectTestMeasure.width = (int)TRoiW;
        RectTestMeasure.height = (int)TRoiH;
        

        iGFFTMax = Pub_MaxCorners;
        iGFQuality = Pub_Quality;
        iGFMinDist = Pub_MinDist;

        if (webCamTextureToMatHelper.IsPlaying() && webCamTextureToMatHelper.DidUpdateThisFrame())
        {
            Mat original = webCamTextureToMatHelper.GetMat();
            Mat rgbaMat = original.submat(RectRoi);
            Mat rgbp = original.submat(RectPlayerarea);
            Mat arena = original.submat(RectArena);


            Imgproc.cvtColor(arena, grayMatcircles, Imgproc.COLOR_RGBA2GRAY);

            // Imgproc.GaussianBlur(arena, arena, new Size(PublicBlurArenaFacro, PublicBlurArenaFacro), PublicBlurArenaSigma);

            Imgproc.medianBlur(arena, arena, MedianBlur);
            circles = new Mat();

            Imgproc.HoughCircles(grayMatcircles, circles, Imgproc.CV_HOUGH_GRADIENT, doctorPepper, minimalDistance, Parmisan1, Parmisan2, minimalRAdius, maximalRAdius);
            // Imgproc.HoughCircles(grayMat, circles, Imgproc.CV_HOUGH_GRADIENT, 2, 10, 160, 50, 10, 40);
            Point ptCircle = new Point();
            int numberOfCirclesFonud = circles.cols();
            // Debug.Log("found " + numberOfCirclesFonud);
            for (int i = 0; i < numberOfCirclesFonud; i++)
            {
                double[] data = circles.get(0, i);
                ptCircle.x = data[0] + ARoiX;
                ptCircle.y = data[1] + ARoiY;
                double rho = data[2];
                Imgproc.circle(original, ptCircle, (int)rho, new Scalar(10, 10, 100, 255), 5);
                minimap.UpdateLocationSimpleVomit_Enemies(ptCircle.x, ptCircle.y);
            }


            //Imgproc.putText (rgbaMat, "W:" + rgbaMat.width () + " H:" + rgbaMat.height () + " SO:" + Screen.orientation, new Point (5, rgbaMat.rows () - 10), Imgproc.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar (255, 255, 255, 255), 2, Imgproc.LINE_AA, false);

            // Utils.matToTexture2D(rgbaMat, texture, webCamTextureToMatHelper.GetBufferColors());










            if (mMOP2fptsPrev.rows() == 0)
            {

                // first time through the loop so we need prev and this mats
                // plus prev points
                // get this mat
                Imgproc.cvtColor(rgbaMat, matOpFlowThis, Imgproc.COLOR_RGBA2GRAY);

                // copy that to prev mat
                matOpFlowThis.copyTo(matOpFlowPrev);

                // get prev corners
                Imgproc.goodFeaturesToTrack(matOpFlowPrev, MOPcorners, iGFFTMax, 0.05, 20);
                mMOP2fptsPrev.fromArray(MOPcorners.toArray());

                // get safe copy of this corners
                mMOP2fptsPrev.copyTo(mMOP2fptsSafe);
            }
            else
            {
                // we've been through before so
                // this mat is valid. Copy it to prev mat
                matOpFlowThis.copyTo(matOpFlowPrev);

                // get this mat
                Imgproc.cvtColor(rgbaMat, matOpFlowThis, Imgproc.COLOR_RGBA2GRAY);

                // get the corners for this mat
                Imgproc.goodFeaturesToTrack(matOpFlowThis, MOPcorners, iGFFTMax, 0.05, 20);
                mMOP2fptsThis.fromArray(MOPcorners.toArray());

                // retrieve the corners from the prev mat
                // (saves calculating them again)
                mMOP2fptsSafe.copyTo(mMOP2fptsPrev);

                // and save this corners for next time through

                mMOP2fptsThis.copyTo(mMOP2fptsSafe);
            }



            /*
                Parameters:
                    prevImg first 8-bit input image
                    nextImg second input image
                    prevPts vector of 2D points for which the flow needs to be found; point coordinates must be single-precision floating-point numbers.
                    nextPts output vector of 2D points (with single-precision floating-point coordinates) containing the calculated new positions of input features in the second image; when OPTFLOW_USE_INITIAL_FLOW flag is passed, the vector must have the same size as in the input.
                    status output status vector (of unsigned chars); each element of the vector is set to 1 if the flow for the corresponding features has been found, otherwise, it is set to 0.
                    err output vector of errors; each element of the vector is set to an error for the corresponding feature, type of the error measure can be set in flags parameter; if the flow wasn't found then the error is not defined (use the status parameter to find such cases).
            */
            Video.calcOpticalFlowPyrLK(matOpFlowPrev, matOpFlowThis, mMOP2fptsPrev, mMOP2fptsThis, mMOBStatus, mMOFerr);

            int PointsFound = 0;
            double Ydisp_loopCumul = 0;
            if (mMOBStatus.rows() > 0)
            {
                List<Point> cornersPrev = mMOP2fptsPrev.toList();
                List<Point> cornersThis = mMOP2fptsThis.toList();
                List<byte> byteStatus = mMOBStatus.toList();

                int x = 0;
                int y = byteStatus.Count - 1;

                for (x = 0; x < y; x++)
                {
                    if (byteStatus[x] == 1)
                    {
                        PointsFound++;


                        Point pt = cornersThis[x];
                        Point pt2 = cornersPrev[x];

                        double Ydisp = pt2.y - pt.y;

                        Ydisp_loopCumul += Ydisp;

                        Imgproc.circle(rgbaMat, pt, 5, colorRed, iLineThickness - 1);

                        Imgproc.line(rgbaMat, pt, pt2, colorRed, iLineThickness);
                    }
                }

                cumulativeYdisplacementForFrame = Ydisp_loopCumul;
                AVRYdisplacementForFrame = Ydisp_loopCumul / PointsFound;
                CumulAvrYdisp += AVRYdisplacementForFrame;

                minimap.UpdateLocationSimpleVomit(CumulAvrYdisp);

            }

            //Imgproc.putText (rgbaMat, "W:" + rgbaMat.width () + " H:" + rgbaMat.height () + " SO:" + Screen.orientation, new Point (5, rgbaMat.rows () - 10), Imgproc.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar (255, 255, 255, 255), 2, Imgproc.LINE_AA, false);






            //Creating destination matrix
            Mat dst = new Mat(rgbp.rows(), rgbp.cols(), rgbp.type());

            if (DoBLur)
            {
                //Applying GaussianBlur on the Image
                Imgproc.GaussianBlur(rgbp, rgbp, new Size(PublicBlurFacro, PublicBlurFacro), PublicBlurSigma);
            }
            Mat erodeElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(PublicScalarmin.val[3], PublicScalarmin.val[3]));
            //dilate with larger element so make sure object is nicely visible
            Mat dilateElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(PublicScalarMax.val[3], PublicScalarMax.val[3]));
            if (DoEnrode)
            {
                Imgproc.erode(rgbp, rgbp, erodeElement);
                Imgproc.erode(rgbp, rgbp, erodeElement);
            }

            if (DoDialate)
            {
                Imgproc.dilate(rgbp, rgbp, dilateElement);
                Imgproc.dilate(rgbp, rgbp, dilateElement);
            }
            Imgproc.cvtColor(rgbaMat, rgbMat, Imgproc.COLOR_RGBA2RGB);

            Imgproc.cvtColor(rgbp, hsvMat, Imgproc.COLOR_RGB2HSV);
            Core.inRange(hsvMat, PublicScalarmin, PublicScalarMax, thresholdMat);
            morphOps(thresholdMat);
            trackFilteredObject(mygreen, thresholdMat, hsvMat, rgbp);









            rgbp.copyTo(original.submat(RectRoi));







            rgbaMat.copyTo(original.submat(RectPlayerarea));
            Imgproc.rectangle(original, RectRoi, new Scalar(125, 200, 190), 3);

            Imgproc.rectangle(original, RectPlayerarea, new Scalar(200, 200, 0), 3);
            
            Imgproc.rectangle(original, RectArena, new Scalar(0, 200, 200), 3);

            Imgproc.rectangle(original, RectTestMeasure, new Scalar(255, 000,000), 1);

            Utils.matToTexture2D(original, texture);
        }
    }


    public void ResetTrackingVertical()
    {
        Debug.Log("REEESETING");
        CumulAvrYdisp = 0;
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy()
    {
        webCamTextureToMatHelper.Dispose();
    }


    double cumulativex, cumulativey, AVRx, AVRy;
    double NumOfObjects;

    private void drawObject(List<ColorObject> theColorObjects, Mat frame, Mat temp, List<MatOfPoint> contours, Mat hierarchy)
    {

        NumOfObjects = theColorObjects.Count;
        cumulativex = 0;
        cumulativey = 0;
        AVRx = 0;
        AVRy = 0;
        for (int i = 0; i < theColorObjects.Count; i++)
        {
            Imgproc.drawContours(frame, contours, i, theColorObjects[i].getColor(), 3, 8, hierarchy, int.MaxValue, new Point());
            Imgproc.circle(frame, new Point(theColorObjects[i].getXPos(), theColorObjects[i].getYPos()), 5, theColorObjects[i].getColor());
            //  Imgproc.putText(frame, theColorObjects[i].getXPos() + " , " + theColorObjects[i].getYPos(), new Point(theColorObjects[i].getXPos(), theColorObjects[i].getYPos() + 20), 1, 1, theColorObjects[i].getColor(), 2);
            Imgproc.putText(frame, theColorObjects[i].getType(), new Point(theColorObjects[i].getXPos(), theColorObjects[i].getYPos() - 20), 1, 2, theColorObjects[i].getColor(), 2);
            cumulativex += theColorObjects[i].getXPos();
            cumulativey += theColorObjects[i].getYPos();
        }

        AVRx = cumulativex / NumOfObjects;
        AVRy = cumulativey / NumOfObjects;
        minimap.UpdateLocationSimpleVomit_Player(AVRx, AVRy);
    }

    /// <summary>
    /// Morphs the ops.
    /// </summary>
    /// <param 
    /// ="thresh">Thresh.</param>
    private void morphOps(Mat thresh)
    {
        //create structuring element that will be used to "dilate" and "erode" image.
        //the element chosen here is a 3px by 3px rectangle
        Mat erodeElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(PublicScalarmin.val[3], PublicScalarmin.val[3]));
        //dilate with larger element so make sure object is nicely visible
        Mat dilateElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(PublicScalarMax.val[3], PublicScalarMax.val[3]));

        Imgproc.erode(thresh, thresh, erodeElement);
        Imgproc.erode(thresh, thresh, erodeElement);

        Imgproc.dilate(thresh, thresh, dilateElement);
        Imgproc.dilate(thresh, thresh, dilateElement);
    }

    /// <summary>
    /// Tracks the filtered object.
    /// </summary>
    /// <param name="theColorObject">The color object.</param>
    /// <param name="threshold">Threshold.</param>
    /// <param name="HSV">HS.</param>
    /// <param name="cameraFeed">Camera feed.</param>
    private void trackFilteredObject(ColorObject theColorObject, Mat threshold, Mat HSV, Mat cameraFeed)
    {

        List<ColorObject> colorObjects = new List<ColorObject>();
        Mat temp = new Mat();
        threshold.copyTo(temp);
        //these two vectors needed for output of findContours
        List<MatOfPoint> contours = new List<MatOfPoint>();
        Mat hierarchy = new Mat();
        //find contours of filtered image using openCV findContours function
        Imgproc.findContours(temp, contours, hierarchy, Imgproc.RETR_CCOMP, Imgproc.CHAIN_APPROX_SIMPLE);

        //use moments method to find our filtered object
        bool colorObjectFound = false;
        if (hierarchy.rows() > 0)
        {
            int numObjects = hierarchy.rows();

            //                      Debug.Log("hierarchy " + hierarchy.ToString());

            //if number of objects greater than MAX_NUM_OBJECTS we have a noisy filter
            if (numObjects < MAX_NUM_OBJECTS)
            {
                for (int index = 0; index >= 0; index = (int)hierarchy.get(0, index)[0])
                {

                    Moments moment = Imgproc.moments(contours[index]);
                    double area = moment.get_m00();

                    //if the area is less than 20 px by 20px then it is probably just noise
                    //if the area is the same as the 3/2 of the image size, probably just a bad filter
                    //we only want the object with the largest area so we safe a reference area each
                    //iteration and compare it to the area in the next iteration.
                    if (area > MIN_OBJECT_AREA)
                    {

                        ColorObject colorObject = new ColorObject();

                        colorObject.setXPos((int)(moment.get_m10() / area));
                        colorObject.setYPos((int)(moment.get_m01() / area));
                        colorObject.setType(theColorObject.getType());
                        colorObject.setColor(theColorObject.getColor());

                        colorObjects.Add(colorObject);

                        colorObjectFound = true;

                    }
                    else
                    {
                        colorObjectFound = false;
                    }
                }
                //let user know you found an object
                if (colorObjectFound == true)
                {
                    //draw object location on screen
                    drawObject(colorObjects, cameraFeed, temp, contours, hierarchy);
                }

            }
            else
            {
                Imgproc.putText(cameraFeed, "TOO MUCH NOISE!", new Point(5, cameraFeed.rows() - 10), Imgproc.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar(255, 255, 255, 255), 2, Imgproc.LINE_AA, false);
            }
        }
    }

}