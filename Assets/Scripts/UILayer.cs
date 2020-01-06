using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
//using Color = UnityEngine.UI.Graphic.Color;

public class UILayer : MonoBehaviour
{
    static UILayer layer;

    public CanvasGroup group;

    //Torch
    public Image torchImage;
    public Sprite torchOn;
    public Sprite torchOff;

    //Speed
    public GameObject speedVisibility;
    public Transform top;
    public Transform current;
    public Transform bot;

    [NonSerialized]
    public float speedTick = 3f;
    [NonSerialized]
    public float fadeTick = 1f;

    //Ammo
    public Image bulletImage;
    public Image fuelImage;

    // Start is called before the first frame update
    void Start()
    {
        layer = this;
    }

    public static void UpdateSpeed(float current, float max, float min)
    {
        Vector3 dir = layer.top.position - layer.bot.position;

        float percent = (current - min) / (max - min);

        layer.current.position = layer.bot.position + (dir * percent);

        
        layer.speedVisibility.SetActive(true);
        layer.speedTick = 3f;
        layer.fadeTick = 1f;
    }

    public static void NoBullet()
    {
        layer.bulletImage.color = new Color(0.5f, 0f, 0f);
        layer.speedTick = 3f;
        layer.fadeTick = 1f;
        layer.speedVisibility.SetActive(true);
    }

    public static void NoFuel()
    {
        layer.fuelImage.color = new Color(0.5f, 0f, 0f);
        layer.speedTick = 3f;
        layer.fadeTick = 1f;
        layer.speedVisibility.SetActive(true);
    }

    public static void Reloading()
    {
        layer.bulletImage.color = new Color(1f, 0.67f, 0f);
        layer.speedTick = 3f;
        layer.fadeTick = 1f;
        layer.speedVisibility.SetActive(true);
    }

    public static void Refueling()
    {
        layer.fuelImage.color = new Color(1f, 0.67f, 0f);
        layer.speedTick = 3f;
        layer.fadeTick = 1f;
        layer.speedVisibility.SetActive(true);
    }

    public static void HasBullet()
    {
        layer.bulletImage.color = new Color(0f, 0.5f, 0f);
        layer.speedTick = 3f;
        layer.fadeTick = 1f;
        layer.speedVisibility.SetActive(true);
    }

    public static void HasFuel()
    {
        layer.fuelImage.color = new Color(0f, 0.5f, 0f);
        layer.speedTick = 3f;
        layer.fadeTick = 1f;
        layer.speedVisibility.SetActive(true);
    }

    public static void TorchOn()
    {
        layer.torchImage.sprite = layer.torchOn;
        layer.speedTick = 3f;
        layer.fadeTick = 1f;
        layer.speedVisibility.SetActive(true);
    }

    public static void TorchOff()
    {
        layer.torchImage.sprite = layer.torchOff;
        layer.speedTick = 3f;
        layer.fadeTick = 1f;
        layer.speedVisibility.SetActive(true);
    }

    void Update()
    {
        if (speedTick > 0)
        {
            speedTick = Mathf.Max(speedTick - Time.deltaTime, 0);
            speedVisibility.SetActive(true);
            group.alpha = 1f;
        }
        else if (speedTick <= 0 && fadeTick > 0)
        {
            group.alpha = Mathf.Clamp(fadeTick, 0, 1);
            fadeTick = Mathf.Max(fadeTick - Time.deltaTime, 0);
        }
        else
        {
            group.alpha = 0f;
            speedTick = 0f;
            fadeTick = 0f;
            speedVisibility.SetActive(false);
        }
    }
}
