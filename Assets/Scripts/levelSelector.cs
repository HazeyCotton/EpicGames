using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class levelSelector : MonoBehaviour
{

    public int levelNum;
    public Text levelText;

    // Start is called before the first frame update
    void Start()
    {
        if (levelText != null)
        {
            levelText.text = levelNum.ToString();
        }
    }

    // Update is called once per frame
    public void OpenScene()
    {
        try 
        {
            SceneManager.LoadScene("Level_" + levelNum.ToString());
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    public void backToHome()
    {
        try 
        {
            SceneManager.LoadScene("StartScreenB");
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }
}
