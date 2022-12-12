using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainStrategyMaker : MonoBehaviour
{


    MiniMapManager _minimap;


    public void Initme(MiniMapManager argMinimap, BrainPlayActionDecider argactiondecider) {
        _minimap = argMinimap;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
