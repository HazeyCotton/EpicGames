using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneUI : MonoBehaviour
{
    public GameObject Menu;
    public GameObject Levels;

    public void Menu_Play() {
        Menu.SetActive(false);
        Levels.SetActive(true);
    }

    public void Menu_Quit() {
        Application.Quit();
    }

    public void Levels_Lv1() {
        Menu.SetActive(false);
        Levels.SetActive(false);
        Scenes.OpenScene("Sample Level");
    }

    public void Levels_Lv2() {
        Menu.SetActive(false);
        Levels.SetActive(false);
        Scenes.OpenScene("Sample Level");
    }

    public void Levels_Lv3() {
        Menu.SetActive(false);
        Levels.SetActive(false);
        Scenes.OpenScene("Sample Level");
    }

    public void Levels_Back() {
        Menu.SetActive(true);
        Levels.SetActive(false);
    }
}
