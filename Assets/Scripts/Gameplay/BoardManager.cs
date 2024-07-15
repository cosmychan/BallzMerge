using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;
    public Vector2Int boardSize;
    public Vector2 offset;
    //public GameObject placePrefab;
    public List<Vector3> placements;
    public List<GameObject> blocksOnBoard;
    public List<GameObject> blockPrefab;
    public GameObject placementHolder;

    private Vector3 _currentBlockPos;
    private Vector2Int _blockPlacement;
    private List<Vector3> _nextPlacement;
    private int _blockType = 0;

    public BlockNumber number;
    public BlockTypeSpawner blockType;

    public AudioSource source;
    public AudioClip blockDestroy;

    public GameObject gameOverScreen;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //Get info about which blocks and how many to spawn
        BlockSpawn();
        //initialize board for block spawn
        StartCoroutine(InitializeBoard());

        //Start move, allow player input
        //MoveScoreCounter.instance.StartMove();
        BallController.instance.startedMove = true;
    }

    public IEnumerator InitializeBoard()
    {
        // create board placement for block movement by size (height, width)
        //for (int k = 0; k < gameObject.transform.childCount; k++)
        //{
        //    //add placeholders to list
        //    placements.Add(transform.GetChild(k).gameObject.transform.position);
        //}



        int randomValue = Random.Range(0, boardSize.x - 1);
        for (int j = 0; j < 1; j++)
        {
            for (int i = 0; i < boardSize.x; i++)
            {
                //spawn placements for block movement
                //GameObject newPlacement = Instantiate(placePrefab, placementHolder);
                //newPlacement.transform.position = transform.position - new Vector3(i * offset.x, j * offset.y, 0);
                //add tranforms to list for better control at the end of each move
                //placements.Add(newPlacement.transform);
                //Debug.LogError("spawn blocks___");
                
                //Debug.LogError("random " + randomValue);

                if ((i == randomValue))
                {
                    //call info from ScriptableObject
                    //Debug.LogError("spawn blocks");
                    GameObject newBlock = Instantiate(blockPrefab[Random.Range(0, blockPrefab.Count-1)], transform);
                    newBlock.transform.position = transform.position - new Vector3(i * offset.x, j * offset.y, 0);
                    //add current blocks on board to list
                    //blocksOnBoard.Add(newBlock);
                }

                //for test
                //if ((i == 2) && (j == 5))
                //{
                //    //call info from ScriptableObject
                //    GameObject newBlock = Instantiate(blockPrefab, transform);
                //    newBlock.transform.position = transform.position - new Vector3(i * offset.x, j * offset.y, 0);
                //    //add current blocks on board to list
                //    //blocksOnBoard.Add(newBlock);
                //}
            }
        }


        yield return new WaitForSeconds(1f);
        //after all spawn done - call the StartMove event to start playing
        MoveScoreCounter.instance.StartMove();
    }

    public void BlockSpawn()
    {
        //Debug.LogError("move nr " + MoveScoreCounter.instance.moveNumber);
        //add to list the possible blocks for spawn, by type
        Type block = blockType.GetBlockTypeByMove(MoveScoreCounter.instance.moveNumber);
        if (block == null)
        {
            return;
        }
        else
        {
            if (blockPrefab.Count <= 0)
            {
                //Debug.LogError("entered");
                blockPrefab.Add(block.blockPrefab);
            }
            else
            {
                for (int i = 0; i < blockPrefab.Count; i++)
                {
                    //Debug.LogError("name " + block.blockPrefab);
                    if (blockPrefab[i].GetComponent<Placement>().blockTypeIndex != block.typeIndex)
                    {
                        blockPrefab.Add(block.blockPrefab);
                    }
                }
            }
        }


        //the probability of spawning a certain number of blocks
        Number blockNum = number.GetBlockNumberToSpawnByMove(MoveScoreCounter.instance.moveNumber);
        int randomValue = Random.Range(0, 100);
        if (blockNum == null)
        {
            blockNum = number.GetBlockNumberToSpawnByMove(0);
        }

        for (int i = 0; i < blockNum.chanceOfSpawn.Count; i++)
        {
            int percent = 0;
            percent += blockNum.chanceOfSpawn[i];
            if (randomValue <= percent)
            {
                _blockType = i + 1;
            }
        }
    }

    public void BoardControlAtMoveEnd()
    {
        //we resset all the blocks for next move
        FindBlockOnBoard();
        MoveBlockToOther();
        MoveAllBlocksRowDown();

        //start next move
        //Debug.LogError("next move");
        BlockSpawn();
        StartCoroutine(InitializeBoard());
    }

    public void FindBlockOnBoard()
    {
        //Debug.LogError("block on board");
        //empty list to avoid problems if there is info on the empty blocks
        blocksOnBoard.Clear();
        // re-add to the list all the active blocks on board
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            blocksOnBoard.Add(gameObject.transform.GetChild(i).gameObject);
        }     
    }

    public void MoveBlockToOther()
    {
        //Debug.LogError("moveblock");
        //Get all the block with same type/id if close to move towards eachother
        if (blocksOnBoard.Count > 1)
        {
            for (int i = 0; i < blocksOnBoard.Count; i++)
            {
                //Debug.LogError("entereeeee");
                blocksOnBoard[i].GetComponent<Placement>().MoveBlock(blocksOnBoard[i].GetComponent<Placement>().GetNearestBlock());
                source.PlayOneShot(blockDestroy);
            }
        }
        
    }

    public void MoveAllBlocksRowDown()
    {
        //Debug.LogError("row down");
        //move all block down by 1 row at the end
        for (int i = 0; i < blocksOnBoard.Count; i++)
        {
            //Debug.LogError("erooooowww");
            blocksOnBoard[i].GetComponent<Placement>().MoveBlockByRow(Vector3.down);
        }
    }

    #region UsedBeforeBug
    
    
    public void BoardControlInMove(Transform position, int idType)
    {
        for (int i = 0; i < placements.Count - 1; i++)
        {
            //Debug.LogError("entered " + position.position + " " + placements[i]);
            if (position.position == placements[i])
            {
                //Transform currentBlock = placements[i].transform;
                //ebug.LogError("entered___" + currentBlock.position);
                GetCurrentBlockPos(placements[i]);
            }
        }

        //CheckBoard();
        if (idType == _blockType)
        {
            Debug.LogError("blockType " + _blockType);
            Debug.LogError("block found_ " + _currentBlockPos);
        }
    }

    //public IEnumerator FindBlockPosOnBoard(Transform position)
    //{

    //}

    public void GetCurrentBlockPos(Vector3 position)
    {
        Debug.LogError("entered_1");
        for (int j = 0; j < boardSize.y; j++)
        {
            for (int i = 0; i < boardSize.x; i++)
            {
                Debug.LogError("currentBlockPos " + placements[j] + " " + position);
                Debug.LogError("currentBlockPosIII " + placements[i] + " " + position);
                if (placements[i] == position)
                {
                    _blockPlacement.x = i;

                    if (i > 2)
                    {
                        Debug.LogError("i " + i);
                        _nextPlacement.Add(placements[i - 1]);
                    }
                    else if (i < boardSize.x - 2)
                    {
                        Debug.LogError("i_ " + i);
                        _nextPlacement.Add(placements[i + 1]);
                    }
                }

                if (placements[j] == position)
                {
                    _blockPlacement.y = j;
                    if (j > 2)
                    {
                        Debug.LogError("j " + j);
                        _nextPlacement.Add(placements[j - 1]);
                        
                    } else if (j < boardSize.y - 2)
                    {
                        Debug.LogError("j_ " + j);
                        _nextPlacement.Add(placements[j + 1]);
                    }
                    
                    
                } 
                
            }
        }
        if (_nextPlacement != null)
        {
            CheckBoard();
        }
        
        
    }

    public void CheckBoard()
    {
        Debug.LogError("entered__");
        for (int i = 0; i < blocksOnBoard.Count - 1; i++)
        {
            for (int j = 0; j < _nextPlacement.Count - 1; j++)
            {
                
                if (blocksOnBoard[i].transform.position == _nextPlacement[j])
                {
                    Debug.LogError("block found " + blocksOnBoard[i].transform.position);
                    _blockType = blocksOnBoard[i].GetComponent<Placement>().blockTypeIndex;
                    _currentBlockPos = _nextPlacement[j];
                }
            }
        }
    }

    #endregion
}
