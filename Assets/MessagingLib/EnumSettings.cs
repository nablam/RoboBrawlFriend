using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_CamSourceName
{
    NDI1,//"NDI Webcam Video 1"
    NDI2,//"NDI Webcam Video 2"
    NDI3,//"NDI Webcam Video 3"
    NDI4,//"NDI Webcam Video 4"
    NDIScreen,//"DESKTOPX0NOM0RA QIntel HD Graphics 530 1Z"
    FHDcap, //"FHD Capture"
    ElGato, //"Game Capture HD60 S+"
    ElstreamLnk,//"Elgato Screen Link"
    UsbHdmiMinimal,//"USB3. 0 capture"
    Wooden,//"USB Video Device"
    Brio,//"Logitech BRIO"
    Bison,//"BisonCam, NB Pro"
    IrPi,//"USB Camera"
}
public enum e_SourceDeviceTypes
{
    MotoG8,
    G6,
    OBS720p,
    NdiVizor,
    other
}

public enum e_BrawlMapType { 
    Starpark,
    GemGrab,
    Solo,

}

public enum e_BrawlMapName {
    Starpark,
    Crystalarcade,
    Deepdiner,
    EchoChamber,
    Gemfort,
    SapphirePlains,
    Undermine
}



public enum e_HandSide
{
    LEFT_hand,
    RRIGHT_hand
}

public enum e_HandState
{
    Centering,
    Touching,
    Dragging,
    Hovering
}

public enum e_ButtonLocationType
{
    Main,
    SuperFire,
    GadgetFire,
    Center
}
