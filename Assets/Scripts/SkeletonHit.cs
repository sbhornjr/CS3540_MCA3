using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHit : MonoBehaviour
{
    public GameObject smokeEffect;
    public AudioClip skeletonHitSFX;
    public LevelManager lm;

    private void Start()
    {
        if (lm == null) lm = GameObject.FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            DestroySkeleton();
        }
    }

    void DestroySkeleton()
    {
        Instantiate(smokeEffect, transform.position, transform.rotation);

        AudioSource.PlayClipAtPoint(skeletonHitSFX, Camera.main.transform.position);

        LevelManager.numSkeletonsRemaining -= 1;
        lm.SetScoreText();

        gameObject.SetActive(false);

        Destroy(gameObject, 0.5f);
    }
}
