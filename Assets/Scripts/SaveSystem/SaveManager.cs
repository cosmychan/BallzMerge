using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public int score = 0;

    private void Awake()
    {
        instance = this;
    }

    public void LoadSave()
    {
        if (PlayerPrefs.HasKey("Score")){
            score = PlayerPrefs.GetInt("Score");
        } else
        {
            Save(score);
        }
    }

    public void Save(int scoreTosave)
    {
        PlayerPrefs.SetInt("Score", scoreTosave);
        PlayerPrefs.Save();
    }
}
