using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BrawlPointsTargetTracker : MonoBehaviour
{
  /*  #region GIZMOSarea
    void OnDrawGizmos()
    {

        PubFrom = BlueTran.position;
        PubTo = GreenTran.position;
        PubAt = RedTran.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(BlueTran.position, 10);


        Gizmos.color = Color.red;
        Gizmos.DrawSphere(RedTran.position, 10);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GreenTran.position, 10);


       // Normalized_MoveDir_FromCenter = (PubTo - PubFrom).normalized;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, Normalized_MoveDir_FromCenter * 100);

       // FireDir_FromCenter = (PubAt - PubFrom);
       // Normalized_FireDir_FromCenter = FireDir_FromCenter.normalized;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, Normalized_FireDir_FromCenter * 100);

    }
    #endregion

    #region _private_Vars
    HandsActionsCoordinator _handActions;

    MiniMapManager _miniTargets;

 
    Vector3 tempPlayer;

    Vector3 _moveDir_v3;
    Vector3 _enemyDir_v3;

    CtrlpadIA controle;
    #endregion
    #region Public_Vars
    [SerializeField]
    public Vector3 PubFrom;
    [SerializeField]
    public Vector3 PubTo;
    [SerializeField]
    public Vector3 PubAt;
    [SerializeField]
    public Vector3 Normalized_MoveDir_FromCenter;
    [SerializeField]
    public Vector3 Normalized_FireDir_FromCenter;
 


    [SerializeField]
    public Vector3 PubFromV3;
    [SerializeField]
    public Vector3 PubToV3;
    [SerializeField]
    public Vector3 PubAtV3;


    public Transform BlueTran;
    public Transform GreenTran;
    public Transform RedTran;
    public bool useNearest;
    public Vector2 AnalLeft;
    public Vector2 AnalRight;
    public bool WalkIsDown;
    #endregion

    #region Enabled_Disable_Awake_Start_Destroy
    private void OnEnable()
    {
        controle = new CtrlpadIA();
        controle.Gameplay.Enable();
       
        controle.Gameplay.Move.performed += ctx => AnalLeft = ctx.ReadValue<Vector2>();
        controle.Gameplay.Move.canceled += ctx => AnalLeft =  Vector2.zero;

        controle.Gameplay.Rotate.performed += ctx => AnalRight = ctx.ReadValue<Vector2>();
        controle.Gameplay.Rotate.canceled += ctx => AnalRight = Vector2.zero;
        
        
        controle.Gameplay.TapGaget_Dpad_down.performed += ctx => Click_TAPGaget();
        controle.Gameplay.TapFire_Dpad_right.performed += ctx => Click_TAPtFire();
        controle.Gameplay.TapPower_Dpad_left.performed += ctx => Click_TAPPowerFire();

        controle.Gameplay.FastFire_B.performed += ctx => Click_FastFireAT();
        controle.Gameplay.FastPower_Y.performed += ctx => Click_FastPowerAT();
        // controle.Gameplay.MoveTog.WasPerformedThisFrame += ctx => WalkIsDown ;//  ToggleWalkSttopwalk();
    }
    private void OnDisable()
    {
        controle.Gameplay.Move.performed -= ctx => AnalLeft = ctx.ReadValue<Vector2>();
        controle.Gameplay.Move.canceled -= ctx => AnalLeft = Vector2.zero;

        controle.Gameplay.Rotate.performed -= ctx => AnalRight = ctx.ReadValue<Vector2>();
        controle.Gameplay.Rotate.canceled -= ctx => AnalRight = Vector2.zero;

        controle.Gameplay.TapGaget_Dpad_down.performed -= ctx => Click_TAPGaget();
        controle.Gameplay.TapFire_Dpad_right.performed -= ctx => Click_TAPtFire();
        controle.Gameplay.TapPower_Dpad_left.performed -= ctx => Click_TAPPowerFire();

        controle.Gameplay.FastFire_B.performed -= ctx => Click_FastFireAT();
        controle.Gameplay.FastPower_Y.performed -= ctx => Click_FastPowerAT();

        controle.Gameplay.Disable();

    }

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

    #region _private_methods

    void ToggleWalkSttopwalk()
    {
        Debug.Log("A togg walk");
    }

    void Click_TAPtFire() { Debug.Log("A taped Fire"); _handActions.FIRE_TAP(); }
    void Click_TAPPowerFire() { Debug.Log("A tapped Power"); _handActions.SUPER_TAP(); }
    void Click_TAPGaget() { Debug.Log("A tapped GAget"); _handActions.GADGET_TAP(); }

    void Click_FastFireAT() { Debug.Log("FastF AT"); _handActions.FIRE_FAST_Direction(); }
    void Click_FastPowerAT() { Debug.Log(" FPoewerAT"); _handActions.SUPER_FAST_Direction(); }

    void ToggleNearest() { useNearest = !useNearest; Debug.Log("A pressed"); }



    void Decision_Walking_Update()
    {

        _handActions._TwidleYoy_walk(true);
    }
    void Decision_Stop_WalkingUpdate()
    {

        _handActions._TwidleYoy_walk(false);
    }


 

  
    #endregion

    #region PUBLIC_Methods
    public void INITme_giveemminimap(MiniMapManager argMinimap, HandsActionsCoordinator argactoinHAnds)
    {
        _handActions = argactoinHAnds;
        _miniTargets = argMinimap;
       
        tempPlayer = Vector3.zero;// _miniTargets.Get_Playerlocations();
     
        _moveDir_v3 = Normalized_MoveDir_FromCenter;
        _enemyDir_v3 = Normalized_FireDir_FromCenter;
    }
    public Vector3 Get_V3_EnemyDir_() { return this._enemyDir_v3; }
    public Vector3 Get_V3_MoveDir_() { return this._moveDir_v3; }
    #endregion

    #region UPDATE
    public void Updateme(bool argusectrl)
    {

        if (argusectrl)
        {
            UseController();
        }

        else
            UseStrategy();



    }


    void UseController() {

        if (controle.Gameplay.WalkTog_LeftTrigger.WasPressedThisFrame()) { Debug.Log("was pressured in this frame"); WalkIsDown = true; }
        else
          if (controle.Gameplay.WalkTog_LeftTrigger.WasReleasedThisFrame()) { Debug.Log("was released in this frame"); WalkIsDown = false; }


        if (controle.Gameplay.AimFireTog_triggerright.WasPressedThisFrame()) { Debug.Log("AIMING held nowwas pressured in this frame");
  //do aim  
    
    _handActions.Fire_Aim_AT(); }
        else
        if (controle.Gameplay.AimFireTog_triggerright.WasReleasedThisFrame()) { Debug.Log("AIMING released in this frame");  _handActions.Break_AT_CloseEnd(); }


        if (controle.Gameplay.AimPowerTog_BumRight.WasPressedThisFrame()) { Debug.Log("AIMING POWheld nowwas pressured in this frame");     _handActions.SUPER_Aim_AT(); }
        else
        if (controle.Gameplay.AimPowerTog_BumRight.WasReleasedThisFrame()) { Debug.Log("AIMING POW released in this frame");  _handActions.Break_AT_CloseEnd(); }


        if (WalkIsDown)
            Decision_Walking_Update();
        else
            Decision_Stop_WalkingUpdate();
        // Construct_moveToAndfireAt();
        /// just to see the gizmo
        /// 
        PubFromV3 = Vector3.zero;
        PubAtV3 = new Vector3(AnalRight.x, AnalRight.y, 0);
        PubToV3 = new Vector3(AnalLeft.x, AnalLeft.y, 0);
        BlueTran.position = PubFromV3;
        GreenTran.position = PubAtV3 * 2;
        RedTran.position = PubToV3 * 2;

        Normalized_MoveDir_FromCenter = (PubToV3 - PubFromV3).normalized;
        Normalized_FireDir_FromCenter = (PubAtV3 - PubFromV3).normalized;

        _moveDir_v3 = Normalized_MoveDir_FromCenter;
        _enemyDir_v3 = Normalized_FireDir_FromCenter;// FireDir_FromCenter; 
    }

    void UseStrategy() {

        PubFromV3 = _miniTargets.Get_PlayerData().Get_inMap_Vector2();
        PubAtV3 = _miniTargets.get_enemiesData()[0].GetLocV2_inMap();
        PubToV3 = _miniTargets.get_enemiesData()[0].GetLocV2_inMap();
        //string deb1 = " regs " + Num_registered;
        //EventsManagerLib.CALL_debug1(deb1);


     


        //string deb3 = " circs " + List_Clean_detected_Circles.Count;
        //EventsManagerLib.CALL_debug3(deb3);

        Normalized_MoveDir_FromCenter = (PubToV3 - PubFromV3);
        Normalized_FireDir_FromCenter = (PubAtV3 - PubFromV3);

        _moveDir_v3 = Normalized_MoveDir_FromCenter.normalized;
        _enemyDir_v3 = Normalized_FireDir_FromCenter.normalized;// FireDir_FromCenter; 


       string deb1 = " dst to " + Normalized_MoveDir_FromCenter.magnitude;
       EventsManagerLib.CALL_debug1(deb1); //47 is inrange , 20 is tooclose 

        string deb2 = " dst at " + Normalized_FireDir_FromCenter.magnitude;
        EventsManagerLib.CALL_debug2(deb2);


    }
    #endregion

    */
}