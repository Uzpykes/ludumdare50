using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTrigger : MonoBehaviour
{
    private bool m_MinigameWasTriggered = false;
    [SerializeField] private bool m_RequireLanded;
    [SerializeField] private List<MinigameStage> m_Stages;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Trigger only once per trigger
        if (m_MinigameWasTriggered)
            return;

        TriggerMinigame(collision.gameObject.layer);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Trigger only once per trigger
        if (m_MinigameWasTriggered)
            return;

        TriggerMinigame(collision.gameObject.layer);
    }

    private void TriggerMinigame(int layer)
    {
        if (layer == LayerMask.NameToLayer("Player"))
        {
            if (m_RequireLanded && !PlayerStatsManager.Instance.IsLanded)
                return;
            Debug.Log("Minigame trigger enter!");
            MinigameManager.Instance.StartMinigame(m_Stages, this);
            m_MinigameWasTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If player somehow leaves the area then cancel the minigame
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Minigame trigger exit!");
            MinigameManager.Instance.EndMinigame(this);
        }
    }
}
