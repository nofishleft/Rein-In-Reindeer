using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    //The radius that animals will be alerted if inside for each type of sound the player is making
    public float Movement;
    public float Shot;
    public float Reloading;
    public float Refueling;

    public void Update()
    {
        float Radius = Mathf.Max(Movement, Shot, Reloading, Refueling);

        if (Radius <= 0) return;

        foreach (Enemy enemy in Enemy.enemies)
        {
            float dist = (enemy.transform.position - transform.position).sqrMagnitude;
            if (dist < Radius)
            {
                enemy.SoundAlert(transform.position);
            }
        }
    }

    public float Max(params float[] list)
    {
        float R = 0;

        foreach (float r in list)
        {
            if (r > R)
            {
                R = r;
            }
        }

        return R;
    }
}