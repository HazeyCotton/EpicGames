using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainSceneUI : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Levels;

    public void Menu_Play()
    {
        Menu.SetActive(false);
        Levels.SetActive(false);
        Scenes.OpenScene("Level_1");
    }

    public void Tutorial()
    {
        Menu.SetActive(false);
        Levels.SetActive(false);
        Scenes.OpenScene("Level_0");
    }

    public void Level_Selector()
    {
        Menu.SetActive(false);
        Levels.SetActive(true);
    }

    public void Menu_Quit()
    {
        Application.Quit();
    }

    public void Levels_Lv0()
    {
        Scenes.OpenScene("Level_0");
    }

    public void Levels_Lv1()
    {
        Menu.SetActive(false);
        Levels.SetActive(false);
        Scenes.OpenScene("Level_1");
    }

    public void Levels_Lv2()
    {
        Scenes.OpenScene("Level_2");
    }

    public void Levels_Lv3()
    {
        Scenes.OpenScene("Level_3");
    }

    public void Levels_Lv4()
    {
        Scenes.OpenScene("Level_4");
    }

    public void Levels_Lv5()
    {
        Scenes.OpenScene("Level_5");
    }

    public void Levels_Back()
    {
        Menu.SetActive(true);
        Levels.SetActive(false);
    }

    public void goToLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }
}