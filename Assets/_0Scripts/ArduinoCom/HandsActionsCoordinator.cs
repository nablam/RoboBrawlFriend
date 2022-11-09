using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsActionsCoordinator : MonoBehaviour
{

    LaMainDroite _myhandDROITE;
    LaMainGauche _myhandGAUCHE;

    public void Initme_givemeMyHands(LaMainDroite arg_DROITE,LaMainGauche arg_GAUCHE )
    {
        _myhandDROITE = arg_DROITE;
        _myhandGAUCHE = arg_GAUCHE;
       
    }


    //---------------------WALKING _ LEFT HAND GAUCHE---------------------------------------
    //---------------MAIN
    public void WALK_Openended_TO_Direction(Vector3 arg_v3_Normed) {
        _myhandGAUCHE.DOACTIONWalkThumb(e_ButtonLocationType.Main, arg_v3_Normed);
    }


    //---------------------ATTACKING _ RIGHT HAND DROITE------------------------------------
    //---------------MAIN
    public void FIRE_TAP() { }
    public void FIRE_FAST_Direction(Vector3 argTarget) { }
    public void FIRE_Openended_AT_Direction(Vector3 argTarget) { }

    //---------------SEC
    public void SUPER_TAP() { }
    public void SUPER_FAST_Direction(Vector3 argTarget) { }
    public void SUPER_Openended_AT_Direction(Vector3 argTarget) { }
    //---------------TER
    public void GADGET_TAP() { }


    public void Break_TO_CloseEnd(Vector3 argTarget) { }
    public void Break_AT_CloseEnd(Vector3 argTarget) { }
    // CoroutineCoordinator_Lefthand
    //pass : ACTION HOME_Main  argNormalized






}
