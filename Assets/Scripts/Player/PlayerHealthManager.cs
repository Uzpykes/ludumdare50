using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthManager : MonoBehaviour
{
    public static UnityEvent<float> OnAttacked;

    private void Awake()
    {
        OnAttacked ??= new UnityEvent<float>();

        OnAttacked.AddListener(HandleAttack);
    }

    private void HandleAttack(float damage)
    {
        PlayerStatsManager.Instance.Health -= damage;
    }

    private void OnDestroy()
    {
        OnAttacked.RemoveListener(HandleAttack);
    }
}
