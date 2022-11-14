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
   
    public bool UseCursor;
    bool wasReset = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!UseCursor)
        {
            transform.Rotate(0, 0, Input.GetAxis("Horizontal") * -speedrot * Time.deltaTime);
            transform.GetChild(0).transform.localPosition += new Vector3(Input.GetAxis("Vertical") * -speedrot * Time.deltaTime, 0, 0) * 10 * Time.deltaTime;
            wasReset = false;
        }

        else {
            if (!wasReset) {
                wasReset = true;
                newX = 25f;
                newY = 0f;
            }
            newX += Input.GetAxis("Horizontal") * 1.5f * Time.deltaTime;
            newY += Input.GetAxis("Vertical") * 1.5f * Time.deltaTime;


            if (newX < 2.5f) {
                newX = 2.5f;
            }

            if (newX > 7.6f)
            {
                newX = 7.6f;
            }

            if (newY < -5.0f)
            {
                newY = -5.0f;
            }

            if (newY > 5.0f)
            {
                newY = 5.0f;
            }

            CursorRoot.transform.localPosition = new Vector3(newY, newX, 0);

        }
    }
}
