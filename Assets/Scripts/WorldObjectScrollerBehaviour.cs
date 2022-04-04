using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves all objects in the list to specified direction
public class WorldObjectScrollerBehaviour : MonoBehaviour
{
    //This is a global variable from perspective of player
    public static Vector3 ScrollDirection;

    public float ScrollDirectionMultiplier = 1f; //can be used to lower the rate of scroll

    public void Update()
    {
        transform.position += ScrollDirection * ScrollDirectionMultiplier * Time.deltaTime;
        PlayerStatsManager.Instance.DistanceTraveled += (ScrollDirection.x * -1 ) * Time.deltaTime;
    }
}
