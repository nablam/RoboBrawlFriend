using UnityEngine;
using System.IO.Ports; //needed to change the api compatibility to .NET Framework . was originally on .NET Standard 2.1
using System;
using System.Collections;

public class MedianNerve : MonoBehaviour
{
    SerialPort sp;
    VectorToServoAnglesConvertor ImpulseGenerator;
    ServosKinematicSolver TestTranslator;
    bool SP_WasInited = false;
    string _message_;           //  lb  lt  l   rb  rt  r  cmd dbg 
    string hard_messageRxample = "<060.120.000.120.060.000.911.000>#";//withous  "."
    bool SP_AllowStimulateNerve;
    public string tmp1;
    public int tmpint1;
    char[] messagearrra;
    float next_time;

    //********************************************************************************************
    //command 911 is emergency break or reset Debug is not used 
    // can be set to used command and debug as signle representation of curent frame number.
    //cmd holds the thousands, and debug the hundreds 
    float _thumb_L_ang_ToSend, _thumb_R_ang_ToSend;
    bool _thumb_L_state_ToSend, _thumb_R_state_ToSend;
    int _commande_toSend, _debug_ToSend;

     
    //********************************************************************************************

    #region Publicmethods
    public void InitializeMe(int argComNumber, int argBAUD, bool argUSECOM) { _Init_(argComNumber, argBAUD, argUSECOM); }

    //can be updates a milion times per second or once a year because
    // MedianNerve will deal with this on a timed basis unlsess urgeant
    public void Update_Message(float thumb_L_ang_ToSend, float thumb_R_ang_ToSend, bool thumb_L_state_ToSend, bool thumb_R_state_ToSend, int commande_toSend, int debug_ToSend)
    {
        _thumb_L_ang_ToSend = thumb_L_ang_ToSend;
        _thumb_R_ang_ToSend = thumb_R_ang_ToSend;
        _thumb_L_state_ToSend = thumb_L_state_ToSend;
        _thumb_R_state_ToSend = thumb_R_state_ToSend;
        _commande_toSend = commande_toSend;
        _debug_ToSend = debug_ToSend;
        _message_= ImpulseGenerator.Set_messageStringForMyServos(_thumb_L_ang_ToSend, _thumb_L_state_ToSend, _thumb_R_ang_ToSend, _thumb_R_state_ToSend, _commande_toSend, _debug_ToSend); ;

        _message_ = string.Concat('<', _message_, '>', '#');
        messagearrra = _message_.ToCharArray();
        //and that's it ... the rest is dealt by this class
       // Debug.Log("updated message  ...lt " + _thumb_L_ang_ToSend + "..."+ _thumb_L_state_ToSend + " com " + _commande_toSend);
    }
   
    public void AlloStimulation(bool argallow) {
        SP_AllowStimulateNerve = argallow;
        Debug.Log("allowing");
    }
    public void ResetMe() { StartCoroutine(ResetUART()); }
    #endregion

#if DEBUG_FRAMENUM
    int _frameNumber = 0;
    int _frameTopThousand = 0;
    int _frameLowHundreds = 0;
#endif
    public float TEstvalue = 15.5f;
    void _Init_(int argComNumber, int argBAUD, bool argUseCom)
    {
       // TestTranslator = new VectorToServo();

        
         //   float _x = i;
            //float _y = TestTranslator.GiveMeanXvalueFor(_x);
            //Debug.Log(_x+ " , " + _y);
         //  GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
           // sphere.transform.position = new Vector3(_x*10, _y,0);
        

        ImpulseGenerator = new VectorToServoAnglesConvertor(120, 60, 10);
        Update_Message(90, 90,false, false, 911, 000); //setting 911 will disregard angles provided and make a message setting defailt angles
        if (argComNumber < 3) argComNumber = 3;
        if (argComNumber > 4) argComNumber = 4;
        //cuz I only am using com 4 as a debug tool 
        if (argBAUD > 115200) argBAUD = 115200;
        if (argBAUD < 115200) argBAUD = 9600;
        next_time = Time.time;

        if(argUseCom)
        OpenCom(argComNumber, argBAUD);

    }
    void OpenCom(int argComNumber, int argBAUD)
    {
        string the_com = "";

        //foreach (string mysps in SerialPort.GetPortNames())
        //{
        //    print(mysps);
        //    if (mysps != "COM1") { the_com = mysps; break; }
        //}
        the_com = "COM" + argComNumber.ToString();

        //  sp = new SerialPort("\\\\.\\" + the_com, 115200);
        sp = new SerialPort("\\\\.\\" + the_com, argBAUD);

        sp.WriteBufferSize = 27;
        sp.NewLine = "\n";
        sp.WriteTimeout = 100;
        sp.DtrEnable = true;//prevents writing to overflow and freakout afterr 90 sec

        if (!sp.IsOpen)
        {
            Debug.Log("Opening " + the_com + ", baud " + argBAUD.ToString());
            sp.Open();
            // sp.ReadTimeout = 500;
            sp.Handshake = Handshake.None;
            if (sp.IsOpen)
            {
                Debug.Log("Open");
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
            if (SP_AllowStimulateNerve)
                Send_latestmessage();
        }
    }

    void Send_latestmessage() {

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
        else {
            Debug.Log("SP NULL!");

        }
    }

    IEnumerator ResetUART()
    {
        //  while (true){
        SP_AllowStimulateNerve = false;
        yield return new WaitForSecondsRealtime(2f);
            // Debug.Log(_message_);
            if (SP_WasInited)
            {
                CloseSerialPort();
            }
       // }
    }
    

        private void ComposeMessage_andWrite()
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
#if DEBUG_FRAMENUM
                        //_frameNumber++;
                        //_frameTopThousand = (_frameNumber / 1000) % 1000;
                        //if (_frameTopThousand > 999) _frameTopThousand = 0;
                        //_frameLowHundreds = _frameNumber % 1000;
                        // _message_ =test.Set_messageStringForMyServos(angleToSend, false, 180, false, _frameTopThousand, _frameLowHundreds);

#endif
                _message_ = ImpulseGenerator.Set_messageStringForMyServos(_thumb_L_ang_ToSend, _thumb_L_state_ToSend, _thumb_R_ang_ToSend, _thumb_R_state_ToSend, _commande_toSend, _debug_ToSend);
                _message_ = string.Concat('<', _message_, '>', '#');
                messagearrra = _message_.ToCharArray();
                sp.WriteLine(_message_);
                // Debug.Log("sent  " + _frameTopThousand + "" + _frameLowHundreds + "   " + _message_ + " " + _message_.Length ) ;
                sp.BaseStream.Flush();
            }
        }

    }

    private void OnDisable() { CloseSerialPort(); }

    private void CloseSerialPort()
    {
        if (sp != null)
        {
            Debug.Log("closing sp and flushing");
            sp.BaseStream.Flush();
            sp.Close();
            sp.Open();
        }
        SP_WasInited = false;
        SP_AllowStimulateNerve = false;
    }

    private void Update()
    {
        //float _y = TestTranslator.GiveMeanXvalueFor(TEstvalue);
        //Debug.Log(TEstvalue + " , " + _y);
    }
}
/*
 

 if (SP_AllowStimulateNerve)
            {
               
            }




  
 * */