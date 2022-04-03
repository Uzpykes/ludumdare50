using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinigamePrize")]
public class MinigamePrize : ScriptableObject
{
    public Sprite PrizeSprite;
    public PrizeType PrizeType;
    public float BaseWidth; //Base width on how hard it is to get this prize
    public int ValueChange;
}

public enum PrizeType
{
    SandBag,
    Survivor,
    MaxFuel,
    Fuel,
    Health,
    FuelEfficency
}

[System.Serializable]
public struct MinigamePrizeEntry
{
    // from 0 to 1
    public float Location;
    public MinigamePrize Prize;
}