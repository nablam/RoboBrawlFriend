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
public class ShellyBlobDetector : MonoBehaviour
{
    const int MAX_NUM_OBJECTS = 10;
    const int MIN__Green_OBJECT_AREA = 600;
    const int MAX__Green_OBJECT_AREA = 2100;

    const int MIN__purpOBJECT_AREA = 198;
    const int MAX__purpOBJECT_AREA = 600;

    int _cur_min_colorArea, cur_max_coloArea;
    Mat rgbMat;
    Mat thresholdMat;
    Mat hsvMat;
    ColorObject ShellyPurplHair = new ColorObject("green");
    ColorObject justgreen = new ColorObject("myPlayer");//myPlayer






    double cumulativex, cumulativey, AVRx, AVRy;
    double NumOfObjects;

    public double low, high, botmaxsatr, topmaxsat;
    public void InitializeBoloer(int argWidth, int argHeight) {

        //rgbMat = new Mat(webCamTextureMat.rows(), webCamTextureMat.cols(), CvType.CV_8UC3);
        rgbMat = new Mat(argHeight, argWidth, CvType.CV_8UC3);
        thresholdMat = new Mat();
        hsvMat = new Mat();
        //_cur_min_colorArea = MIN__Green_OBJECT_AREA;
        //cur_max_coloArea = MAX__Green_OBJECT_AREA;

        _cur_min_colorArea = MIN__purpOBJECT_AREA;
        cur_max_coloArea = MAX__purpOBJECT_AREA;
        minScalr = new Scalar(low, botmaxsatr, botmaxsatr);
        maxScalar = new Scalar(high, topmaxsat, topmaxsat);
    }

    Scalar minScalr, maxScalar;


    public void DoRunBloberGreen(Mat rgbaMat)
    {
        minScalr = new Scalar(low, botmaxsatr, botmaxsatr);
        maxScalar = new Scalar(high, topmaxsat, topmaxsat);

        Imgproc.cvtColor(rgbaMat, rgbMat, Imgproc.COLOR_RGBA2RGB);

        //first find purpls objects
        //Imgproc.cvtColor(rgbMat, hsvMat, Imgproc.COLOR_RGB2HSV);
        //Core.inRange(hsvMat, ShellyPurplHair.getHSVmin(), ShellyPurplHair.getHSVmax(), thresholdMat);
        //morphOps(thresholdMat);
        //trackFilteredObject(ShellyPurplHair, thresholdMat, hsvMat, rgbMat);
 
        //then greens
        Imgproc.cvtColor(rgbMat, hsvMat, Imgproc.COLOR_RGB2HSV);
        // Core.inRange(hsvMat, justgreen.getHSVmin(), justgreen.getHSVmax(), thresholdMat);
        Core.inRange(hsvMat, minScalr, maxScalar, thresholdMat);
        morphOps(thresholdMat);
        trackFilteredObject(justgreen, thresholdMat, hsvMat, rgbMat);

        //Imgproc.putText (rgbMat, "W:" + rgbMat.width () + " H:" + rgbMat.height () + " SO:" + Screen.orientation, new Point (5, rgbMat.rows () - 10), Imgproc.FONT_HERSHEY_SIMPLEX, 1.0, new Scalar (255, 255, 255, 255), 2, Imgproc.LINE_AA, false);

        Imgproc.cvtColor(rgbMat, rgbaMat, Imgproc.COLOR_RGB2RGBA);

 
    }
    /// <summary>
    /// Morphs the ops.
    /// </summary>
    /// <param name="thresh">Thresh.</param>
    private void morphOps(Mat thresh)
    {
        //create structuring element that will be used to "dilate" and "erode" image.
        //the element chosen here is a 3px by 3px rectangle
        Mat erodeElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(3,3));
        //dilate with larger element so make sure object is nicely visible
        Mat dilateElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(8,8));

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

                    //Debug.Log("area " + area);

                    //if the area is less than 20 px by 20px then it is probably just noise
                    //if the area is the same as the 3/2 of the image size, probably just a bad filter
                    //we only want the object with the largest area so we safe a reference area each
                    //iteration and compare it to the area in the next iteration.
                    if (area > _cur_min_colorArea && area < cur_max_coloArea)
                    {
                        //Debug.Log("keeparea " + area);
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
        //  minimap.UpdateLocationSimpleVomit_Player(AVRx, AVRy);
        EventsManagerLib.CALL_Player_Located_evnt(AVRx, AVRy);
    }

    void OnDestroy()
    {
         
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
