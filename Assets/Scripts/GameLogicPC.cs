using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class GameLogicPC : MonoBehaviour
{
    // All Public Inputs (Same as Serialized Attributes)

    public Transform t1, t2;
    public Rigidbody player;
    public Camera cam;
    public Image image;
    public RectTransform canvas;
    public Transform level;
    public Image border;

    public float playerSpeed;
    public float canvasSpeed;
    public float upForce = 100f;

    public bool moveUnlocked, jumpUnlocked, shiftUnlocked, rotateUnlocked;

    public AudioSource audioSource;
    public AudioClip clip;

    public TrailRenderer trail;

    public List<MeshRenderer> meshRenderers;

    // All Private Inputs (Same as Non-Serialized Attributes)

    private bool origin = true;
    private bool trigger = false;
    private bool jumpPressed = false;

    private float timer = 0;
    private float jumpTimer = 1f;

    private AudioSource camAudio;

    // All Post Processing Attributes

    private PostProcessVolume volume;
    private Bloom bloom;
    private ChromaticAberration chromatic;

    // Joystick Controls

    public FloatingJoystick joystick;

    private void Awake()
    {
        // Scene Logic setup

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            moveUnlocked = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            moveUnlocked = true;
            jumpUnlocked = true;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            moveUnlocked = true;
            jumpUnlocked = true;
            shiftUnlocked = true;
        }
        else
        {
            moveUnlocked = true;
            jumpUnlocked = true;
            shiftUnlocked = true;
            rotateUnlocked = true;
        }
    }

    private void Start()
    {
        // All setup and component values initialized

        camAudio = GetComponent<AudioSource>();
        volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out chromatic);
        bloom.intensity.value = 1;
        chromatic.intensity.value = 0;

        transform.position = t1.position;
        transform.rotation = t1.rotation;
        cam.fieldOfView = 45;
        cam.nearClipPlane = 4.9f;
        cam.farClipPlane = 10f;
    }

    private void FixedUpdate()
    {
        // Trigger logic for gravity and movement (All physics handled here)

        if (origin && !trigger)
        {
            player.useGravity = true;
            player.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;

            if (Input.GetKey(KeyCode.UpArrow) && !jumpPressed && jumpUnlocked)
            {
                audioSource.PlayOneShot(clip);
                player.AddForce(Vector3.up * upForce);
                jumpPressed = true;
            }

            if (Input.GetKey(KeyCode.RightArrow) && moveUnlocked)
            {
                player.MovePosition(player.position + new Vector3(playerSpeed * Time.fixedDeltaTime, 0));
            }

            if (Input.GetKey(KeyCode.LeftArrow) && moveUnlocked)
            {
                player.MovePosition(player.position + new Vector3(-playerSpeed * Time.fixedDeltaTime, 0));
            }
        }
        else
        {
            player.useGravity = false;
            player.constraints = RigidbodyConstraints.FreezeAll;
        }
    }

    public void JumpPressedToFalse()
    {
        jumpPressed = false;
        jumpTimer = 1f;
    }

    void Update()
    {
        // Non physics triggers handled here

        if (jumpPressed)
        {
            jumpTimer -= Time.deltaTime;

            if (jumpTimer < 0)
            {
                JumpPressedToFalse();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && shiftUnlocked)
        {
            trigger = true;
        }

        // Scene Transition triggers and Linear Interpolation handled here

        if (trigger)
        {
            timer += Time.deltaTime;
            timer = Mathf.Clamp01(timer);
            chromatic.intensity.value = Mathf.Lerp(1, 0, timer);

            if (origin)
            {
                if (!border.enabled)
                    border.enabled = true;

                if (trail.enabled)
                    trail.enabled = false;

                if (player.GetComponent<BoxCollider>().enabled)
                    player.GetComponent<BoxCollider>().enabled = false;

                if (!meshRenderers[meshRenderers.Count - 1].enabled)
                {
                    foreach (var item in meshRenderers)
                    {
                        item.enabled = true;
                    }
                }

                camAudio.volume = Mathf.Lerp(1f, 0.25f, timer);
                camAudio.pitch = Mathf.Lerp(1f, 0.5f, timer);
                bloom.intensity.value = Mathf.Lerp(1, 0.5f, timer);
                transform.position = Vector3.Lerp(t1.position, t2.position, timer);
                transform.rotation = Quaternion.Lerp(t1.rotation, t2.rotation, timer);
                cam.nearClipPlane = Mathf.Lerp(4.9f, 0.01f, timer);
                cam.farClipPlane = Mathf.Lerp(10f, 100f, timer);
                cam.fieldOfView = Mathf.Lerp(45, 60, timer);
                float a = Mathf.Lerp(255, 200, timer);
                image.color = new Color32(0, 0, 0, (byte)a);

                if (timer == 1)
                {
                    timer = 0;
                    origin = false;
                    trigger = false;
                }
            }
            else
            {
                camAudio.volume = Mathf.Lerp(0.25f, 1f, timer);
                camAudio.pitch = Mathf.Lerp(0.5f, 1f, timer);
                bloom.intensity.value = Mathf.Lerp(0.5f, 1, timer);
                transform.position = Vector3.Lerp(t2.position, t1.position, timer);
                transform.rotation = Quaternion.Lerp(t2.rotation, t1.rotation, timer);
                cam.nearClipPlane = Mathf.Lerp(0.01f, 4.9f, timer);
                cam.farClipPlane = Mathf.Lerp(100f, 10f, timer);
                cam.fieldOfView = Mathf.Lerp(60, 45, timer);
                float a = Mathf.Lerp(200, 255, timer);
                image.color = new Color32(0, 0, 0, (byte)a);

                if (timer >= 0.75f && border.enabled)
                {
                    border.enabled = false;
                }

                if (timer == 1)
                {
                    timer = 0;
                    origin = true;
                    trigger = false;
                    trail.enabled = true;
                    player.GetComponent<BoxCollider>().enabled = true;

                    foreach (var item in meshRenderers)
                    {
                        item.enabled = false;
                    }
                }
            }
        }

        if (!origin)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                canvas.position = new Vector3(canvas.position.x, canvas.position.y, canvas.position.z + canvasSpeed * Time.deltaTime);
                t1.position = new Vector3(t1.position.x, t1.position.y, t1.position.z + canvasSpeed * Time.deltaTime);
                t2.position = new Vector3(t2.position.x, t2.position.y, t2.position.z + canvasSpeed * Time.deltaTime);
                cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z + canvasSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                canvas.position = new Vector3(canvas.position.x, canvas.position.y, canvas.position.z - canvasSpeed * Time.deltaTime);
                t1.position = new Vector3(t1.position.x, t1.position.y, t1.position.z - canvasSpeed * Time.deltaTime);
                t2.position = new Vector3(t2.position.x, t2.position.y, t2.position.z - canvasSpeed * Time.deltaTime);
                cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z - canvasSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.G) && rotateUnlocked)
            {
                level.Rotate(1, 0, 0);
            }
            else if (Input.GetKey(KeyCode.H) && rotateUnlocked)
            {
                level.Rotate(0, 1, 0);
            }
            else if (Input.GetKey(KeyCode.J) && rotateUnlocked)
            {
                level.Rotate(0, 0, 1);
            }
        }
    }
}
