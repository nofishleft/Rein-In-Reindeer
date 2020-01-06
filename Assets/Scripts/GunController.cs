using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public float MaxFuel;

    public float Fuel;

    public float MaxSpeed;

    public bool Chambered;

    public bool ChamberClosed;
    public bool TankHolstered;

    public bool Refueling;

    public float RefuelRate;

    private Animator animator;

    public Transform DartSpawnPoint;
    public GameObject DartPrefab;

    public LayerMask crossHairLayerMask;
    public Transform crossHairPos;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    //Actions
    public void _Fire()
    {
        GameObject spawned = Instantiate(DartPrefab, DartSpawnPoint.position, transform.rotation);
        Rigidbody rb = spawned.GetComponent<Rigidbody>();
        rb.velocity = transform.rotation * Vector3.forward * (Fuel / MaxFuel) * MaxSpeed;

        foreach (Collider col in spawned.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(col, PlayerController.player.controller);
        }

        Chambered = false;
        Fuel = 0;
    }

    public void _Refuel()
    {
        animator.SetTrigger("Start Fueling");
        TankHolstered = false;
    }

    public void _Reload()
    {
        animator.SetTrigger("Open Chamber");
        Chambered = false;
        ChamberClosed = false;
    }

    public void _StopRefuel()
    {
        animator.SetTrigger("Stop Fueling");
    }

    //Checks
    public bool CanFire()
    {
        return Chambered && ChamberClosed && TankHolstered && Fuel > 0;
    }

    public bool CanRefuel()
    {
        return ChamberClosed && TankHolstered && Fuel < MaxFuel;
    }

    public bool CanReload()
    {
        return !Chambered && ChamberClosed && TankHolstered;
    }

    //Events
    public void ChamberIsOpen()
    {
        //Put in bullet
        animator.SetTrigger("Load Chamber");
    }

    public void BulletLoaded()
    {
        //Close chamber
        Chambered = true;
        animator.SetTrigger("Close Chamber");
    }

    public void ChamberIsClosed()
    {
        ChamberClosed = true;
    }

    public void StartRefueling()
    {
        Refueling = true;
        StartCoroutine("RefuelCoroutine");
    }

    public void FinishedRefueling()
    {
        Refueling = false;
        animator.ResetTrigger("Stop Fueling");
        /* Resetting this trigger, in the extremely unlikely scenario
         * where the player stops refueling just before reaching full fuel setting the trigger,
         * and the coroutine ticks and sets the trigger before the animation calls the vent and sets refueling to false
         */
    }

    public void CannisterHolstered()
    {
        TankHolstered = true;
        animator.ResetTrigger("Stop Fueling");
    }

    //Coroutines
    IEnumerator RefuelCoroutine()
    {
        while (Refueling)
        {
            yield return null;
            Fuel += Time.deltaTime * RefuelRate;

            if (Fuel >= MaxFuel) {
                Fuel = MaxFuel;
                animator.SetTrigger("Stop Fueling");
                break;
            }
        }
    }

    void Update()
    {
        if (ChamberClosed && TankHolstered)
        {
            crossHairPos.gameObject.SetActive(true);

            if (Physics.Raycast(DartSpawnPoint.position, transform.rotation * Vector3.forward, out RaycastHit hit, Mathf.Infinity, crossHairLayerMask))
            {
                //Hit
                Vector3 pos = cam.WorldToScreenPoint(hit.point);
                pos.z = crossHairPos.position.z;
                crossHairPos.position = pos;
            }
            else
            {
                int x = cam.pixelWidth;
                int y = cam.pixelHeight;
                //No hit
                //Center on screen
                Vector3 pos = new Vector3(x/2f, y/2f, crossHairPos.position.z);
                crossHairPos.position = pos;
            }
        }
        else
        {
            //Hide crosshair
            crossHairPos.gameObject.SetActive(false);
        }
    }
}