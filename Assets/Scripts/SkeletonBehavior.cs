using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBehavior : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float minDistance = 0f;
    public int dmgAmount = 20;
    public AudioClip hitPlayerSFX;

    bool holdUp = false;
    float timeSinceHold;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!holdUp)
        {
            float step = speed * Time.deltaTime;

            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > minDistance)
            {
                transform.LookAt(player);
                transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            }
        }
        else
        {
            timeSinceHold += Time.deltaTime;
            if (timeSinceHold >= 1) holdUp = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(hitPlayerSFX, Camera.main.transform.position);
            other.GetComponent<PlayerHealth>().TakeDamage(dmgAmount);
            holdUp = true;
        }
    }
}
