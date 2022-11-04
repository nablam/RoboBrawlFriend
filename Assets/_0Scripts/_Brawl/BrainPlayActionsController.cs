using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainPlayActionsController : MonoBehaviour
{
    // Start is called before the first frame update
    public void Fire_inDirection(Vector3 argTarget) { }
    public void Fire_Tap() { }
    public void FirePower_inDirection(Vector3 argTarget) { }
    public void FirePower_Tap() { }
    public void FireGadget_Tap() { }

    public void Go_ToDirection(Vector3 argTarget) { }
    public void WaitGo_CenterSelf() { }


    //Buffer Of Last DirectionVEctors and Last Solinoid states
    Vector3 _curRawDiectionVEctor;


}
