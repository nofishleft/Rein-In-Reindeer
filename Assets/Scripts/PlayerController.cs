using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController player;

    //Flashlight
    public Light light;
    public bool initialLightState;

    //Animation variables
    public GunController gun;

    private Animator animator;

    private bool _reloading;
    private bool _refueling;

    private bool _ads;

    //Movement variables
    public CharacterController controller;
    public CharacterMotor motor;
    public NoiseMaker noise;

    private float yaw;
    public float pitch = 0;

    public float yawSpeed = 10f;
    public float pitchSpeed = 10f;

    private float moveY = 0;

    private int speed = 0;
    public int divisions = 10;
    public float minSpeed = 0.5f;
    public float maxSpeed = 5f;

    //Pause menu
    public bool paused = true;
    public GameObject pauseMenuObj;
    public PopUpMenu pauseMenu;

    void Awake()
    {
        player = this;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        light.enabled = initialLightState;
    }

    void Update()
    {
        if (PauseKey())
        {
            Move();
            Aim();
            Gun();
            OtherKeys();
        }
    }

    private bool PauseKey()
    {
        if (!paused && Input.GetKeyDown(KeyCode.Escape))
        {
            paused = true;
            //Open pause menu
            pauseMenuObj.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if (paused && Input.GetKeyDown(KeyCode.Escape))
        {
            //Forward event to pause menu class
            //Returns true if pause menu is being closed
            if (pauseMenu.EscapeClicked())
            {
                paused = false;
                pauseMenuObj.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                //Return false just for this frame, then continue to return true for next frames
                return false;
            }
        }

        return !paused;
    }

    private void Move()
    {
        int x = 0, z = 0;

        if (Input.GetKey(KeyCode.W))
            z += 1;
        if (Input.GetKey(KeyCode.S))
            z -= 1;
        if (Input.GetKey(KeyCode.A))
            x -= 1;
        if (Input.GetKey(KeyCode.D))
            x += 1;

        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.Space))
                motor.inputJump = true;
            else
                motor.inputJump = false;
        }
        else
        {
            motor.inputJump = false;
        }

        //moveY -= gravity * Time.deltaTime;

        Vector3 v = Rotate(new Vector3(x, 0, z), -yaw).normalized;

        motor.inputMoveDirection = v;

        //controller.Move(v * Time.deltaTime);
    }

    private static Vector3 Rotate(Vector3 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float tz = v.z;
        v.x = (cos * tx) - (sin * tz);
        v.z = (sin * tx) + (cos * tz);
        return v;
    }

    private void Aim()
    {
        yaw += Input.GetAxis("Mouse X") * yawSpeed * Time.deltaTime;

        yaw = yaw % 360; //Prevent overflow

        pitch -= Input.GetAxis("Mouse Y") * pitchSpeed * Time.deltaTime;

        pitch = Mathf.Clamp(pitch, -80, 80); // Prevent overflow

        transform.rotation = Quaternion.Euler(0,yaw,0) * Quaternion.Euler(pitch, 0, 0);
    }

    private void Gun()
    {
        if (Input.GetMouseButtonDown(0) && !_reloading && !_refueling && gun.CanFire())
        {
            animator.ResetTrigger("ADS");
            animator.ResetTrigger("Hip");

            Fire();
        }
        else if (Input.GetKeyDown(KeyCode.R) && !_reloading && !_refueling && gun.CanReload())
        {
            Reload();

            animator.ResetTrigger("ADS");
            animator.ResetTrigger("Hip");
            
            _ads = false;
        }
        else if (Input.GetKeyDown(KeyCode.F) && !_reloading && !_refueling && gun.CanRefuel())
        {
            Refuel();

            animator.ResetTrigger("ADS");
            animator.ResetTrigger("Hip");

            _ads = false;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (CanADS())
            {
                _ads = true;
                ADS();
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if (CanADS())
            {
                _ads = false;
                Hip();
            }
        }
    }

    private void OtherKeys()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            light.enabled = !light.enabled;

            if (light.enabled) UILayer.TorchOn();
            else UILayer.TorchOff();
        }

        bool spUpdate = false;

        //Mouse scroll
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            speed = Math.Min(Math.Max(speed + 1, 0), divisions);

            spUpdate = true;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            speed = Math.Min(Math.Max(speed - 1, 0), divisions);

            spUpdate = true;
        }


        float mvspd = (speed / (float)divisions) * (maxSpeed - minSpeed) + minSpeed;

        motor.movement.maxForwardSpeed =
        motor.movement.maxSidewaysSpeed =
        motor.movement.maxBackwardsSpeed = mvspd;

        if (spUpdate) UILayer.UpdateSpeed(mvspd, maxSpeed, minSpeed);
}

    //Actions
    private void ADS()
    {
        animator.ResetTrigger("Hip");
        animator.SetTrigger("ADS");
    }

    private void Hip()
    {
        animator.ResetTrigger("ADS");
        animator.SetTrigger("Hip");
    }

    private void Reload()
    {
        if (gun.CanReload())
        {
            UILayer.Reloading();
            _reloading = true;
            animator.SetTrigger("ReloadPosition");
        }
    }

    private void Refuel()
    {
        if (gun.CanRefuel())
        {
            UILayer.Refueling();
            _refueling = true;
            animator.SetTrigger("RefuelPosition");
        }
    }

    private void Fire()
    {
        if (gun.CanFire())
        {
            UILayer.NoBullet();
            UILayer.NoFuel();
            gun._Fire();
        }
    }

    //Checks
    public bool CanADS()
    {
        return !_refueling && !_reloading;
    }

    //Events
    public void StartReloadAnim()
    {
        gun._Reload();
        StartCoroutine("WaitForChamberClosed");
    }

    public void StartRefuelAnim()
    {
        gun._Refuel();
        StartCoroutine("WaitForTankHolstered");
    }

    //Enumerator
    IEnumerator WaitForChamberClosed()
    {
        while (!gun.ChamberClosed)
        {
            yield return null;
        }

        _reloading = false;
        animator.SetTrigger("Hip");

        UILayer.HasBullet();
    }

    //Enumerator
    IEnumerator WaitForTankHolstered()
    {
        while (!gun.TankHolstered)
        {
            yield return null;
        }

        _refueling = false;
        animator.SetTrigger("Hip");

        UILayer.HasFuel();
    }
}
