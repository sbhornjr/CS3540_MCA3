using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallElevator : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    Animator elevatorAnim;

    public AudioClip buttonClicked;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        anim.SetTrigger("buttonClicked");
        AudioSource.PlayClipAtPoint(buttonClicked, Camera.main.transform.position);
        elevatorAnim.SetTrigger("elevatorCalled");
    }
}
