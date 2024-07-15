using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockType", menuName = "Block/BlockType", order = 1)]
public class BlockTypeSpawner : ScriptableObject
{
    public List<Type> blocks;

    public Type GetBlockTypeByMove(int move)
    {
        foreach (var blockType in blocks)
        {
            if (blockType.move == move)
            {
                return blockType;
            }
        }

        return null;
    }
}

[System.Serializable]
public class Type
{
    public int typeIndex;
    public GameObject blockPrefab;
    public int move;
}
