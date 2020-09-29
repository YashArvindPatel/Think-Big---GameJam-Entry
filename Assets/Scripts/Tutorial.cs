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
    private GameLogic logic;

    private void Start()
    {
        logic = FindObjectOfType<GameLogic>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            //{
            //    text.enabled = false;
            //}

            if (logic.joystick.Horizontal < 0 || logic.joystick.Horizontal > 0)
            {
                text.enabled = false;
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            //if (Input.GetKeyDown(KeyCode.UpArrow))
            //{
            //    text.enabled = false;
            //}

            if (logic.joystick.Vertical > 0.33f)
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

            if (logic.trigger && !done)
            {
                done = true;
                CancelInvoke();
            }

            if (done)
            {
                //if (Input.GetKeyDown(KeyCode.Space))
                //{
                //    text.text = "Now try using moving joystick VERTICALLY to Shift Plane";
                //    spacePressed = true;
                //}

                if (logic.trigger)
                {
                    text.text = "Now try using moving joystick VERTICALLY to Shift Plane";
                    spacePressed = true;
                }

                //if (spacePressed)
                //{
                //    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                //    {
                //        text.enabled = false;
                //    }
                //}

                if (spacePressed)
                {
                    if (logic.joystick.Vertical > 0.33f || logic.joystick.Vertical < -0.33f)
                    {
                        text.enabled = false;
                    }
                }
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            //if (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J))
            //{
            //    text.enabled = false;
            //}

            if (logic.leftButtonPressed || logic.rightButtonPressed || logic.upButtonPressed || logic.downButtonPressed)
            {
                text.enabled = false;
            }
        }
    }

    void CallThis()
    {
        text.text = "A 3D Game Duh..... Try pressing Middle Part of your screen to change the View";
        done = true;
    }
}
