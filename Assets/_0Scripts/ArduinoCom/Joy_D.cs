using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joy_D : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Update_FireAtV3(  Vector3 argFireVec)
    {

       
        _Local_Fire_v3_Normed = argFireVec.normalized;
    }
     
     Vector3 _Local_Fire_v3_Normed;

    
             
}
