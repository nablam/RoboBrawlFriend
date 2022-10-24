using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rect = OpenCVForUnity.CoreModule.Rect;

public class RectsAndPointsMaker : MonoBehaviour
{
    //1 3
    //0 2

    public Point p0;
    public Point p1;
    public Point p2;
    public Point p3;


    public Point pa;
    public Point pb;
    public Point pc;
    public Point pd;



    public Point o_ptl;
    public Point o_ptr;
    public Point o_pbr;
    public Point o_pbl;


    public Point correct_ptl;
    public Point correct_ptr;
    public Point correct_pbr;
    public Point correct_pbl;
    Mat srcRectMat;
    Mat dstRectMat;

    Point pPerspective_tl;
    Point pPerspective_tr;
    Point pPerspective_br;
    Point pPerspective_bl;
    Rect _rect_TrimmedPerspective;
    private Rect _rect_TrackLeft;
    public Point PTrim_tl { get => pPerspective_tl; private set => pPerspective_tl = value; }
    public Point PTrim_tr { get => pPerspective_tr; private set => pPerspective_tr = value; }
    public Point PTrim_br { get => pPerspective_br; private set => pPerspective_br = value; }
    public Point PTrim_bl { get => pPerspective_bl; private set => pPerspective_bl = value; }
    public Rect Rect_TrimmedPerspective { get => _rect_TrimmedPerspective; private set => _rect_TrimmedPerspective = value; }
    public Rect Rect_TrackLeft { get => _rect_TrackLeft; private set => _rect_TrackLeft = value; }

    public e_BrawlMapType MapType; //is it gemgrab or starpark
    int Tile_W, Tile_H, HorizonTileCount;
    const int VertiTileCount = 16;
    void Awake()
    {
        if (MapType == e_BrawlMapType.Starpark) { HorizonTileCount = 17; }
        else if (MapType == e_BrawlMapType.GemGrab) { HorizonTileCount = 21; }
          else if (MapType == e_BrawlMapType.Solo) { HorizonTileCount = 27; }

        srcRectMat = new Mat(4, 1, CvType.CV_32FC2);
        dstRectMat = new Mat(4, 1, CvType.CV_32FC2);
        pPerspective_tl = new Point(100, 60);
        pPerspective_tr = new Point(1180, 60);
        pPerspective_br = new Point(1180, 660);
        pPerspective_bl = new Point(100, 660);

     
        int Rectwidthwanted = 940; //makes 21tiles 40px
        int RectHEIGHThwanted = 600;
        int ScreenWidth = 1280;
        int ScreenHeight = 720;
        int leftmargin = (ScreenWidth - Rectwidthwanted) / 2;
        int topMArgin = (ScreenHeight - RectHEIGHThwanted) / 2;

        _rect_TrimmedPerspective = new Rect(leftmargin, topMArgin, Rectwidthwanted, RectHEIGHThwanted); //makes 50px sssssssssssquarewidth for 21 arena tiles

        int TrackColumnWidth = 40;
        int TrackColumnHeight = 500;
        _rect_TrackLeft = new Rect(leftmargin+ Rectwidthwanted, topMArgin+80, TrackColumnWidth, TrackColumnHeight);


    }

    // Update is called once per frame
    void Update()
    {

        pPerspective_tl.x = (o_ptl.x + correct_ptl.x) / 2;
        pPerspective_bl.x = (o_pbl.x + correct_pbl.x) / 2;

        pPerspective_tr.x = (o_ptr.x + correct_ptr.x) / 2;
        pPerspective_br.x = (o_pbr.x + correct_pbr.x) / 2;

        srcRectMat.put(0, 0, o_ptl.x, o_ptl.y, o_ptr.x, o_ptr.y, o_pbr.x, o_pbr.y, o_pbl.x, o_pbl.y);
        dstRectMat.put(0, 0,correct_ptl.x,correct_ptl.y,correct_ptr.x,correct_ptr.y,correct_pbr.x,correct_pbr.y,correct_pbl.x,correct_pbl.y);
    }
    private void OnDestroy()
    {
        if (srcRectMat != null)
            srcRectMat.Dispose();

        if (dstRectMat != null)
            dstRectMat.Dispose();
    }

    public Mat GetSrcPTS() { return this.srcRectMat; }
    public Mat GetDstPTS() { return this.dstRectMat; }

}
