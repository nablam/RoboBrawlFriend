using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppSettings : MonoBehaviour
{
    public static AppSettings Instance = null;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;

           // GAMESETTINGS_DATA = new Data_presetSettings();
           // PicDim_preset_List = new List<PicDims>();
        }
        else
            Destroy(gameObject);
    }

    //what phone is used (or obs video)
    //get rectdims for motog8 
    //get rects for arena 
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

}
