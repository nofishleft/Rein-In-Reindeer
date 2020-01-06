using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> enemies = new List<Enemy>();

    public Animator animator;
    public CharacterController controller;

    public Transform toRotate;

    public float MoveSpeed;

    public float RotateSpeed;

    float yaw;

    [NonSerialized]
    public bool alive;

    [NonSerialized]
    public float fleeTimer;

    // Start is called before the first frame update
    void Awake()
    {
        enemies.Add(this);
    }

    IEnumerator Alive()
    {
        while (alive)
        {
            Vector3 forward = Quaternion.Euler(0, yaw, 0) * Vector3.forward;

            //Get Floor
            RaycastHit hit;

            if (Physics.Raycast(toRotate.position, Vector3.down, out hit))
            {
                Quaternion tilt = Quaternion.FromToRotation(Vector3.up, hit.normal);

                toRotate.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(forward, hit.normal), hit.normal);
            }

            yield return null;

            controller.SimpleMove(Vector3.zero);
        }
    }

    void Start()
    {
        alive = true;

        yaw = toRotate.rotation.eulerAngles.y;

        StartCoroutine(Alive());
    }

    public void Hit()
    {
        Debug.Log("Hit");
        Vector3 v = transform.position - PlayerController.player.transform.position;
        v.y = 0;
        StartCoroutine(Flee(v.normalized));

        if (alive)
        {
            StartCoroutine("Dead");
        }
    }

    void OnDestroy()
    {
        enemies.Remove(this);
    }

    public void SoundAlert(Vector3 position)
    {
        StopCoroutine("Flee");

        Vector3 dir = transform.position - position;
        dir.y = 0;

        dir = dir.normalized;

        StartCoroutine(Flee(dir));
    }

    IEnumerator Dead()
    {
        alive = false;

        StopCoroutine("Alive");

        float MaxSpeed = MoveSpeed;
        float deathTimer = 2;

        yield return null;

        while (deathTimer > 0)
        {
            deathTimer = Mathf.Max(deathTimer - Time.deltaTime, 0);

            //Slope from MaxSpeed to 0.5*MaxSpeed
            MoveSpeed = MaxSpeed * (0.375f * deathTimer + 0.25f);

            yield return null;
        }

        animator.SetTrigger("Death");

        StopCoroutine("Flee");

        Destroy(this);
    }

    IEnumerator Flee(Vector3 dir)
    {
        StopCoroutine("Flee");
        StopCoroutine("Alive");

        fleeTimer = 5f;

        animator.SetTrigger("Flee");

        float destAng = Random.Range(5f, 15f);
        if (Random.value > 0.5f) destAng *= -1;

        destAng += Mathf.Atan2(dir.x, dir.z);

        //dir = Rotate(dir, destAng);

        Vector3 facing = toRotate.rotation * Vector3.forward;

        float ang;

        while (fleeTimer > 0)
        {
            if (fleeTimer < 4)
            {
                ang = destAng;
            }
            else
            {
                ang = Mathf.Lerp(yaw, destAng, 1 - (fleeTimer-4));
            }
            

            Vector3 forward = Quaternion.Euler(0, ang, 0) * Vector3.forward;

            //Get Floor
            RaycastHit hit;

            if (Physics.Raycast(toRotate.position, Vector3.down, out hit))
            {
                Quaternion tilt = Quaternion.FromToRotation(Vector3.up, hit.normal);

                toRotate.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(forward, hit.normal), hit.normal);
            }

            controller.SimpleMove(forward * MoveSpeed);

            fleeTimer = Mathf.Max(fleeTimer - Time.deltaTime, 0);

            yield return null;
        }

        animator.SetTrigger("Idle");
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
}
