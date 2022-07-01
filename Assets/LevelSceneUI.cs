using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSceneUI : MonoBehaviour
{
    public GameObject Pause;

    void OnCancel() {
        Time.timeScale = 0f;
        Pause.SetActive(true);
    }

    public void Pause_Resume() {
        Time.timeScale = 1f;
        Pause.SetActive(false);
    }

    public void Pause_Restart() {
        Time.timeScale = 1f;
        Pause.SetActive(false);
        Scenes.ReloadScene();
    }

    public void Pause_Leave() {
        Time.timeScale = 1f;
        Pause.SetActive(false);
        Scenes.OpenScene("StartScreen");
    }
}
