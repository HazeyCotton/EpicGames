using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    private static string currScene = "Main";

    public static void OpenScene(string name) {
        SceneManager.LoadScene(name);
        currScene = name;
    }

    public static void ReloadScene() {
        SceneManager.LoadScene(currScene);
    }
}
