using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator doorAnimator;
    private BoxCollider boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = gameObject.GetComponent<Animator>();
        if (doorAnimator == null)
        {
            Debug.Log("doorAnimator가 유효하지 않습니다");
        }

        boxCollider = gameObject.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.Log("boxCollider가 유효하지 않습니다");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Triggered!");
            doorAnimator.SetBool("open", true);
        }
    }
}