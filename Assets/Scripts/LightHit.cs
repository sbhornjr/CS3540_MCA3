using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHit : MonoBehaviour
{
    public GameObject torchLight;
    public AudioClip lightHitSFX;

    // Start is called before the first frame update
    void Start()
    {
        torchLight = transform.GetChild(0).gameObject;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            AudioSource.PlayClipAtPoint(lightHitSFX, Camera.main.transform.position);

            torchLight.SetActive(false);

            Destroy(torchLight, 0.5f);
        }
    }
}
