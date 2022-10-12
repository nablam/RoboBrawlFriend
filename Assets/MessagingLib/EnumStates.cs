using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public enum MODE { 

   Ndi_As_Source,
   Ndi_asSource_camAsBKG,
   Cam_as_source,



}

public enum ErrorCodez  
{
    UNKNOWN = 0,
    CAMERA_DEVICE_NOT_EXIST,
    CAMERA_PERMISSION_DENIED,
    TIMEOUT,
}
//public enum oldAppmoned
//{
//    BasickSnakeNdiServo,
//    CamWindowTracking,
//    TrackMeServo,
//    TrackMeWindowd

//     Nditrack_U2A_ExtCam, //trackface from ndi -> ut2 externalcam
//    Nditrack_U2ACam,    // trackface ndi u2a -> internal cam unity ndi out
//    Nditrack_WindowedCam,  // trackface ndi window -> internal cam unity ndi out

//    CamTrackme_U2A_ExtCam,
//    CamTrackme_U2ACam,
//    CamTrackme_WindowedCam,

   

//    Debug_ndi_draw,
//    Debug_cam_draw,
//    Debug_astroz

//}

public enum ApplicationModes
{
    Nditrack_U2A_ExtCam,
    Nditrack_U2ACam, //new
    Nditrack_WindowedCam,
    CamTrackme_U2A_ExtCam,
    CamTrackme_U2ACam, //new
    CamTrackme_WindowedCam,

    Debug_ndi_draw,
    Debug_cam_draw,
    Debug_astroz



}

public enum PanningMode
{
    Hardware,
    Software
}

public enum SourceType
{
    CAM,
    NDI
}


public enum Resolution_faces { 

    _64x64,
    _100x100,
    _128x128
}

public enum Resolution_DisplayRoi
{

    _320x180,
    _320x240,
    _400x300
}
public enum ResolutionsTypes
{


    _640x480,  // ---------Working format
    _800x600,  //default
    _1920x1080,//--------HD1080ultra
    _960x540,//half
    _480x270,//quaret
    _1280x720,//--------720 super
    _640x360,//half
    _320x180,//quarter
    _1280x960,//--------960p
    _960x480,//half
    _480x240,//quarter
    _64x64,
    _128x128,
    _512x512,
    _100x200,
    _64x128,
    _430x240,
    _400x300,
    _320x240,
        _100x100,
        _480x360,
        _200x200
    


}
public enum VideoCapturerNames {
    HD_USB_Camera,
    NewTek_NDI_Video,
    BisonCamx_NB_Pro,
    GENERAL_X_UVC_,
    FHD_Capture,
    Unity_Video_Capture,
    OBS_Virtual_Camera,

    square_64,
    square_128,
    square_512,
    facecustom_01,
    facecustom_02,
    facecustom_03,
    DisplayCustom_01,
    DisplayCustom_02,
    DisplayCustom_03,

    DESKTOPX0NOM0RA_QIntel_HD_Graphics_530_1Z ,
    DESKTOPX0NOM0RA_QIntel_HD_Graphics_530_2Z ,
    DESKTOPX0NOM0RA_QIntel_HD_Graphics_530_3Z ,
    DESKTOPX0NOM0RA_QRemote_Connection_1Z ,
}


public enum InteractiveVideoGameName
{
    SpaceOdyssey,
    YetiChase,
}


public enum CameraState_ingame
{
    UDP_RotateAroundPlayer,
    Fixed_UdpRotatesPlayer,
    FreeRoaming,
}
//string[7] camnames = { "HD USB Camera", "NewTek NDI Video", "BisonCam, NB Pro", "GENERAL - UVC ", "FHD Capture", "Unity Video Capture", "OBS Virtual Camera" };

// X  -
// x  ,
// _ " "

//Q (
//Z )

// K separate name from resolution

