using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class BrawlPointsTargetTracker : MonoBehaviour
{
    #region GIZMOSarea
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

    MiniMapManager _miniTargets;

    Vector3[] temptargets;
    Vector3[] tempCardinal;

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
    public Vector3 FireDir_FromCenter;



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
    #endregion

    #region Enabled_Disable_Awake_Start_Destroy
    private void OnEnable()
    {
        controle = new CtrlpadIA();
        controle.Gameplay.Enable();
        controle.Gameplay.Grow.performed += ctx => ToggleNearest();
        controle.Gameplay.Move.performed += ctx => AnalLeft = ctx.ReadValue<Vector2>();
        controle.Gameplay.Rotate.performed += ctx => AnalRight = ctx.ReadValue<Vector2>();
    }
    private void OnDisable()
    {
        controle.Gameplay.Grow.performed -= ctx => ToggleNearest();
        controle.Gameplay.Move.performed -= ctx => AnalLeft = ctx.ReadValue<Vector2>();
        controle.Gameplay.Rotate.performed -= ctx => AnalRight = ctx.ReadValue<Vector2>();
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

    void ToggleNearest() { useNearest = !useNearest; Debug.Log("A pressed"); }
    
    void Construct_moveToAndfireAt() {

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ToggleNearest();


        }

        if (useNearest)
            ConstructwithNEarest();
        else
            ConstructWihDirecttarget();

    }

    void ConstructwithNEarest() {
        PubFromV3 = _miniTargets.Get_Playerlocations();
        int numberoftargets = _miniTargets.Get_Final_Enemilocations().Length;
        int indexOfNearestEnemy = 0;
        float DistanceToNEarestEnemy = 991337;
        for (int i = 0; i < numberoftargets; i++)
        {
            Vector3 VtoEnemy = _miniTargets.Get_Final_Enemilocations()[i] - PubFromV3;
            if (Vector3.Magnitude(VtoEnemy) < DistanceToNEarestEnemy)
            {
                DistanceToNEarestEnemy = Vector3.Magnitude(VtoEnemy);
                indexOfNearestEnemy = i;
            }
        }

        PubAtV3 = _miniTargets.Get_Final_Enemilocations()[indexOfNearestEnemy];
        PubToV3 = _miniTargets.Get_Final_Enemilocations()[indexOfNearestEnemy];
    }
    int indexOfChosenEnemy = 0;
    void ConstructWihDirecttarget() {
        PubFromV3 = _miniTargets.Get_Playerlocations();
        int numberoftargets = _miniTargets.Get_Final_Enemilocations().Length;
        



        if (Input.GetKeyDown(KeyCode.I))
        {
            indexOfChosenEnemy = 2;
        }
        else
                  if (Input.GetKeyDown(KeyCode.O))
        {
            indexOfChosenEnemy = 3;
        }
        else
                  if (Input.GetKeyDown(KeyCode.L))
        {
            indexOfChosenEnemy = 1;
        }
        else
        if (Input.GetKeyDown(KeyCode.K))
        {
            indexOfChosenEnemy = 0;
        }



        PubAtV3 = _miniTargets.Get_Final_Enemilocations()[indexOfChosenEnemy];
        PubToV3 = _miniTargets.Get_Final_Enemilocations()[indexOfChosenEnemy];

    }
    float Get_360_angle(Vector3 argFrom, Vector3 argto)
    {
        Vector3 Temp_NormalizedFromCenter = (argFrom - argto).normalized;
        var angle = Mathf.Atan2(Temp_NormalizedFromCenter.x, Temp_NormalizedFromCenter.y) * Mathf.Rad2Deg;
        float angle360 = (angle + 180) % 360;
        return angle360;
    }
    #endregion

    #region PUBLIC_Methods
    public void INITme_giveemminimap(MiniMapManager argMinimap)
    {
        _miniTargets = argMinimap;
        temptargets = _miniTargets.Get_Final_Enemilocations();
        tempPlayer = Vector3.zero;// _miniTargets.Get_Playerlocations();
        tempCardinal = _miniTargets.Get_Cardinallocations();
        _moveDir_v3 = Normalized_MoveDir_FromCenter;
        _enemyDir_v3 = FireDir_FromCenter;
    }
    public Vector3 Get_V3_EnemyDir_() { return this._enemyDir_v3; }
    public Vector3 Get_V3_MoveDir_() { return this._moveDir_v3; }
    #endregion

    #region UPDATE
    void Update()
    {
        Construct_moveToAndfireAt();
        /// just to see the gizmo
        /// 
        PubFromV3 = Vector3.zero;
        PubAtV3 = new Vector3(AnalRight.x, AnalRight.y,0);
        PubToV3 = new Vector3(AnalLeft.x, AnalLeft.y, 0);
        BlueTran.position = PubFromV3;
         GreenTran.position = PubAtV3;
          RedTran.position = PubToV3;

        Normalized_MoveDir_FromCenter = (PubToV3 - PubFromV3).normalized;
        FireDir_FromCenter = (PubAt - PubFrom);

        _moveDir_v3 = Normalized_MoveDir_FromCenter;
        _enemyDir_v3 = Normalized_FireDir_FromCenter;// FireDir_FromCenter; 
    }
    #endregion
}