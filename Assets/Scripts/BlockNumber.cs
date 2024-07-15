using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockNumber", menuName = "Block/BlockNumber", order = 1)]
public class BlockNumber : ScriptableObject
{
    public List<Number> blockNumberChance;

    public Number GetBlockNumberToSpawnByMove(int move)
    {
        foreach (var blockNum in blockNumberChance)
        {
            if (blockNum.move == move)
            {
                return blockNum;
            }
        }

        return null;
    }
}

[System.Serializable]
public class Number
{
    public int move;
    public List<int> chanceOfSpawn;
}
