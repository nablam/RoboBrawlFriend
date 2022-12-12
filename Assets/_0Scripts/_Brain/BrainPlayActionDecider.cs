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
  
    #endregion

    #region PUBLIC_Methods
    public void InitmePlz(HandsActionsCoordinator argCoordinatorOfHAnds)
    {
        e_ButtonLocationType _thibutton = e_ButtonLocationType._2_GadgetFire;
        HandsCoordinator = argCoordinatorOfHAnds;
       
    }

    public void RunDecisionMaking_and_TriggerActions( bool argUseCtrl )
    {
         
         
       
       
    }
    #endregion

    #region UPDATE_______________________DRIVEN
    //void Update()
    //{

    //}
    #endregion
}
