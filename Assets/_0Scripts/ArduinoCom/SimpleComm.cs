
using UnityEngine;
using System.IO.Ports; //needed to change the api compatibility to .NET Framework . was originally on .NET Standard 2.1
using System;
using System.Collections;
using TMPro;

public class SimpleComm : MonoBehaviour
{

    #region _private_Vars
    SerialPort sp;
    bool SP_WasInited = false;
    string _message_;           //  lb  lt  l   rb  rt  r  cmd dbg 
                                //string hard_messageRxample = "<060.120.000.120.060.000.911.000>#";//withous  "."
    string hard_messageRxample = "060120000120060000911000";//withous  "."
    string hard_90message = "090090000090090000000000";
    bool SP_AllowSerialWrite;
    char[] messagearrra;
    #endregion

    #region Public_Vars

    #endregion

    #region Enabled_Disable_Awake_Start_Destroy
    private void OnEnable()
    {

    }
    private void OnDisable() { StopCoroutine(RunUART()); CloseSerialPort(); }


    private void Awake()
    {

    }
    void Start()
    {

    }
    private void OnDestroy()
    {

    }
    #endregion

    #region EventHAndlers


    #endregion

    #region UPDATE
    void Update()
    {

    }
    #endregion

    #region _private_methods
    void _Init_(int argComNumber, int argBAUD)
    {

        // Update_Message(90, 90, false, false, 911, 000); //setting 911 will disregard angles provided and make a message setting defailt angles
        Update_Message(hard_messageRxample); //setting 911 will disregard angles provided and make a message setting defailt angles
        if (argComNumber < 3) argComNumber = 3;
        if (argComNumber > 4) argComNumber = 4;
        //cuz I only am using com 4 as a debug tool 
        if (argBAUD > 115200) argBAUD = 115200;
        if (argBAUD < 115200) argBAUD = 9600;
        OpenCom(argComNumber, argBAUD);

    }
    void OpenCom(int argComNumber, int argBAUD)
    {
        string the_com = "";
        the_com = "COM" + argComNumber.ToString();
        //  sp = new SerialPort("\\\\.\\" + the_com, 115200);
        sp = new SerialPort("\\\\.\\" + the_com, argBAUD);
        sp.WriteBufferSize = 27;
        sp.NewLine = "\n";
        sp.WriteTimeout = 100;
        sp.DtrEnable = true;//prevents writing to overflow and freakout afterr 90 sec
        if (!sp.IsOpen)
        {
            Debug.Log("Opening Serial " + the_com + ", baud " + argBAUD.ToString());
            sp.Open();
            // sp.ReadTimeout = 500;
            sp.Handshake = Handshake.None;
            if (sp.IsOpen)
            {
                Debug.Log("Serial is Open");
                SP_WasInited = true;
                StartCoroutine(RunUART());
            }
        }
    }
    IEnumerator RunUART()
    {
        Debug.Log("starteduart");
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.02f);
            // Debug.Log(_message_);
            if (SP_AllowSerialWrite)
                Send_BufferedMEssage();
        }
    }
    void Send_BufferedMEssage()
    {
        if (sp != null)
        {
            if (!sp.IsOpen)
            {
                sp.Open();
                Debug.Log("opened sp");
            }
            if (sp.IsOpen)
            {
                sp.WriteLine(_message_);
                sp.BaseStream.Flush();
            }
        }
        else
        {
            Debug.Log("SP NULL!");

        }
    }
    IEnumerator ResetUART()
    {
        SP_AllowSerialWrite = false;
        StopCoroutine(RunUART());
        yield return new WaitForSecondsRealtime(1f);
        // Debug.Log(_message_);
        if (SP_WasInited)
        {
            CloseSerialPort();
        }
    }
    private void CloseSerialPort()
    {
        if (sp != null)
        {
            Debug.Log("closing sp and flushing");
            Update_Message(hard_90message);
            sp.WriteLine(_message_);
            sp.BaseStream.Flush();
            sp.Close();
            sp.Open();
        }
        SP_WasInited = false;
        SP_AllowSerialWrite = false;
    }
    #endregion

    #region PUBLIC_Methods
    public void InitializeMe(int argComNumber, int argBAUD) { _Init_(argComNumber, argBAUD); }

    //can be updates a milion times per second or once a year because
    // MedianNerve will deal with this on a timed basis unlsess urgeant
    public void Update_Message(string arg_27ByteMessage)
    {
        _message_ = arg_27ByteMessage;
        _message_ = string.Concat('<', _message_, '>', '#');
        messagearrra = _message_.ToCharArray();
        //and that's it ... the rest is dealt by this class
        // Debug.Log("updated message  ...lt " + _thumb_L_ang_ToSend + "..."+ _thumb_L_state_ToSend + " com " + _commande_toSend);
        EventsManagerLib.CALL_DisplayStrings(_message_, "27bytes");
    }

    public void AllowSerialWrite(bool argallow)
    {
        SP_AllowSerialWrite = argallow;
        Debug.Log("allowing");
    }
    public void ResetMe() { StartCoroutine(ResetUART()); }
    #endregion


}
 