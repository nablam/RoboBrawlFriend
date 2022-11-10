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
    public void Update_2Vectors(Vector3 arg_moveVEc, Vector3 argFireVec) {
           _myhandDROITE.Update_MoveandFireVEctors(  arg_moveVEc, argFireVec);
            _myhandGAUCHE.Update_MoveandFireVEctors(arg_moveVEc, argFireVec);
    }

    //---------------------WALKING _ LEFT HAND GAUCHE---------------------------------------
    //---------------MAIN
    public void WALK_Openended_TO_Direction( ) {
        _myhandGAUCHE.DOACTIONWalkThumb(e_ButtonLocationType._0_Main);
    }


    //---------------------ATTACKING _ RIGHT HAND DROITE------------------------------------
    //---------------MAIN
    public void FIRE_TAP( ) {
        _myhandDROITE.DOACTION_FireTap(e_ButtonLocationType._0_Main );
    }
    public void FIRE_FAST_Direction( ) {
        _myhandDROITE.DOACTION_FastFire(e_ButtonLocationType._0_Main );
    }
    public void FIRE_Openended_AT_Direction( ) {
        _myhandDROITE.DOACTION_FireWait(e_ButtonLocationType._0_Main );
    }

    //---------------SEC
    public void SUPER_TAP( ) {
        _myhandDROITE.DOACTION_FireTap(e_ButtonLocationType._1_SuperFire );
    }
    public void SUPER_FAST_Direction( ) {
        _myhandDROITE.DOACTION_FastFire(e_ButtonLocationType._1_SuperFire );
    }
    public void SUPER_Openended_AT_Direction( ) {
        _myhandDROITE.DOACTION_FireWait(e_ButtonLocationType._1_SuperFire );
    }
    //---------------TER
    public void GADGET_TAP( ) {
        _myhandDROITE.DOACTION_FireTap(e_ButtonLocationType._2_GadgetFire );
    }


    public void Break_TO_CloseEnd( ) {
        _myhandGAUCHE.DOACTION_CLOSEWALK();
    }
    public void Break_AT_CloseEnd( ) {
        _myhandDROITE.DOACTION_CLOSEFirewait();
    }
    // CoroutineCoordinator_Lefthand
    //pass : ACTION HOME_Main  argNormalized
    public void QUICK_HOVER_D() {

        _myhandDROITE.QUICKHOVER();
    }
    public void QUICK_HOVER_G()
    {

        _myhandGAUCHE.QUICKHOVER();
    }





}


/* //---------------------WALKING _ LEFT HAND GAUCHE---------------------------------------
    //---------------MAIN
    public void WALK_Openended_TO_Direction(Vector3 arg_v3_Normed) {
        _myhandGAUCHE.DOACTIONWalkThumb(e_ButtonLocationType.Main, arg_v3_Normed);
    }


    //---------------------ATTACKING _ RIGHT HAND DROITE------------------------------------
    //---------------MAIN
    public void FIRE_TAP(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FireTap(e_ButtonLocationType.Main, argTarget);
    }
    public void FIRE_FAST_Direction(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FastFire(e_ButtonLocationType.Main, argTarget);
    }
    public void FIRE_Openended_AT_Direction(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FireWait(e_ButtonLocationType.Main, argTarget);
    }

    //---------------SEC
    public void SUPER_TAP(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FireTap(e_ButtonLocationType.SuperFire, argTarget);
    }
    public void SUPER_FAST_Direction(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FastFire(e_ButtonLocationType.SuperFire, argTarget);
    }
    public void SUPER_Openended_AT_Direction(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FireWait(e_ButtonLocationType.SuperFire, argTarget);
    }
    //---------------TER
    public void GADGET_TAP(Vector3 argTarget) {
        _myhandDROITE.DOACTION_FireTap(e_ButtonLocationType.GadgetFire, argTarget);
    }


    public void Break_TO_CloseEnd(Vector3 argTarget) {
        _myhandGAUCHE.DOACTION_CLOSEWALK(argTarget);
    }
    public void Break_AT_CloseEnd(Vector3 argTarget) {
        _myhandDROITE.DOACTION_CLOSEFirewait(argTarget);
    }*/
