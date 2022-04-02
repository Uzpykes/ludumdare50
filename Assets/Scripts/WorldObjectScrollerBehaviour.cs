using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves all objects in the list to specified direction
public class WorldObjectScrollerBehaviour : MonoBehaviour
{
    public Vector3 ScrollDirection;
    public List<Transform> Object;

    public void Update()
    {
        transform.position += ScrollDirection * Time.deltaTime;
    }
}
