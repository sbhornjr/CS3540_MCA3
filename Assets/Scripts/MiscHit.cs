using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscHit : MonoBehaviour
{
    Rigidbody rb;
    public AudioClip miscHitSFX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            AudioSource.PlayClipAtPoint(miscHitSFX, Camera.main.transform.position);
            rb.AddForce(collision.transform.forward);
        }
    }
}
