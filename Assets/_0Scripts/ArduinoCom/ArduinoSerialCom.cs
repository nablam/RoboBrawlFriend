using UnityEngine;
using System.IO.Ports; //needed to change the api compatibility to .NET Framework . was originally on .NET Standard 2.1
using System;
using System.Collections;

public class ArduinoSerialCom : MonoBehaviour
{


    SerialPort sp;
    float next_time;
    bool WasInited = false;

    public bool AllowManual;
    string _message_;
    string hard_message = "<053149000074133000001067>#";
    int avergageINT;

    public bool DoRUN = true;
    public aRROWrOTATOR myarrow;
    public void _Init_()
    {
        string the_com = "";
        next_time = Time.time;

        //foreach (string mysps in SerialPort.GetPortNames())
        //{
        //    print(mysps);
        //    if (mysps != "COM1") { the_com = mysps; break; }
        //}
        the_com = "COM3";

        sp = new SerialPort("\\\\.\\" + the_com, 115200);
        sp.WriteBufferSize = 27;
        sp.NewLine = "\n";
        sp.WriteTimeout = 100;
        sp.DtrEnable = true;

        if (!sp.IsOpen)
        {
            Debug.Log("Opening " + the_com + ", baud 115200");
            sp.Open();
            // sp.ReadTimeout = 500;

            sp.Handshake = Handshake.None;
            if (sp.IsOpen)
            {
                Debug.Log("Open");
                WasInited = true;
                StartCoroutine(FireUART());
               // StartCoroutine(ResetUART());
                
            }
        }

    }


    private void OnDisable()
    {
        Debug.Log("closing");
        sp.Close();
    }

    public string tmp1;
    public int tmpint1;
    float angleToSend;

    int _frameNumber = 0;
    int _frameTopThousand = 0;
    int _frameLowHundreds = 0;
    char[] messagearrra;
    IEnumerator FireUART()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.02f);


            // Debug.Log(_message_);

            if (DoRUN)
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
                        // sp.BaseStream.Flush();


                        _frameNumber++;



                        _frameTopThousand = (_frameNumber / 1000) % 1000;
                        if (_frameTopThousand > 999) _frameTopThousand = 0;
                        _frameLowHundreds = _frameNumber % 1000;
                        //  if (_frameLowHundreds > 999) _frameLowHundreds = 0;

                        _message_ =test.Set_messageStringForMyServos(angleToSend, false, 180, false, _frameTopThousand, _frameLowHundreds);
                        _message_ = string.Concat('<', _message_, '>', '#');
                        messagearrra = _message_.ToCharArray();
                        sp.WriteLine(_message_);
                       //sp.Write(messagearrra, 0,1);//when received it will be size 27 :  < + 24 + > + '\n'
                                                    // sp.BaseStream.Flush();
                                                    //if (!string.IsNullOrEmpty(_message_))
                        Debug.Log("sent  " + _frameTopThousand + "" + _frameLowHundreds + "   " + _message_ + " " + _message_.Length ) ;

                        sp.BaseStream.Flush();

                    //sp.Write((  avergageINT.ToString("D3")));
                    // sp.Write(("S"+Percentage.ToString("D3")));

                    }

                }



            }
        }

    }


    IEnumerator ResetUART()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(30f);


            // Debug.Log(_message_);

            if (DoRUN)
            {

                if (sp != null)
                {
                    sp.BaseStream.Flush();
                    sp.Close();
                    
                    sp.Open();
                }



            }
        }

    }
    void Updatex()
    {
        angleToSend = myarrow.VewAng;

        if (!WasInited) { return; }



        if (false)
        {

            if (Time.time > next_time)
            {

                _frameNumber++;



                _frameTopThousand = (_frameNumber / 1000) % 1000;
                if (_frameTopThousand > 999) _frameTopThousand = 0;
                _frameLowHundreds = _frameNumber % 1000;
                //  if (_frameLowHundreds > 999) _frameLowHundreds = 0;

                _message_ = test.Set_messageStringForMyServos(angleToSend, false, 180, false, _frameTopThousand, _frameLowHundreds);
                _message_ = string.Concat('<', _message_, '>');
                // Debug.Log(_message_);

                if (DoRUN)
                {
                    if (!sp.IsOpen)
                    {
                        sp.Open();
                        Debug.Log("opened sp");
                    }
                    if (sp.IsOpen)
                    {
                        // sp.BaseStream.Flush();
                        if (!string.IsNullOrEmpty(_message_))
                            if (sp != null)
                            {
                                sp.WriteLine(_message_);//when received it will be size 27 :  < + 24 + > + '\n'
                                                        // sp.BaseStream.Flush();
                                                        //if (!string.IsNullOrEmpty(_message_))
                                Debug.Log("sent  " + _frameTopThousand + "" + _frameLowHundreds + "   " + _message_);
                            }
                        //sp.Write((  avergageINT.ToString("D3")));
                        // sp.Write(("S"+Percentage.ToString("D3")));

                    }
                }


                //next_time = Time.time + 0.1f;
                next_time = Time.time + 0.02f;
                // sp.BaseStream.Flush();
                // _message_ = "";
            }
        }

    }


    VectorToServoAnglesConvertor test;
    private void Start()
    {
        test = new VectorToServoAnglesConvertor(30, 90, 't', 10);
        _Init_();
    }

    private void FixedUpdate()
    {
        Updatex();
    }

}





/*

        if (AllowManual)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                 Debug.Log("press 1");
                _message_ = tmp1 ;
            }
            else
            if (Input.GetKey(KeyCode.Alpha2))
            {
                Debug.Log("press 2");
              //  _message_ = tmpint1.ToString();
              
            }
            //else
            //if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    Debug.Log("held 3 ");

            //    _message_ = tmp1+'\n';
            //}
            //else
            //if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    Debug.Log("pressed 4 ");
            //    _message_ = test.Set_messageStringForMyServos(30, false, 40, false, 888, 1);// "010020030040111000444222";
            //}
            //else
            //if (Input.GetKeyDown(KeyCode.Alpha5))
            //{
            //    Debug.Log("pressed  5");
            //    _message_ = _message_ = test.Set_messageStringForMyServos(50, true, 180, true, 888, 12); // "010020030040111000777333";
            //}
            //else
            //if (Input.GetKeyDown(KeyCode.Alpha6))
            //{
            //    Debug.Log("held 6 ");
            //    _message_ = "12312313231321321321";
            //}


            else
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _message_ = test.Set_messageStringForMyServos(angleToSend, true, 180, true, 888, 12);
                _message_ = '<' + _message_ + '>';

                Debug.Log("press space");
                if (!sp.IsOpen)
                {
                    sp.Open();
                    print("opened sp");
                }
                if (sp.IsOpen)
                {
                    if (!string.IsNullOrEmpty(_message_))
                        if (sp != null)
                        {
                            sp.Write(_message_);
                            // sp.BaseStream.Flush();
                            if (!string.IsNullOrEmpty(_message_))
                                Debug.Log("sent  " + _message_);
                        }
                    //sp.Write((  avergageINT.ToString("D3")));
                    // sp.Write(("S"+Percentage.ToString("D3")));

                }
                _message_ = "";
            }
        }

*/