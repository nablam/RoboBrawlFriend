using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorWithKEYbard : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedrot = 90;
    public float speedmove = 10;
    public float newX,newY; //for serbo 0basedX

    public GameObject CursorRoot;

    public GameObject AroundPlayerRoot;

    public GameObject PlayerRoot;

    Vector3 PlaerDir;

    public bool UseCursor;
    bool wasReset = false;

    public Vector3 TheDIr;

    public bool ZeroOut;
    void Start()
    {
        newY = 2.5f;
        TheDIr = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {


        //if (!UseCursor)
        //{
        AroundPlayerRoot.transform.Rotate(0, 0, Input.GetAxis("Horizontal") * -speedrot * Time.deltaTime);
        AroundPlayerRoot.transform.GetChild(0).transform.localPosition += new Vector3(Input.GetAxis("Vertical") * -speedrot * Time.deltaTime, 0, 0) * 10 * Time.deltaTime;


        PlaerDir = (PlayerRoot.transform.position - AroundPlayerRoot.transform.position).normalized;
        if (ZeroOut)
         {
         PlaerDir = Vector3.zero;
         }


        //    TheDIr.x = newX;
        //    TheDIr.y = newY;

        //    CursorRoot.transform.localPosition = new Vector3(newX, newY, 0);
        CursorRoot.transform.localPosition = PlaerDir;

        TheDIr = PlaerDir;
        //    wasReset = false;


        //}

        //else {
        //    if (!wasReset) {
        //        wasReset = true;
        //        newX =0f;
        //        newY = 2.5f;
        //    }




        //    if (ZeroOut)
        //    {
        //        newX = 0;
        //        newY = 0;
        //    }
        //    else
        //    {
        //        newX += Input.GetAxis("Horizontal") * 2.5f * Time.deltaTime;
        //        newY += Input.GetAxis("Vertical") * 2.5f * Time.deltaTime;
        //    }


        //    PlaerDir = PlayerRoot.transform.position - AroundPlayerRoot.transform.position;
        //    TheDIr.x = newX;
        //    TheDIr.y = newY;






        //    //filterX();
        //    //   FilterY();


        //    CursorRoot.transform.localPosition = new Vector3( newX,newY, 0);

        //}
    }


    void filterX()
    {
        if (newY < 2.5f)
        {
            newY = 2.5f;
        }

        if (newY > 7.6f)
        {
            newY = 7.6f;
        }
    }
    void FilterY() {

        if (newX < -5.0f)
        {
            newX = -5.0f;
        }

        if (newX > 5.0f)
        {
            newX = 5.0f;
        }

    }
}
