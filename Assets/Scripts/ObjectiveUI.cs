using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    public TMP_Text text;
    public int total;

    // Start is called before the first frame update
    void Start()
    {
        total = Enemy.enemies.Count;
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            yield return null;

            int n = total - Enemy.enemies.Count;

            text.text = $"{n}/{total}";

            if (n >= total)
            {
                StartCoroutine(ObjectiveCompleted());
            }
        }
    }

    IEnumerator ObjectiveCompleted()
    {
        StopCoroutine("Loop");
        yield return null;
    }
}
