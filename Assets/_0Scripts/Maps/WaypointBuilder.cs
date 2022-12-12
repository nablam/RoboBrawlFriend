using OpenCVForUnity.CoreModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBuilder : MonoBehaviour
{

    List<Point> Waypoints;
    public List<Point> GeneratePointsForMap(e_BrawlMapName argMapname) {
        Waypoints = new List<Point>();

        Waypoints.Add(new Point(155, 70));
        Waypoints.Add(new Point(155, 100));
        Waypoints.Add(new Point(155, 120));
        Waypoints.Add(new Point(140, 120));
        Waypoints.Add(new Point(180, 120));
        return Waypoints;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
