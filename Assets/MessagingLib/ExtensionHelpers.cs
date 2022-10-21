using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionHelpers  
{
    public static string HelpMakeString_camsourceName(e_CamSourceName argEnumName)
    {
        string Outstr = "";
        switch (argEnumName) {
            case e_CamSourceName.NDI1:
                Outstr = "NDI Webcam Video 1";
                break;
            case e_CamSourceName.NDI2:
                Outstr = "NDI Webcam Video 2";
                break;
            case e_CamSourceName.NDI3:
                Outstr = "NDI Webcam Video 3";
                break;
            case e_CamSourceName.NDI4:
                Outstr = "NDI Webcam Video 4";
                break;
            case e_CamSourceName.NDIScreen:
                Outstr = "DESKTOPX0NOM0RA QIntel HD Graphics 530 1Z";
                break;


            case e_CamSourceName.FHDcap:
                Outstr = "FHD Capture";
                break;
            case e_CamSourceName.ElGato:
                Outstr = "Game Capture HD60 S+";
                break;
            case e_CamSourceName.ElstreamLnk:
                Outstr = "Elgato Screen Link";
                break;

            case e_CamSourceName.UsbHdmiMinimal:
                Outstr = "USB3. 0 capture";
                break;

            case e_CamSourceName.Wooden:
                Outstr = "USB Video Device";
                break;
            case e_CamSourceName.Brio:
                Outstr = "Logitech BRIO";
                break;

            case e_CamSourceName.IrPi:
                Outstr = "USB Camera";
                break;


        }
        //string inputstr = argEnumName.ToString();
        //inputstr = inputstr.Replace('X', '-');
        //inputstr = inputstr.Replace('x', ',');
        //inputstr = inputstr.Replace('_', ' ');
        //inputstr = inputstr.Replace('Q', '(');
        //inputstr = inputstr.Replace('Z', ')');

        return Outstr;
    }
}
