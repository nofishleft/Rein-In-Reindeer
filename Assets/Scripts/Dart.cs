using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    public Rigidbody body;
    public Animator animator;
    public BoxCollider toEnable;
    public BoxCollider toDisable;

    bool drained = false;

    void OnCollisionEnter(Collision collision)
    {
        if (drained) return;

        drained = true;

        Transform newParent;

        newParent = collision.transform.Find("Base");

        if (newParent == null)
            newParent = collision.transform;
                
        Enemy e = newParent.GetComponent<Enemy>();

        if (e != null)
        {
            e.Hit();
        }

        Debug.Log(newParent.gameObject.name);

        transform.parent = newParent;

        body.isKinematic = true;
        body.detectCollisions = false;
        body.velocity = Vector3.zero;

        transform.position = collision.GetContact(0).point - transform.rotation * Vector3.forward * 0.1f;

        toEnable.enabled = true;
        toDisable.enabled = false;

        animator.SetTrigger("Drain");
    }

    public void FinishedDrain()
    {
        body.isKinematic = false;
        body.detectCollisions = true;
        body.velocity = Vector3.zero;
        body.useGravity = true;

        transform.parent = null;

        StartCoroutine("Despawn");
    }

    IEnumerator Despawn()
    {
        yield return null;

        float t = 5f;
        while (t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }

        //Destroy(this.gameObject);
    }
}
