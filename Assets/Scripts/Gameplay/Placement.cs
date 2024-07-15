using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    public int blockTypeIndex;
    public bool justSpawned;
    public Vector3 blockPosition;
    public List<MoveBlock> moveBlocks;

    private void Awake()
    {
        blockPosition = gameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Board")
        {
            //needed to avoid change pos of block on start if detect any trigger
            if (justSpawned)
            {
                //DoTween maybe?
                //to correct postion because accesses the collider position, not transform
                gameObject.transform.position = other.gameObject.transform.position;
                //blockPosition = other.gameObject.transform.position;
                //moved = true;

                //call board control event
                //BoardManager.instance.BoardControlInMove(gameObject.transform, blockTypeIndex);
                //justSpawned = false;
            }

            if (!justSpawned)
            {
                justSpawned = true;
                //gameObject.transform.position = other.gameObject.transform.position;
                Debug.LogError("Just Spawned!");
            }

        } else if (other.tag == "EndZone")
        {
            //game over screen, block input
            BoardManager.instance.gameOverScreen.SetActive(true);
            BallController.instance.startedMove = true;
            MoveScoreCounter.instance.isMove = false;

            // save progress
            SaveManager.instance.Save(MoveScoreCounter.instance.moveNumber);

        }
    }

    public void MoveBlock(Vector3 direction)
    {
        //move the block at the respective direction
        for (int i = 0; i < moveBlocks.Count; i++)
        {
            if (direction == moveBlocks[i].direction)
            {
                gameObject.transform.position += direction;
                justSpawned = true;
                return;
            }
        }
        
    }

    public void MoveBlockByRow(Vector3 direction)
    {
        //move block down
        //Debug.LogError("enterd");
        gameObject.transform.position += (direction + new Vector3( BoardManager.instance.offset.x, BoardManager.instance.offset.y, 0));
        justSpawned = true;
    }

    public Vector3 GetNearestBlock()
    {
        //check the first element to have found an intersection with the other block with same id/type
        for (int i = 0; i < moveBlocks.Count; i++)
        {
            if (blockTypeIndex == moveBlocks[i].otherBlockType)
            {
                //Debug.LogError("near");
                Vector3 dirToBlock = moveBlocks[i].direction;
                return dirToBlock;
            }
        }
        //if nothing found block remain in place
        return Vector3.zero;
    }
    
}
