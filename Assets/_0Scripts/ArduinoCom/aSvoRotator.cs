using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aSvoRotator : MonoBehaviour
{
     float Mangle;
     float Rectified_L_Angle = 360;

     float angle_R_G, angle_L_G ;

    public Transform SvoR_G, SvoL_G;


     float angle_R_D, angle_L_D;

    public Transform SvoR_D, SvoL_D;

    public GameObject PushStateObj_D, PushStateObj_G;
    Renderer Push_D_ren, Push_G_ren;
    bool _D_is_On, _G_is_On;
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

    private void Start()
    {
        Push_D_ren= PushStateObj_D.GetComponent<Renderer>();
        Push_G_ren= PushStateObj_G.GetComponent<Renderer>();
    }
    void Update_D_State()
    {
        if (_D_is_On)
        {
            Push_D_ren.material.color = Color.green;
        }
        else
        {
            Push_D_ren.material.color = Color.red;
        }

    }
    void Update_G_State()
    {
        if (_G_is_On)
        {
            Push_G_ren.material.color = Color.green;
        }
        else
        {
            Push_G_ren.material.color = Color.red;
        }

    }

    void Set_test_SRSL(float arg_RG, float arg_LG, float arg_RD, float argLD, bool argSolenG, bool argSolenD) {
        angle_R_G = arg_RG;
        angle_L_G= arg_LG;
        angle_R_D = arg_RD;
        angle_L_D= argLD;
        _G_is_On = argSolenG;
        _D_is_On = argSolenD;
    }

    void Set_test_HAnd(float arg_R, float arg_L,  bool argSolen, e_HandSide argSide)
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
    }

    // Update is called once per frame
    void Update()
    {
       

        //main Gauche
        SvoR_G.localEulerAngles = new Vector3(0, 0, angle_R_G);
        SvoL_G.localEulerAngles = new Vector3(0, 0, angle_L_G);

        //main Dright
        SvoR_D.localEulerAngles = new Vector3(0, 0, angle_R_D);
        SvoL_D.localEulerAngles = new Vector3(0, 0, angle_L_D);
        Update_D_State();
        Update_G_State();


    }
}
