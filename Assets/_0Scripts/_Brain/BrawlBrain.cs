using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlBrain : MonoBehaviour
{

    #region _private_Vars
    e_BrawGameState _GameState;

    BrainPlayActionDecider ActionsDEcider;
    BrawlPointsTargetTracker PointsTrack_Vectorizer;
    HandsActionsCoordinator HandsCoordinator;
    Joy_D _DROITE;
    Joy_G _GAUCHE;
    SimpleComm _CommBrainRef;
    MSG_composer magComp;

    bool coroutinIsRuning;
    bool can_startWritingToArduino;
    bool CanStartInited;
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
        ActionsDEcider = GetComponent<BrainPlayActionDecider>();
        PointsTrack_Vectorizer = GetComponent<BrawlPointsTargetTracker>();
        HandsCoordinator = GetComponent<HandsActionsCoordinator>();
        _DROITE = GetComponent<Joy_D>();
        _GAUCHE = GetComponent<Joy_G>();
        _GameState = e_BrawGameState.Loading;
        magComp = GetComponent<MSG_composer>();
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
    IEnumerator StartCOmmIn_3()
    {
        coroutinIsRuning = true;
        yield return new WaitForSeconds(10);
        Debug.Log("waited 2 seecons");
        if (!CanStartInited)
        {
            Debug.Log("state nont can start , must turnonkey");
            can_startWritingToArduino = true;
            CanStartInited = true;//and neveragain until reset
            _CommBrainRef.AllowSerialWrite(can_startWritingToArduino);
            magComp.InitME_withCom(_CommBrainRef);
        }
        coroutinIsRuning = false;
    }
    #endregion
    #region PUBLIC_Methods

    public void INITme_giveemminimap(MiniMapManager argMinimap, SimpleComm argComm, bool argUseCOmm)
    {
        PointsTrack_Vectorizer.INITme_giveemminimap(argMinimap);
        HandsCoordinator.Initme_givemeMyHands(_DROITE, _GAUCHE);
        ActionsDEcider.InitmePlz(HandsCoordinator, PointsTrack_Vectorizer);


        _CommBrainRef = argComm;
        if (argUseCOmm)
        {
            _CommBrainRef.InitializeMe(3, 115200);
        }
        else
        {
            Debug.LogWarning("COmm not open");
        }

        StartCoroutine(StartCOmmIn_3());
    }
    public void ResetComm()
    {
        Debug.Log("reseting actions");
        can_startWritingToArduino = false;
        CanStartInited = false;
        _CommBrainRef.AllowSerialWrite(can_startWritingToArduino);
        if (!coroutinIsRuning) { StartCoroutine(StartCOmmIn_3()); }

    }

    #endregion

    #region UPDATE
    void Update()
    {
        if (!can_startWritingToArduino) return;
        ActionsDEcider.RunDecisionMaking_and_TriggerActions();
    }
    #endregion
}
