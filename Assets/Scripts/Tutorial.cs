using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    // Tutorial Script. Basic text shown to player depending on level

    public TextMeshProUGUI text;
    private bool done = false;
    private bool invokeOnce = true;
    private bool spacePressed = false;

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                text.enabled = false;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                text.enabled = false;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            if (invokeOnce)
            {
                Invoke("CallThis", 5f);
                invokeOnce = false;
            }

            if (done)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    text.text = "Now try pressing [Up/Down Arrow] to Shift Plane";
                    spacePressed = true;
                }

                if (spacePressed)
                {
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        text.enabled = false;
                    }
                }
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            if (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J))
            {
                text.enabled = false;
            }
        }
    }

    void CallThis()
    {
        text.text = "A 3D Game Duh..... Try pressing [Space] to change the View";
        done = true;
    }
}
