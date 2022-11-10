using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorWithKEYbard : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedrot = 90;
    public float speedmove = 10;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,Input.GetAxis("Horizontal") * -speedrot * Time.deltaTime);
        transform.GetChild(0).transform.localPosition += new Vector3(Input.GetAxis("Vertical") * -speedrot * Time.deltaTime, 0,0) * 10 * Time.deltaTime;
    }
}
