using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.VideoModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTracker : MonoBehaviour
{


    public double CumulAvrYdisp =0;
    public float MAxDist = 820f;
    public float PERCENT;

    public int Pub_MaxCorners = 40;
    public double Pub_Quality = 0.025;
    public int Pub_MinDist = 20;

    public float CumulAvrYdispFloat = 0;
    /// <summary>
    /// The mat op flow this.
    /// </summary>
    Mat matOpFlowThis;

    /// <summary>
    /// The mat op flow previous.
    /// </summary>
    Mat matOpFlowPrev;
    List<Mat> UsedMats;

    /// <summary>
    /// The i GFFT max.
    /// </summary>
    int iGFFTMax = 40;

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
   // double originalacum = 0;
    public double cumulativeYdisplacementForFrame;
    public double AVRYdisplacementForFrame;

    bool isInResetWait = false;

    void Awake()
    {
       
        matOpFlowThis = new Mat();
        matOpFlowPrev = new Mat();
        MOPcorners = new MatOfPoint();
        mMOP2fptsThis = new MatOfPoint2f();
        mMOP2fptsPrev = new MatOfPoint2f();
        mMOP2fptsSafe = new MatOfPoint2f();
        mMOBStatus = new MatOfByte();
        mMOFerr = new MatOfFloat();

        UsedMats = new List<Mat>() { matOpFlowThis, matOpFlowPrev , MOPcorners , mMOP2fptsThis, mMOP2fptsPrev, mMOP2fptsSafe, mMOBStatus , mMOFerr };
        CumulAvrYdisp = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Do_ResetAll_andWait() {
        isInResetWait = true;
        cumulativeYdisplacementForFrame = 0;
        AVRYdisplacementForFrame = 0;
        CumulAvrYdisp = 0;
        disposeMAtslist();
        yield return new WaitForSeconds(0.5f);
        matOpFlowThis = new Mat();
        matOpFlowPrev = new Mat();
        MOPcorners = new MatOfPoint();
        mMOP2fptsThis = new MatOfPoint2f();
        mMOP2fptsPrev = new MatOfPoint2f();
        mMOP2fptsSafe = new MatOfPoint2f();
        mMOBStatus = new MatOfByte();
        mMOFerr = new MatOfFloat();

        UsedMats.Add(matOpFlowThis);
        UsedMats.Add(matOpFlowPrev);
        UsedMats.Add(MOPcorners);
        UsedMats.Add(mMOP2fptsThis);
        UsedMats.Add(mMOP2fptsPrev);
        UsedMats.Add(mMOP2fptsSafe);
        UsedMats.Add(mMOBStatus);
        UsedMats.Add(mMOFerr);
        isInResetWait = false;
    }
    #region PUBLIC_Methods
    public void TrackRoi(Mat rgbaMat_traking, bool argDoDrawPoints) {

        if (isInResetWait) return;

        Imgproc.equalizeHist(rgbaMat_traking, rgbaMat_traking);
        if (mMOP2fptsPrev.rows() == 0)
        {

            // first time through the loop so we need prev and this mats
            // plus prev points
            // get this mat
            Imgproc.cvtColor(rgbaMat_traking, matOpFlowThis, Imgproc.COLOR_RGBA2GRAY);

            // copy that to prev mat
            matOpFlowThis.copyTo(matOpFlowPrev);

            // get prev corners
            Imgproc.goodFeaturesToTrack(matOpFlowPrev, MOPcorners, iGFFTMax, Pub_Quality, Pub_MinDist);
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
            Imgproc.cvtColor(rgbaMat_traking, matOpFlowThis, Imgproc.COLOR_RGBA2GRAY);

            // get the corners for this mat
            Imgproc.goodFeaturesToTrack(matOpFlowThis, MOPcorners, iGFFTMax, Pub_Quality, Pub_MinDist);
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
            PointsFound = 0;
            Ydisp_loopCumul = 0;
            int x = 0;
            int y = byteStatus.Count - 1;

            for (x = 0; x < y; x++)
            {
                if (byteStatus[x] == 1)
                {
                    PointsFound++;


                    Point pt = cornersThis[x];
                    Point pt2 = cornersPrev[x];

                    double Ydisp =pt2.y - pt.y;
                    //float ydispFl = (float)Ydisp;

                    //float f = Mathf.Round(ydispFl * 100.0f);
                    //int i = (int)f;
                    //i /= 100;
                    
                    Ydisp_loopCumul += Ydisp;
                    if (argDoDrawPoints)
                    {
                        Imgproc.circle(rgbaMat_traking, pt, 5, colorRed, iLineThickness - 1);

                        Imgproc.line(rgbaMat_traking, pt, pt2, colorRed, iLineThickness);
                    }
                   
                }
            }
            if (PointsFound > 0) {

                cumulativeYdisplacementForFrame = Ydisp_loopCumul ;
                AVRYdisplacementForFrame = Ydisp_loopCumul / PointsFound*-1;
                if (AVRYdisplacementForFrame > 3) AVRYdisplacementForFrame = 3;
                if (AVRYdisplacementForFrame < -3) AVRYdisplacementForFrame = -3;

                CumulAvrYdisp += AVRYdisplacementForFrame;

                if (CumulAvrYdisp < 1) CumulAvrYdisp = 0;
                if (CumulAvrYdisp > MAxDist) CumulAvrYdisp = MAxDist;


                 
                PERCENT =(float) CumulAvrYdisp / MAxDist ;
                PERCENT *= 100;
                Mathf.RoundToInt(PERCENT);
                PERCENT *= 0.01f;
                //float OnePercent = (float)CumulAvrYdisp / 100f;
                //CumulAvrYdisp -= OnePercent;
                // Debug.Log(PointsFound + "  " + CumulAvrYdisp);
            }
            
            // minimap.UpdateLocationSimpleVomit(CumulAvrYdisp);

        }
    
    }
    public void ResetTracking() {
        if (isInResetWait) return;
        StartCoroutine(Do_ResetAll_andWait());
    }

    #endregion
    private void OnDestroy()
    {
        disposeMAtslist();
    }
    void disposeMAtslist()
    {
        if (UsedMats != null)
        {
            foreach (Mat m in UsedMats)
            {
                if (m != null)
                    m.Dispose();
            }
            UsedMats.Clear();
        }

     
    }

}
