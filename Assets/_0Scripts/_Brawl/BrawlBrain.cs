using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlBrain : MonoBehaviour
{
    public int s = 100;

    [SerializeField]
    public Vector3 PubFrom;
    [SerializeField]

    public Vector3 PubTo;
    [SerializeField]

    public Vector3 NormalizedFromCenter;


    public Transform GreenTran;
    public Transform RedTran;

    Vector3[] temptargets;
    Vector3[] tempCardinal;
    Vector3 tempPlayer;
    void OnDrawGizmos()
    {

        PubFrom = GreenTran.position;
        PubTo = RedTran.position;

        NormalizedFromCenter = (PubTo - PubFrom).normalized;
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, NormalizedFromCenter);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, NormalizedFromCenter*100);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GreenTran.position, 10);


        Gizmos.color = Color.red;
        Gizmos.DrawSphere(RedTran.position, 10);

       // var angle = Mathf.Atan2(NormalizedFromCenter.x, NormalizedFromCenter.y) * Mathf.Rad2Deg;
       // float angle360 = (angle + 180) % 360;

       // float theDotFL = Vector3.Dot(PubFrom, PubTo);
       // theDotFL = theDotFL / (PubFrom.magnitude * PubTo.magnitude);
       // float varacos = Mathf.Acos(theDotFL);
       // float VarAng = varacos * 180 / Mathf.PI;

       // float fAngle = Vector3.Cross(PubFrom.normalized, PubTo.normalized).y;


       // // Convert to -180 to +180 degrees
       // fAngle *= 180.0f;


       // var v1 = PubFrom;// - this.transform.position;
       // var v2 = PubTo;// - this.transform.position;
       // var axis = Vector3.Cross(v1, v2).normalized;
       //// var axis = Vector3.up;
       // var a = AngleOffAroundAxis(v1, v2, axis, false);


       // Gizmos.color = Color.black;
       // Gizmos.DrawLine(this.transform.position, PubFrom);
       // Gizmos.color = Color.blue;
       // Gizmos.DrawLine(this.transform.position, PubTo);
       // Gizmos.color = Color.red;
       // Gizmos.DrawLine(this.transform.position, this.transform.position + axis * 20f);

       //float SingenAng360= SignedAngleBetween( v2, v1, Vector3.forward);
       // Debug.Log(angle360);
       // //Debug.Log(a);



        // Debug.Log(VarAng);
    }

    public float Get_360_angle() {

        NormalizedFromCenter = (PubTo - PubFrom).normalized;
        var angle = Mathf.Atan2(NormalizedFromCenter.x, NormalizedFromCenter.y) * Mathf.Rad2Deg;
        float angle360 = (angle + 180) % 360;
        Debug.Log(angle360);
        return angle360;
   
    }

    float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis, bool clockwise = false)
    {
        Vector3 right;
        if (clockwise)
        {
            right = Vector3.Cross(forward, axis);
            forward = Vector3.Cross(axis, right);
        }
        else
        {
            right = Vector3.Cross(axis, forward);
            forward = Vector3.Cross(right, axis);
        }
        return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * Mathf.Rad2Deg;
    }



    float SignedAngleBetween(Vector3 a, Vector3 b, Vector3 n)
    {
        // angle in [0,180]
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        // angle in [-179,180]
        float signed_angle = angle * sign;

        // angle in [0,360] (not used but included here for completeness)
        float angle360 =  (signed_angle + 180) % 360;

        return angle360;
    }
    MinimapTest _miniTargets;
    public void INITme_giveemminimap(MinimapTest argMinimap) {
        _miniTargets = argMinimap;
        temptargets = _miniTargets.Get_Final_Enemilocations();
        tempPlayer = _miniTargets.Get_Playerlocations();
        tempCardinal = _miniTargets.Get_Cardinallocations();
    }


    public void MoveTo(Vector3 argTarget) { 
    
        
    }
    public int PubCardinalIndex07; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PubCardinalIndex07 > 7) PubCardinalIndex07 = 7;
        if (PubCardinalIndex07 < 1) PubCardinalIndex07 = 0;


        GreenTran.position= tempCardinal[8];

        RedTran.position = tempCardinal[PubCardinalIndex07];
        //temptargets = _miniTargets.Get_Final_Enemilocations();
        //tempPlayer = _miniTargets.Get_Playerlocations();
        //tempCardinal = _miniTargets.Get_Cardinallocations();


    }
}
