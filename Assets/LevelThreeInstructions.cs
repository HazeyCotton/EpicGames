using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelThreeInstructions : MonoBehaviour
{
    public GameObject[] textboxes;
    private int textboxIndex;

    private void Start()
    {

        for (int i = 0; i < textboxes.Length; i++) {
            textboxes[i].SetActive(false);
        }


        textboxIndex = 0;
        textboxes[textboxIndex].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
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
                //textboxes[textboxIndex].SetActive(true);
            }

        }
    }
}
