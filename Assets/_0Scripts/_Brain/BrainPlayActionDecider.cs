using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 
/// This is where the decisions are made
/// 
/// </summary>
public class BrainPlayActionDecider : MonoBehaviour
{

    #region _private_Vars
    HandsActionsCoordinator HandsCoordinator;
    BrawlPointsTargetTracker _PointsTrack_Vectorizer;

    Vector3 _rawEnemyDir_normed;
    Vector3 _rawMoveDirDir_normed;
    float CloseRange = 1.5f;
    float MedRange = 4.5f;
    float FarRange = 6.5f;
    bool doprint = false;
    bool WalkOn;
    #endregion

    #region Public_Vars

    #endregion

    #region Enabled_Disable_Awake_Start_Destroy
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

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
    void Decision_Walking_Update()
    {
        if (doprint) print("walk");
        HandsCoordinator._TwidleYoy_walk(true);
    }
    void Decision_Stop_WalkingUpdate()
    {
        if (doprint) print("stopwalk");
        HandsCoordinator._TwidleYoy_walk(false);
    }
    #endregion

    #region PUBLIC_Methods
    public void InitmePlz(HandsActionsCoordinator argCoordinatorOfHAnds, BrawlPointsTargetTracker argPointsTrack_Vectorizer)
    {
        e_ButtonLocationType _thibutton = e_ButtonLocationType._2_GadgetFire;
        HandsCoordinator = argCoordinatorOfHAnds;
        _PointsTrack_Vectorizer = argPointsTrack_Vectorizer;
    }

    public void RunDecisionMaking_and_TriggerActions()
    {
        _rawEnemyDir_normed = _PointsTrack_Vectorizer.Get_V3_EnemyDir_();
        _rawMoveDirDir_normed = _PointsTrack_Vectorizer.Get_V3_MoveDir_();
        HandsCoordinator.Update_2Vectors(_rawMoveDirDir_normed, _rawEnemyDir_normed);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            HandsCoordinator.FIRE_TAP();
        }
        else
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HandsCoordinator.SUPER_TAP();
        }
        else
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            HandsCoordinator.GADGET_TAP();
        }
        else
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            HandsCoordinator.FIRE_FAST_Direction();
        }
        else
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            HandsCoordinator.SUPER_FAST_Direction();
        }
        else
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            HandsCoordinator.Fire_Aim_AT();
        }
        else
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            HandsCoordinator.SUPER_Aim_AT();
        }
        else
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandsCoordinator.Break_AT_CloseEnd();
        }
        else
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {

            WalkOn = !WalkOn;
            if (doprint)
            {
                if (WalkOn)
                    print("key9 walk on");
                else
                    print("key9 walkoff");
            }
        }

        if (WalkOn)
            Decision_Walking_Update();
        else
            Decision_Stop_WalkingUpdate();

    }
    #endregion

    #region UPDATE_______________________DRIVEN
    //void Update()
    //{

    //}
    #endregion
}
