using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aRROWrOTATOR : MonoBehaviour
{
    Vector3 CurEulAng;
    //public float RotSpeed = 40;
    float rx, ry, rz;

    float rotStep = 0.01f;
    public int multiplyer=1;

    float curRotAngle = 0;

    public float VewAng ;
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        curRotAngle += (rotStep * multiplyer);

        if (curRotAngle > 359.9) curRotAngle = 0f;
        if (curRotAngle < 0) curRotAngle = 359.9f;

        VewAng = curRotAngle;

         CurEulAng = new Vector3(0,0, curRotAngle) ;
        this.transform.eulerAngles = CurEulAng;
    }
}
