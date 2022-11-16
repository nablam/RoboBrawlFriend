using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


        Normalized_MoveDir_FromCenter = (PubTo - PubFrom).normalized;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, Normalized_MoveDir_FromCenter * 100);

        FireDir_FromCenter = (PubAt - PubFrom);
        Normalized_FireDir_FromCenter = FireDir_FromCenter.normalized;
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, Normalized_FireDir_FromCenter * 100);

    }
    #endregion

    #region _private_Vars

    MinimapTest _miniTargets;

    Vector3[] temptargets;
    Vector3[] tempCardinal;

    Vector3 tempPlayer;

    Vector3 _moveDir_v3;
    Vector3 _enemyDir_v3;

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


    public Transform BlueTran;
    public Transform GreenTran;
    public Transform RedTran;
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
    float Get_360_angle(Vector3 argFrom, Vector3 argto)
    {
        Vector3 Temp_NormalizedFromCenter = (argFrom - argto).normalized;
        var angle = Mathf.Atan2(Temp_NormalizedFromCenter.x, Temp_NormalizedFromCenter.y) * Mathf.Rad2Deg;
        float angle360 = (angle + 180) % 360;
        return angle360;
    }
    #endregion

    #region PUBLIC_Methods
    public void INITme_giveemminimap(MinimapTest argMinimap)
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
        _moveDir_v3 = Normalized_MoveDir_FromCenter;
        _enemyDir_v3 = Normalized_FireDir_FromCenter;// FireDir_FromCenter; 
    }
    #endregion
}