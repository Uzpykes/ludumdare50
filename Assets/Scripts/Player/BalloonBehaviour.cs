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



    [SerializeField] private float m_HeatDecayRate;
    [SerializeField] private float m_HeatFromFuelRate;

    private float m_VerticalSpeed;
    [SerializeField] private float m_VerticalSpeedUpperBound;
    [SerializeField] private float m_VerticalSpeedLowerBound;

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
        transform.position += new Vector3(0, m_VerticalSpeed * Time.deltaTime);
    }

    //Compares weight and heat and decides which way the balloon is moving and how fast
    private void UpdateVerticalSpeed()
    {
        var dif = (m_Heat - m_Weight);
        if (dif > 0f) // if heat is higher than weight, then loose speed faster
            dif *= 3f;
        else
            dif /= 2f;

        m_VerticalSpeed = dif * .1f; // To move up you need to have heat higher than your weight.

        if (m_VerticalSpeed < m_VerticalSpeedLowerBound)
            m_VerticalSpeed = m_VerticalSpeedLowerBound;
        else if (m_VerticalSpeed > m_VerticalSpeedUpperBound)
            m_VerticalSpeed = m_VerticalSpeedUpperBound;
    }

    private void HandleInput()
    {
        m_AddedHeatThisFrame = false;
        if (Input.GetKeyDown(KeyCode.Space))
            DropWeight();

        if (Input.GetKey(KeyCode.H))
            AddHeat();

        if (Input.GetKeyDown(KeyCode.F))
            ReleaseHeat();
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
        if (m_Heat > 100f) m_Heat = 100f;

        m_Fuel -= 1f * Time.deltaTime;
        if (m_Fuel < 0f) m_Fuel = 0f;
    }

    private void ReleaseHeat()
    {
        m_Heat -= 5f;
        if (m_Heat < 0f) m_Heat = 0f;
    }

}
