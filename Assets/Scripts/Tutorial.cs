using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject[] textboxes;
    private int textboxIndex;
    public GameObject timerText;
    public GameObject livesText;
    public GameObject score;

    private void Start() 
    {
        timerText.SetActive(false);
        livesText.SetActive(false);
        score.SetActive(false);

        for (int i = 0; i < textboxes.Length; i++) {
            textboxes[i].SetActive(false);
        }

        
        textboxIndex = 0;
        textboxes[textboxIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("textbox index: " + textboxIndex);

        //loop through all textboxes and set which ones are active
        //for (int i = 0; i < textboxes.Length; i++)
        //{
        //    if (i == textboxIndex) {
        //        Debug.Log("set active: " + i);
        //        textboxes[textboxIndex].SetActive(true);
        //    } else {
        //        textboxes[textboxIndex].SetActive(false);
        //    }
        //}

        if (textboxIndex == 0) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                textboxes[textboxIndex].SetActive(false);

                textboxIndex++;
                textboxes[textboxIndex].SetActive(true);
            }
        } else if (textboxIndex == 1) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                textboxes[textboxIndex].SetActive(false);

                textboxIndex++;
                textboxes[textboxIndex].SetActive(true);
            }
        } else if (textboxIndex == 2) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                textboxes[textboxIndex].SetActive(false);

                textboxIndex++;
                textboxes[textboxIndex].SetActive(true);
            }
        } else if (textboxIndex == 3) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                textboxes[textboxIndex].SetActive(false);

                textboxIndex++;
                textboxes[textboxIndex].SetActive(true);
            }
        } else if (textboxIndex == 4) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                textboxes[textboxIndex].SetActive(false);

                textboxIndex++;
                textboxes[textboxIndex].SetActive(true);
            }
        } else if (textboxIndex == 5) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                textboxes[textboxIndex].SetActive(false);

                textboxIndex++;
                textboxes[textboxIndex].SetActive(true);
                timerText.SetActive(true);
                livesText.SetActive(true);
                score.SetActive(true);

            }
        } else if (textboxIndex == 6) {
            

            if (Input.GetKeyDown(KeyCode.Space)) {
                textboxes[textboxIndex].SetActive(false);

                textboxIndex++;
                textboxes[textboxIndex].SetActive(true);
            }
        } else if (textboxIndex == 7) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                textboxes[textboxIndex].SetActive(false);
            }
        }
    }
}
