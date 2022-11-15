using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MSG_composer : MonoBehaviour
{
    float angle_R_G, angle_L_G;
    float angle_R_D, angle_L_D;
    bool _D_is_On, _G_is_On;
    string str_S0, str_S1, str_solenoid_G, str_S3, str_S4, str_solenoid_D, str_comand, str_debug;
    string[] MessageArray;
    string hard_90message = "090090000090090000000000";
    SimpleComm _mycom;
    bool wasinited = false;

    public TMP_Text tb_commstring;

    //  MSG:  [0]BL      [1]TL       [2]LP     [3]BR       [4]TR       [5]LR
    //  msg:  angle_R_G  angle_L_G   _G_is_On  angle_R_D   angle_L_D    _D_is_On

    //                     120                      60
    //  ┌──────────────────┐   x                xx    ┌──────────────────┐ 
    //  │                  │xxx                  xxx  │                  │
    //  │      SL         xxx                       xxx    SR            │ 
    //  │              xx│                          xx    [4]TR          │ 
    //  │    [1]TL         │                          │                  │ 
    //  │                  │                          │                  │ 
    //  └──────────────────┘                          └──────────────────┘ 
    //      
    //                      [2]LP            [5]LR
    //
    //  ┌──────────────────┐                          ┌──────────────────┐ 
    //  │                  │                          │                  │ 
    //  │                  │                          │xx   SL           │ 
    //  │      SR       xx │                          xx    [3]BR        │ 
    //  │  [0]BL         xx│                         xx                  │ 
    //  └─────────────────xx                       xxx───────────────────┘ 
    //                     xxx                    xx                       
    //                  60    xx                   120                       the hand side doesnt mater  always SR= 60 SL =120




    private void OnEnable()
    {
        // EventsManagerLib.On_SRSLBroadcast += Set_test_SRSL;
        EventsManagerLib.On_Hand_Broadcast += Set_test_HAnd;
    }

    private void OnDisable()
    {
        //   EventsManagerLib.On_SRSLBroadcast -= Set_test_SRSL;
        EventsManagerLib.On_Hand_Broadcast -= Set_test_HAnd;


    }
    void Set_test_HAnd(float arg_R, float arg_L, bool argSolen, e_HandSide argSide)
    {
        if (argSide == e_HandSide.LEFT_hand)
        {
            angle_R_G = arg_R;
            angle_L_G = arg_L;
            _G_is_On = argSolen;

        }
        else
        {
            angle_R_D = arg_R;
            angle_L_D = arg_L;
            _D_is_On = argSolen;
        }

        if (wasinited)
        {
            UpdateMSGBuffer();
            _mycom.Update_Message(string.Concat(MessageArray));
          
        }
    }

    // Start is called before the first frame update
    public void InitME_withCom(SimpleComm argCom)
    {
        wasinited = true;
        _mycom = argCom;
        angle_R_G = 60; angle_L_G = 120; _G_is_On = false; angle_R_D = 120; angle_L_D = 60; _D_is_On = false;
        str_S0 = "60"; str_S1 = "120"; str_solenoid_G = "000"; str_S3 = "120"; str_S4 = "60"; str_solenoid_D = "000";
        str_comand = "000"; str_debug = "000";
        MessageArray = new string[8];
        UpdateMSGBuffer();

    }
    private void Awake()
    {
       
    }
    void UpdateMSGBuffer() {
       
        //  MSG:  [0]BL      [1]TL       [2]LP     [3]BR       [4]TR       [5]LR
        //  msg:  angle_R_G  angle_L_G   _G_is_On  angle_R_D   angle_L_D    _D_is_On
        str_S0 = angle_R_G.ToString("000");
        str_S1 = angle_L_G.ToString("000");
        str_solenoid_G = "000";
        if (_G_is_On == true)
        {
            str_solenoid_G = "111";
        }
        MessageArray[0] = str_S0;
        MessageArray[1] = str_S1;
        MessageArray[2] = str_solenoid_G;


        str_S3 = angle_R_D.ToString("000");
        str_S4 = angle_L_D.ToString("000");
        str_solenoid_D = "000";
        if (_D_is_On == true)
        {
            str_solenoid_D = "111";
        }
        ///
        //  WEIRDNESS 
        ///
        MessageArray[3] = str_S4;   //must swp these 
        MessageArray[4] = str_S3;   // dont want to dive too deep 
        MessageArray[5] = str_solenoid_D;
        MessageArray[6] = str_comand;
        MessageArray[7] = str_debug;

    }

    // Update is called once per frame
    void Update()
    {
        if (wasinited)
            tb_commstring.text = string.Concat(MessageArray);
        else
            tb_commstring.text = "msg c_comp not inited";
    }
}
