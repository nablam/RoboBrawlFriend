using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hand 
{

    bool _myThumbIsDown = false; //not touching


    public abstract void CursorToSelected_POI();
    protected abstract void _ToSelected_POI();
}
