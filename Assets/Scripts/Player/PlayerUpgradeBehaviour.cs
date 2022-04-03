using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Listens to callbacks from minigame and other stuff and upgrades player stats
public class PlayerUpgradeBehaviour : MonoBehaviour
{
    void Start()
    {
        MinigameManager.Instance.OnPrizeSuccess.AddListener(HandleSuccessfulUpgrade);
    }
    
    private void HandleSuccessfulUpgrade(MinigamePrize prizeEntry)
    {
        var stats = PlayerStatsManager.Instance;
        switch (prizeEntry.PrizeType)
        {
            case PrizeType.Fuel:
               stats.Fuel = Mathf.Min(stats.MaxFuel, prizeEntry.ValueChange + stats.Fuel);
                break;
            case PrizeType.FuelEfficency:
                stats.HeatEfficency += prizeEntry.ValueChange * 0.1f;
                break;
            case PrizeType.Health:
                stats.Health = Mathf.Min(stats.MaxHealth, prizeEntry.ValueChange + stats.Health);
                break;
            case PrizeType.MaxFuel:
                stats.MaxFuel += prizeEntry.ValueChange;
                break;
            case PrizeType.SandBag:
                //TODO add
                break;
            case PrizeType.Survivor:
                //TODO add
                break;
        }


    }

}
