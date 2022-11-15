using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppSettings : MonoBehaviour
{
    public static AppSettings Instance = null;

    e_SourceDeviceTypes _defaultGameDevice;

    PlayerDevice G8deviceDimenssions;
    PlayerDevice M6deviceDimenssions;
    PlayerDevice Native720pdeviceDimenssions;
    PlayerDevice Native1080pdeviceDimenssions;
    PlayerDevice _usedDevicedims;
    [SerializeField]
    float _Actual_Device_resolution_w=0;//= 1080;//default G8 widtht
    [SerializeField]
    float _Actual_Device_resolution_h=0;// = 1080; //default G8 height
    /// <summary>
    /// can get width height in px and mm and e_type
    /// </summary>
    /// <returns></returns>
    public PlayerDevice Get_NativeDevice_player()
    {
        
        return _usedDevicedims;
    }

    const int NUMBER_OF_POSSIBLETILE_MIDSCREEN = 31;
    float _tileSize = 50f;
    public int  Get_Total_HorizontalTiles( ) {

        
        return   NUMBER_OF_POSSIBLETILE_MIDSCREEN;
    }
    int NumberOfInGameTilesInARow = 21; //in starpark it is 17 , TODO: find solo 
    public int Get_InGameRowSize_inTiles(e_BrawlMapType arg_BrawlMapType) {

        if (arg_BrawlMapType == e_BrawlMapType.Starpark) {
            NumberOfInGameTilesInARow = 17;
        }
        else
            if (arg_BrawlMapType == e_BrawlMapType.GemGrab)
        {
            NumberOfInGameTilesInARow = 21;
        }
        else
            if (arg_BrawlMapType == e_BrawlMapType.Solo)
        {
            NumberOfInGameTilesInARow = 27; //TODO: FINDOUT
        }

        return this.NumberOfInGameTilesInARow;
    }


    float radius_D, radius_G,radius_;
    float timeThreshold_Dashing;
    float timeThreshold_transiting;

    public float Radius_ { get => radius_; private set => radius_ = value; }
    public float Radius_D1 { get => radius_D;private set => radius_D = value; }
    public float Radius_G1 { get => radius_G;private set => radius_G = value; }
    public float TimeThreshold_Dashing { get => timeThreshold_Dashing; private set => timeThreshold_Dashing = value; }
    public float TimeThreshold_Transiting { get => timeThreshold_transiting; private set => timeThreshold_transiting = value; }

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;

            // GAMESETTINGS_DATA = new Data_presetSettings();
            // PicDim_preset_List = new List<PicDims>();
            _defaultGameDevice = e_SourceDeviceTypes.MotoG8;

            G8deviceDimenssions = new PlayerDevice("moto g8", e_SourceDeviceTypes.MotoG8, 2300, 1080, 150, 68);
              M6deviceDimenssions = new PlayerDevice("moto M6", e_SourceDeviceTypes.G6, 2100, 1080, 140, 60);
            Native720pdeviceDimenssions = new PlayerDevice("obs 720p", e_SourceDeviceTypes.OBS720p, 1280, 720, 100, 50); //idk if mm size will be used 
            Native1080pdeviceDimenssions = new PlayerDevice(" 1080p", e_SourceDeviceTypes.other, 1920, 1080, 100, 50);
            _usedDevicedims = G8deviceDimenssions; //M6deviceDimenssions;// G8deviceDimenssions;
            _Actual_Device_resolution_w = _usedDevicedims.Widh_px;
            _Actual_Device_resolution_h = _usedDevicedims.Height_px;
            _defaultGameDevice = _usedDevicedims.My_eType;

            radius_ = 8f;
            radius_D = 6f;
            radius_G = 6f;
            timeThreshold_Dashing = 1f;
            timeThreshold_transiting = 2f;
        }
        else
            Destroy(gameObject);
    }

    //what phone is used (or obs video)
    //get rectdims for motog8 
    //get rects for arena 

    public class PlayerDevice{
        string _myGivenname;
        e_SourceDeviceTypes _my_eType;
        float _widh_px,_widh_mm, _height_px, _height_mm;

        public PlayerDevice(string myGivenname, e_SourceDeviceTypes my_eType, float widh_px, float height_px, float widh_mm, float height_mm)
        {
            _myGivenname = myGivenname;
            _my_eType = my_eType;
            _widh_px = widh_px;
            _height_px = height_px;
            _widh_mm = widh_mm;
            _height_mm = height_mm;
        }
        

        public string MyGivenname { get => _myGivenname; private set => _myGivenname = value; }
        public e_SourceDeviceTypes My_eType { get => _my_eType; private set => _my_eType = value; }
        public float Widh_px { get => _widh_px; private set => _widh_px = value; }
        public float Widh_mm { get => _widh_mm; private set => _widh_mm = value; }
        public float Height_px { get => _height_px; private set => _height_px = value; }
        public float Height_mm { get => _height_mm; private set => _height_mm = value; }
    }
    public class ScreenAreaManipulationPoints 
    {
        int _frame_W,_frame_H;
        int _marginTop, _marginLeft;
        int _numberofTilesInFieldRow;
        const int _numberofTilesVertical = 16;

        public ScreenAreaManipulationPoints(int argframe_W, int argframe_H, e_SourceDeviceTypes argsOurceDevice, e_BrawlMapType argGameType  )
        {

            _frame_W = argframe_W;
            _frame_H = argframe_H;

             

            if (argsOurceDevice == e_SourceDeviceTypes.MotoG8) { 
            
            
            }
            else
            if (argsOurceDevice == e_SourceDeviceTypes.G6)
            {


            }
            else
            if (argsOurceDevice == e_SourceDeviceTypes.OBS720p)
            {


            }


        }

        //public int Frame_W { get => _frame_W; set => _frame_W = value; }
        //public int Frame_H { get => _frame_H; set => _frame_H = value; }
        //public int MarginTop { get => _marginTop; set => _marginTop = value; }
        //public int MarginLeft { get => _marginLeft; set => _marginLeft = value; }
        //public int NumberofTilesInFieldRow { get => _numberofTilesInFieldRow; set => _numberofTilesInFieldRow = value; }

      
    }


    public class SheeringPoints { 
    
    
    
    }


    public  void DebugMat(Mat m, string matvarname)
    {
        int mrows = m.rows();
        int mcols = m.cols();
        int chans = m.channels();
           Debug.Log("*Mat RC* " + matvarname + " [rowz." + mrows + " colz."+  mcols + "ch("+ chans+ ")]  **");
        string Col = "col0 ";
        for (int r = 0; r < mrows; r++) {

            Col += m.get(r, 0).GetValue(0) + " , " + m.get(r, 0).GetValue(1) + " | ";
        }
        Debug.Log(Col);
    }
}
