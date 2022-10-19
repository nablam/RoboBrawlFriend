using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 

public class CanvasCTRL : MonoBehaviour
{
    public GameObject MinimapObj;

    public MyFpsMon myfpsmon;

    public GameObject ConsolePanelj;

    public TMP_Text m_TextComponent_con;

    public  Button PausebuttonBAckGround;
    public  Text PauseButtonText;


    public void TurnRed()
    {
        ColorBlock colors = PausebuttonBAckGround.colors;
        colors.selectedColor = Color.red;
       
        PausebuttonBAckGround.colors = colors;
        PauseButtonText.text = "play";
    }

    public void TurnGreen()
    {
        ColorBlock colors = PausebuttonBAckGround.colors;
        colors.selectedColor = Color.green;

        PausebuttonBAckGround.colors = colors;
        PauseButtonText.text = "playing";
    }

    bool fpsOn = true;
    public void OnFps_clicked() { Debug.Log("fps"); fpsOn = !fpsOn; myfpsmon.boxVisible = fpsOn; }

    bool mapOn = true;
    public void OnMap_clicked() { mapOn = !mapOn;  if (mapOn) { MinimapObj.transform.localPosition = new Vector3(MinimapObj.transform.localPosition.x, MinimapObj.transform.localPosition.y, -6); } else { MinimapObj.transform.localPosition = new Vector3(MinimapObj.transform.localPosition.x, MinimapObj.transform.localPosition.y, 2); } }

    bool conOn = true;
    public void OnCon_clicked() { conOn = !conOn; ConsolePanelj.gameObject.SetActive(conOn); }


    bool PauseOn = false;
    public void OnPause_clicked() { PauseOn = !PauseOn; if (PauseOn) { TurnRed(); } else { TurnGreen(); } }

    public void On_ResetMapTracking() { Debug.Log("Reset Tracking event"); EventsManagerLib.CALL_OCV_Retrack_evnt(); }

    void Start()
    {
        
    }
    int framecnt = 0;
    // Update is called once per frame
    void Update()
    {
        framecnt++;

        string temmp = m_TextComponent_con.text;
      //  m_TextComponent_con.text =  framecnt.ToString() + "\n "  + m_TextComponent_con.text;



    }
}
