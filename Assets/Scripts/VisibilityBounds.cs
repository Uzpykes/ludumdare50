using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityBounds : MonoBehaviour
{
    private Camera m_MainCamera;

    public static Bounds Bounds { get; private set; }

    private void Awake()
    {
        m_MainCamera = Camera.main;
        Bounds = new Bounds();
    }

    private void Update()
    {
        SetCameraBounds();
    }

    private void SetCameraBounds()
    {
        Bounds = new Bounds(m_MainCamera.transform.position, new Vector3(m_MainCamera.orthographicSize * m_MainCamera.aspect * 2f, m_MainCamera.orthographicSize * 2f));
        Debug.DrawLine(Bounds.min, Bounds.max);
    }
}
