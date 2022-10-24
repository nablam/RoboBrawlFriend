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
    public GameObject ActionButtons;
    public GameObject HistoQuad;

    public TMP_Text m_TextComponent_con;

    public  Button PausebuttonBAckGround;
    public  Text PauseButtonText;

    #region On_menuebuttonClicked
    void TurnRed()
    {
        ColorBlock colors = PausebuttonBAckGround.colors;
        colors.selectedColor = Color.red;
       
        PausebuttonBAckGround.colors = colors;
        PauseButtonText.text = "play";
    }

     void TurnGreen()
    {
        ColorBlock colors = PausebuttonBAckGround.colors;
        colors.selectedColor = Color.green;

        PausebuttonBAckGround.colors = colors;
        PauseButtonText.text = "playing";
    }

    bool fpsOn = true;
    public void OnFps_clicked() { Debug.Log("fps"); fpsOn = !fpsOn; myfpsmon.boxVisible = fpsOn; }

    bool mapOn = false;
    public void OnMap_clicked() { mapOn = !mapOn;  if (mapOn) { MinimapObj.transform.localPosition = new Vector3(MinimapObj.transform.localPosition.x, MinimapObj.transform.localPosition.y, -6); } else { MinimapObj.transform.localPosition = new Vector3(MinimapObj.transform.localPosition.x, MinimapObj.transform.localPosition.y, 2); } }

    bool conOn = false;
    public void OnCon_clicked() { conOn = !conOn; ConsolePanelj.gameObject.SetActive(conOn); }


    bool PauseOn = false;
    public void OnPause_clicked() { PauseOn = !PauseOn; if (PauseOn) { TurnRed(); } else { TurnGreen(); } }

  

    bool HistoOn = true;
    public void On_ToggleHisto() { Debug.Log("clicked Histo"); HistoOn=!HistoOn; HistoQuad.gameObject.SetActive(HistoOn); }

    bool ActionBtnsOn = false;
    public void On_Toggl_ActionButtons() { Debug.Log("clicked togact Btns"); ActionBtnsOn = !ActionBtnsOn; ActionButtons.gameObject.SetActive(ActionBtnsOn); }
    #endregion

    #region OnActionButtonClicked

    public void On_ResetMapTracking() { Debug.Log("Reset Tracking event"); EventsManagerLib.CALL_OCV_Retrack_evnt(); }

    public void On_Actionclicked(int argActionNuber) {

        switch (argActionNuber) {

            case 0:
                Debug.Log("clicked action 0");
                EventsManagerLib.CALL_DoAction_i(0);
                break;

            case 1:
                Debug.Log("clicked action 1");
                EventsManagerLib.CALL_DoAction_i(1);
                break;

            case 2:
                Debug.Log("clicked action 2");
                EventsManagerLib.CALL_DoAction_i(2);
                break;


            case 3:
                Debug.Log("clicked action 3");
                EventsManagerLib.CALL_DoAction_i(3);
                break;


            case 4:
                Debug.Log("clicked action 4");
                EventsManagerLib.CALL_DoAction_i(4);
                break;
        }
        

    }
    #endregion
    void Start()
    {
        myfpsmon.boxVisible = fpsOn;
        HistoQuad.gameObject.SetActive(HistoOn);
        ConsolePanelj.gameObject.SetActive(conOn);
        ActionButtons.gameObject.SetActive(ActionBtnsOn);

        if (mapOn) {
            MinimapObj.transform.localPosition = new Vector3(MinimapObj.transform.localPosition.x, MinimapObj.transform.localPosition.y, -6); } 
        else { MinimapObj.transform.localPosition = new Vector3(MinimapObj.transform.localPosition.x, MinimapObj.transform.localPosition.y, 2); }

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
