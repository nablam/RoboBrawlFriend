using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManagerLib : MonoBehaviour
{
    public static EventsManagerLib Instance = null;

    private void Awake()
    {
        // Debug.Log("GAME eventMANAGER Awake");
        //  FindObjectOfType<cursorkiller>().ShouldIkillCursor();
        if (Instance == null)
        {
            Instance = this;
        }
        else
            Destroy(gameObject);
    }
    public delegate void EVENT_evnt(int x, int y);
    public static event EVENT_evnt On_EVENT_evnt;
    public static void CALL_EVENT_evnt(int x, int y)
    {
        if (On_EVENT_evnt != null) On_EVENT_evnt(x, y);
    }


    #region OnInited

    public delegate void EVENT_NDIstartedStreaming(int argframe_W, int argframe_H);
    public static event EVENT_NDIstartedStreaming On_NDI_startedStreaming;
    public static void CALL_NDI_StartedStreaming(int argframe_W, int argframe_H)
    {
        if (On_NDI_startedStreaming != null) On_NDI_startedStreaming(argframe_W, argframe_H);
    }

    //public delegate void EVENT_CAMstartedStreaming(int argframe_W, int argframe_H);
    //public static event EVENT_CAMstartedStreaming On_Cam_startedStreaming;
    //public static void CALL_CAM_StartedStreaming(int argframe_W, int argframe_H)
    //{
    //    if (On_Cam_startedStreaming != null) On_Cam_startedStreaming(argframe_W, argframe_H);
    //}
    #endregion



    #region OnDispose
    //public delegate void EVENT_CAM_DisposeEvnt( );
    //public static event EVENT_CAM_DisposeEvnt On_cam_Dispos;
    //public static void CALL_cam_dispose_evnt( )
    //{
    //    if (On_cam_Dispos != null) On_cam_Dispos();
    //}

    public delegate void EVENT_NDI_DisposeEvnt();
    public static event EVENT_NDI_DisposeEvnt On_ndi_Dispos;
    public static void CALL_ndi_dispose_evnt()
    {
        if (On_ndi_Dispos != null) On_ndi_Dispos();
    }
    #endregion


    #region OnError
    //public delegate void EVENT_CAM_ERROREvnt(int argerrorInt );
    //public static event EVENT_CAM_ERROREvnt On_cam_Error;
    //public static void CALL_cam_Error_evnt(int argerrorInt)
    //{
    //    if (On_cam_Error != null) On_cam_Error(argerrorInt);
    //}

    public delegate void EVENT_NDI_ERROREvnt(int argerrorInt);
    public static event EVENT_NDI_ERROREvnt On_ndi_Error;
    public static void CALL_ndi_Error_evnt(int argerrorInt)
    {
        if (On_ndi_Error != null) On_ndi_Error(argerrorInt);
    }
    #endregion


    #region Tracking

    public delegate void EVENT_OCV_ReTrack();
    public static event EVENT_OCV_ReTrack On_reTrack;
    public static void CALL_OCV_Retrack_evnt()
    {
        if (On_reTrack != null) On_reTrack();
    }


    public delegate void EVENT_TrackerCSRT_X(int argXposition);
    public static event EVENT_TrackerCSRT_X On_TrackerCSRT_X;
    public static void CALL_TrackerCSRT_updated_X(int argXposition)
    {
        if (On_TrackerCSRT_X != null) On_TrackerCSRT_X(argXposition);
    }


    public delegate void EVENT_FaceRectDetected(int x, int y, int w, int h, int nx, int ny);
    public static event EVENT_FaceRectDetected On_FaceRectDetected;
    public static void CALL_FaceRectedDetected_evnt(int x, int y, int w, int h, int nx, int ny)
    {
        if (On_FaceRectDetected != null) On_FaceRectDetected(x, y, w, h, nx, ny);
    }
    #endregion



    #region NDIframeWarning
    public delegate void EVENT_NDIWarning(int x, int y);
    public static event EVENT_NDIWarning On_NDIWarning;
    public static void CALL_NDIWarningT_evnt(int x, int y)
    {
        if (On_NDIWarning != null) On_NDIWarning(x, y);
    }
    #endregion



    #region Actions
    public delegate void EVENT_DoAction(int argActionNumber);
    public static event EVENT_DoAction On_DoAction_i;
    public static void CALL_DoAction_i(int argActionNumber)
    {
        if (On_DoAction_i != null) On_DoAction_i(argActionNumber);
    }
    #endregion


    #region SRSLBroadcast
   

    public delegate void EVENT_HAND_Broadcast(float arg_R, float arg_L, bool arg_sol, e_HandSide argSide);
    public static event EVENT_HAND_Broadcast On_Hand_Broadcast;
    public static void CALL_Hand_Broadcast(float arg_R, float arg_L, bool arg_sol, e_HandSide argSide)
    {
        if (On_Hand_Broadcast != null) On_Hand_Broadcast(arg_R, arg_L, arg_sol, argSide);
    }
    #endregion


    #region Display_Text_Broadcast
    public delegate void EVENT_TextDisplay(string argstr0, string argstr1);
    public static event EVENT_TextDisplay On_TextDisplay;
    public static void CALL_DisplayStrings(string argstr0, string argstr1)
    {
        if (On_TextDisplay != null) On_TextDisplay( argstr0,  argstr1);
    }

    public delegate void EVENT_Player_Located(double pos_X_Parea, double pos_Y_Parea);
    public static event EVENT_Player_Located On_Player_Located;
    public static void CALL_Player_Located_evnt(double pos_X_Parea, double pos_Y_Parea)
    {
        if (On_Player_Located != null) On_Player_Located(pos_X_Parea, pos_Y_Parea);
    }


    public delegate void EVENT_FieldScrollDist_(double avrDeltaY , double percent);
    public static event EVENT_FieldScrollDist_ On_FiieldScrollDist_;
    public static void CALL_FieldScrollDist__evnt(double avrDeltaY , double percent)
    {
        if (On_FiieldScrollDist_ != null) On_FiieldScrollDist_(avrDeltaY , percent);
    }
    #endregion

}
