using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class JumpScript : MonoBehaviour
{
    // All Public Input same as Serialized Attributes

    public GameObject winLosePanel;
    public TextMeshProUGUI text;

    public ParticleSystem particle;
    public AudioClip clip;

    // All Private Input same as Non-Serialized Attributes

    private bool endTime = false;
    private bool endTime2 = false;
    private float timer = 0;

    // All Post Processing Attributes

    private PostProcessVolume volume;
    private Bloom bloom;
    private ChromaticAberration chromatic;
    private LensDistortion lens;


    private void Start()
    {
        // All setup and component values initialized

        volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out chromatic);
        volume.profile.TryGetSettings(out lens);
        bloom.intensity.value = 1;
        chromatic.intensity.value = 0;
        lens.intensity.value = 0;
    }

    // Called on collision enter

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Platform"))
        {
            FindObjectOfType<GameLogic>().JumpPressedToFalse();
        }
        else if (collision.collider.CompareTag("Goal"))
        {
            text.text = "YOU WIN";
            SetupEnd();
        }
        else if (collision.collider.CompareTag("Enemy"))
        {
            text.text = "YOU LOSE";
            SetupEnd();
        }
    }

    // Called at the end of every level

    void SetupEnd()
    {
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {
            text.text = "THANKS FOR PLAYING!";
        }

        winLosePanel.SetActive(true);
        endTime = true;
        particle.Play();
        FindObjectOfType<GameLogic>().enabled = false;
        GetComponent<AudioSource>().volume = 1;
        GetComponent<AudioSource>().PlayOneShot(clip);
    }

    // Reset the scene if player pressed R

    public void RestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (endTime)
        {
            timer += Time.unscaledDeltaTime * 2;
            timer = Mathf.Clamp01(timer);

            bloom.intensity.value = Mathf.Lerp(1, 10, timer);
            chromatic.intensity.value = Mathf.Lerp(0, 1, timer);
            lens.intensity.value = Mathf.Lerp(0, 50, timer);

            if (timer >= 1)
            {
                endTime = false;
                endTime2 = true;
            }
        }
        
        if (endTime2)
        {
            timer -= Time.unscaledDeltaTime * 2;
            timer = Mathf.Clamp01(timer);

            bloom.intensity.value = Mathf.Lerp(1, 10, timer);
            chromatic.intensity.value = Mathf.Lerp(0, 1, timer);
            lens.intensity.value = Mathf.Lerp(0, 50, timer);

            if (timer <= 0)
            {
                endTime2 = false;
                Invoke("LoadNextScene", 1f);
            }
        }
    }

    // Next scene loading basic logic 
    
    void LoadNextScene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 6)
        {

        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }      
    }
}
