using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCollide : MonoBehaviour
{
    // public GameObject playerController
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground") || collision.collider.CompareTag("Enemies") || collision.collider.CompareTag("Decor"))
        Destroy(this.gameObject);
    }
}
