using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class FinalScore : MonoBehaviour
{
    public TextMeshProUGUI pointsLabel;
    public TextMeshProUGUI timeLabel;
    
    void Start() {
        int points = PlayerController.points;
        pointsLabel.text = "Score: " + points.ToString();

        

        float time = PlayerController.time;

        var minutes = time / 60;
        var seconds = time % 60;
        var fraction = (time * 100) % 100;

        timeLabel.text = "Time: " + string.Format("{0:00} : {1:00} : {2:00}", minutes, seconds, fraction);
    }
}