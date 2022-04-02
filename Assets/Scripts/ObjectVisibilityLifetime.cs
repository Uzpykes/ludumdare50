using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVisibilityLifetime : MonoBehaviour
{
    private bool m_ShouldDestroy;
    [SerializeField] private Collider2D m_Collider;

    private void Awake()
    {
    }

    private void Update()
    {
        CheckIfShouldDestroy();

        if (m_ShouldDestroy)
            Destroy(this.gameObject);
    }

    private void CheckIfShouldDestroy()
    {
        if (m_ShouldDestroy)
            return; //return if already decided that this should be destroyed

        if (m_Collider.bounds.max.x < VisibilityBounds.Bounds.min.x) //if objects highest bound is outside left side of camera then destroy
        {
            m_ShouldDestroy = true;
        }
    }


}
