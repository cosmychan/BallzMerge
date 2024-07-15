using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MoveScoreCounter : MonoBehaviour
{
    public static MoveScoreCounter instance;
    public bool isMove;
    public int moveNumber = 0;
    public TMP_Text moveText;
    public TMP_Text bestScore;

    private void Awake()
    { 
        instance = this;

        //load save for move number
        //if save exists, get the previous best score
        SaveManager.instance.LoadSave();
        bestScore.text = "Best Score: " + SaveManager.instance.score.ToString();

        //show current/initial move number at start of play
        moveText.text = "Move: " + moveNumber.ToString();
    }

    public void StartMove()
    {
        //Debug.LogError("startmove");
        //add move number by 1 at the start of each move
        moveNumber += 1;
        moveText.text = "Move: " + moveNumber.ToString();

        //change value of isMove to allow player input
        isMove = true;
    }

    public void EndMove()
    {
        //save current numver of moves
        SaveManager.instance.Save(moveNumber);

        //call BoardControlAtMoveEnd event to spawn new blocks, move (if possible) the ones with same number
        BoardManager.instance.BoardControlAtMoveEnd();

        //block player input
        isMove = false;
    }
}
