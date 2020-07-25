using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierLightHit : MonoBehaviour
{
    public AudioClip lightHitSFX;

    private void OnCollisionEnter(Collision collision)
    {
        AudioSource.PlayClipAtPoint(lightHitSFX, Camera.main.transform.position);

        gameObject.SetActive(false);

        Destroy(gameObject, 0.5f);
    }
}
