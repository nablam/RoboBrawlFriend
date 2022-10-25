using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageErodeDialateFiler : MonoBehaviour
{
    public double PublicBlurFacro = 15;
    public double PublicBlurSigma = 0;
    public double kernel = 10;
    public bool DoBLur, DoEnrode, DoDialate, DoOpen;
    public void Do_Image_manipulation(Mat rgbp) {
        //Creating destination matrix
        Mat dst = new Mat(rgbp.rows(), rgbp.cols(), rgbp.type());

        if (DoBLur)
        {
            //Applying GaussianBlur on the Image
            Imgproc.GaussianBlur(rgbp, rgbp, new Size(PublicBlurFacro, PublicBlurFacro), PublicBlurSigma);
        }
        Mat erodeElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(kernel, kernel));
        //dilate with larger element so make sure object is nicely visible
        Mat dilateElement = Imgproc.getStructuringElement(Imgproc.MORPH_RECT, new Size(kernel, kernel));
        if (DoEnrode)
        {
            Imgproc.erode(rgbp, rgbp, erodeElement);
          //  Imgproc.erode(rgbp, rgbp, erodeElement);
        }

        if (DoDialate)
        {
            Imgproc.dilate(rgbp, rgbp, dilateElement);
          //  Imgproc.dilate(rgbp, rgbp, dilateElement);
        }


    }

    void OldWay() { 
    
    
    }

    void Do_openning(Mat rgbp) {

        Mat kernel = Mat.ones(5, 5, CvType.CV_32F);
        //Applying dilate on the Image
        Imgproc.morphologyEx(rgbp, rgbp, Imgproc.MORPH_OPEN, kernel);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
