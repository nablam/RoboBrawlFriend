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
    void InitializeMatsandTextures()
    {

        imgTexture_originalPic = Resources.Load<Texture2D>("_minimaps/MiniMaps_starpark");
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



    void Start()
    {

        InitializeMatsandTextures();

    }

    // Update is called once per frame
    void Update()
    {
        GameView.y = ConvertedY;
        PlayerArea.y = GameView.y + 40;
        //Imgproc.threshold(imgMat, imgMat, THRESH, MAXVAL, ttype012347816);//threshold = 0.8

        //Imgproc.rectangle(imgMat, new Point(0, 200), new Point(200,100), new Scalar(255, 0, 0, 255), 2); GameView

        Mat m = imgMat.clone();
        Imgproc.circle(m, PlayerPoint, 10, new Scalar(255, 50, 60, 255), 2);
        if (enemiepoints != null)
        {
            if (enemiepoints.Length > 0)
            {
                Imgproc.circle(m, enemiepoints[0], 10, new Scalar(0, 255, 60, 255), 2);
            }
        }
        Imgproc.rectangle(m, GameView, new Scalar(255, 0, 0, 255), 2);
        Imgproc.rectangle(m, PlayerArea, new Scalar(0, 0, 255, 255), 2);
        Utils.matToTexture2D(m, OCVtexture, false);//no flip

    }
}
