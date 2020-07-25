using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootProjectile : MonoBehaviour
{
    public float projectileSpeed = 100f;
    public GameObject projectilePrefab;
    public GameObject lightProjPrefab;
    public GameObject miscProjPrefab;
    public GameObject weapon;
    public AudioClip throwSFX;

    public Image crosshairs;
    public Color skeletonReticleColor;
    public Color lightReticleColor;
    public Color miscReticleColor;

    Color originalReticleColor;
    Color originalWeaponColor;
    GameObject currentPrefab;

    // Start is called before the first frame update
    void Start()
    {
        currentPrefab = projectilePrefab;
        originalReticleColor = crosshairs.color;
        originalWeaponColor = weapon.GetComponent<Renderer>().materials[1].color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject projectile = Instantiate(currentPrefab,
                transform.position + transform.forward, transform.rotation);

            projectile.transform.Rotate(new Vector3(0, -90, 0));

            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

            AudioSource.PlayClipAtPoint(throwSFX, transform.position);

            projectile.transform.SetParent(
                GameObject.FindGameObjectWithTag("ProjectileParent").transform);
        }
    }

    private void FixedUpdate()
    {
        ReticleEffect();
    }

    void ReticleEffect()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Skeleton"))
            {
                currentPrefab = projectilePrefab;

                crosshairs.color = Color.Lerp(
                    crosshairs.color, skeletonReticleColor, Time.deltaTime * 2);

                weapon.GetComponent<Renderer>().materials[1].color = Color.Lerp(
                    weapon.GetComponent<Renderer>().materials[1].color, skeletonReticleColor, Time.deltaTime * 2);

                crosshairs.transform.localScale = Vector3.Lerp(
                    crosshairs.transform.localScale, new Vector3(0.7f, 0.7f, 1), Time.deltaTime * 2);
            }
            else if (hit.collider.CompareTag("Light"))
            {
                currentPrefab = lightProjPrefab;

                crosshairs.color = Color.Lerp(
                    crosshairs.color, lightReticleColor, Time.deltaTime * 2);

                weapon.GetComponent<Renderer>().materials[1].color = Color.Lerp(
                    weapon.GetComponent<Renderer>().materials[1].color, lightReticleColor, Time.deltaTime * 2);

                crosshairs.transform.localScale = Vector3.Lerp(
                    crosshairs.transform.localScale, new Vector3(0.7f, 0.7f, 1), Time.deltaTime * 2);
            }
            else if (hit.collider.CompareTag("Misc"))
            {
                currentPrefab = miscProjPrefab;

                crosshairs.color = Color.Lerp(
                    crosshairs.color, miscReticleColor, Time.deltaTime * 2);

                weapon.GetComponent<Renderer>().materials[1].color = Color.Lerp(
                    weapon.GetComponent<Renderer>().materials[1].color, miscReticleColor, Time.deltaTime * 2);

                crosshairs.transform.localScale = Vector3.Lerp(
                    crosshairs.transform.localScale, new Vector3(0.7f, 0.7f, 1), Time.deltaTime * 2);
            }
            else
            {
                currentPrefab = projectilePrefab;

                crosshairs.color = Color.Lerp(
                       crosshairs.color, originalReticleColor, Time.deltaTime * 2);

                weapon.GetComponent<Renderer>().materials[1].color = Color.Lerp(
                    weapon.GetComponent<Renderer>().materials[1].color, originalWeaponColor, Time.deltaTime * 2);

                crosshairs.transform.localScale = Vector3.Lerp(
                    crosshairs.transform.localScale, Vector3.one, Time.deltaTime * 2);
            }
        }
        else
        {
            crosshairs.color = Color.Lerp(
                   crosshairs.color, originalReticleColor, Time.deltaTime * 2);

            weapon.GetComponent<Renderer>().materials[1].color = Color.Lerp(
                    weapon.GetComponent<Renderer>().materials[1].color, originalWeaponColor, Time.deltaTime * 2);

            crosshairs.transform.localScale = Vector3.Lerp(
                crosshairs.transform.localScale, Vector3.one, Time.deltaTime * 2);
        }
    }
}
