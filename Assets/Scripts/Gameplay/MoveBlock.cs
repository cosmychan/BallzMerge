using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    public GameObject parentObj;
    public int blockType;
    public int otherBlockType;
    //public List<Vector3> pos;
    public Vector3 pos;
    public Vector3 direction;

    private void Awake()
    {
        blockType = gameObject.GetComponentInParent<Placement>().blockTypeIndex;

        switch (gameObject.tag)
        {
            case "ColUp":
                direction = Vector3.up;
                break;
            case "ColDown":
                direction = Vector3.down;
                break;
            case "ColRight":
                direction = Vector3.right;
                break;
            case "ColLeft":
                direction = Vector3.left;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        switch (other.tag)
        {
            case "Block":
                int otherBlockId = other.gameObject.GetComponent<Placement>().blockTypeIndex;
                if (blockType == otherBlockId)
                {
                    other.gameObject.SetActive(false);
                    parentObj.SetActive(false);
                    BoardManager.instance.source.PlayOneShot(BoardManager.instance.blockDestroy);
                }
                else
                {
                    direction = Vector3.zero;
                }
                
                break;
            case "Board":
                //if (pos != null)
                //{
                //    for (int i = 0; i < pos.Count - 1; i++)
                //    {
                //        if (pos[i] != other.transform.position)
                //        {
                //            pos.Add(other.transform.position);
                //        }
                //    }
                //}
                //else
                //{
                //    pos.Add(other.transform.position);
                //}

                pos = other.transform.position;

                break;

            case "Wall":
                direction = Vector3.zero;
                break;

            case "ColUp":
                otherBlockType = other.gameObject.GetComponent<MoveBlock>().blockType;
                break;

            case "ColDown":
                otherBlockType = other.gameObject.GetComponent<MoveBlock>().blockType;
                break;

            case "ColRight":
                otherBlockType = other.gameObject.GetComponent<MoveBlock>().blockType;
                break;

            case "ColLeft":
                otherBlockType = other.gameObject.GetComponent<MoveBlock>().blockType;
                break;

            //case "EndZone":
            //    direction = Vector3.down;
            //    break;
        }
    }
}
