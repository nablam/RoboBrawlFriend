using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlueCube_KB : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedrot = 90;
  
    float newX,newY; //for serbo 0basedX

    public GameObject CursorRoot;

    public GameObject AroundPlayerRoot;

    public GameObject PlayerRoot;


    public GameObject BLUECUBE;

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

        BLUECUBE.transform.localPosition += new Vector3(Input.GetAxis("Horizontal") * speedrot * Time.deltaTime, Input.GetAxis("Vertical") * speedrot * Time.deltaTime, 0) * 10 * Time.deltaTime;
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
