using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBehaviour : MonoBehaviour
{
    private float m_Heat
    {
        get => PlayerStatsManager.Instance.Heat;
        set => PlayerStatsManager.Instance.Heat = value;
    }

    private float m_Weight
    {
        get => PlayerStatsManager.Instance.Weight;
        set => PlayerStatsManager.Instance.Weight = value;
    }

    private float m_Fuel
    {
        get => PlayerStatsManager.Instance.Fuel;
        set => PlayerStatsManager.Instance.Fuel = value;
    }

    private float m_VerticalSpeed
    {
        get => PlayerStatsManager.Instance.VerticalSpeed;
        set => PlayerStatsManager.Instance.VerticalSpeed = value;
    }

    private float m_HeatEfficency
    {
        get => PlayerStatsManager.Instance.HeatEfficency;
    }

    private float m_FuelUseRate
    {
        get => PlayerStatsManager.Instance.FuelUseRate;
    }


    [SerializeField] private float m_HeatDecayRate;
    [SerializeField] private float m_HeatFromFuelRate;

    [SerializeField] private float m_VerticalSpeedUpperBound;
    [SerializeField] private float m_VerticalSpeedLowerBound;

    private bool m_IsGrounded
    {
        get => PlayerStatsManager.Instance.IsLanded;
        set => PlayerStatsManager.Instance.IsLanded = value;
    }

    private bool m_AddedHeatThisFrame = false;

    private void OnGUI()
    {
        GUILayout.Label($"Heat: {m_Heat}");
        GUILayout.Label($"Weight: {m_Weight}");
        GUILayout.Label($"Vertical Speed: {m_VerticalSpeed}");
    }

    private void Start()
    {
        WorldObjectScrollerBehaviour.ScrollDirection = new Vector3(-1, 0, 0);
    }

    private void Update()
    {
        HandleInput();

        if (!m_AddedHeatThisFrame)
            UpdateHeat();

        UpdateVerticalSpeed();

        MoveVertically();
    }


    //Loose heat gradually
    private void UpdateHeat()
    {
        m_Heat -= m_HeatDecayRate * Time.deltaTime;
        if (m_Heat < 0f) m_Heat = 0f;
    }

    private void MoveVertically()
    {
        var vSpeed = new Vector3(0, m_VerticalSpeed * Time.deltaTime);
        if (m_IsGrounded && vSpeed.y < 0f)
            vSpeed.y = 0f;
        transform.position += vSpeed;
    }

    //Compares weight and heat and decides which way the balloon is moving and how fast
    private void UpdateVerticalSpeed()
    {
        var dif = ((m_Heat * m_HeatEfficency) - m_Weight) / 2f;

        m_VerticalSpeed = dif * .1f; // To move up you need to have heat higher than your weight.

        if (m_VerticalSpeed < m_VerticalSpeedLowerBound)
            m_VerticalSpeed = m_VerticalSpeedLowerBound;
        else if (m_VerticalSpeed > m_VerticalSpeedUpperBound)
            m_VerticalSpeed = m_VerticalSpeedUpperBound;
    }

    private void HandleInput()
    {
        m_AddedHeatThisFrame = false;
        if (Input.GetKeyDown(KeyCode.D))
            DropWeight();

        if (Input.GetKey(KeyCode.H))
            AddHeat();

        if (Input.GetKeyDown(KeyCode.F))
            ReleaseHeat();

        if (Input.GetKeyDown(KeyCode.T))
            PlayerStatsManager.Instance.MaxFuel += 10;
    }

    // Just remove some weight
    private void DropWeight()
    {
        m_Weight -= 5f;
        if (m_Weight < 10) m_Weight = 10f;
    }

    //Add heat and remove fuel
    private void AddHeat()
    {
        if (m_Fuel <= 0f)
            return;

        m_AddedHeatThisFrame = true;

        m_Heat += m_HeatFromFuelRate * Time.deltaTime;
        if (m_Heat > PlayerStatsManager.Instance.MaxHeat) m_Heat = 100f;

        m_Fuel -= m_FuelUseRate * Time.deltaTime;
        if (m_Fuel < 0f) m_Fuel = 0f;
    }

    private void ReleaseHeat()
    {
        m_Heat -= 5f;
        if (m_Heat < 0f) m_Heat = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            m_IsGrounded = true;
            WorldObjectScrollerBehaviour.ScrollDirection = Vector3.zero;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            m_IsGrounded = false;
            WorldObjectScrollerBehaviour.ScrollDirection = new Vector3(-1, 0, 0);
        }
    }

}
