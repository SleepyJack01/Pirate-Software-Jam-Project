using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static bool isDead = false;
    public Transform[] respawnPoints;
    [SerializeField] Animator animator;
    [SerializeField] private CheckPointManager checkPointManager;
    private CharacterController characterController;
    

    private void Start()
    {
        checkPointManager = FindObjectOfType<CheckPointManager>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision");

        if (other.gameObject.CompareTag("Enemies") && !isDead)
        {
            StartCoroutine(Respawn());
        }
        else if (other.gameObject.CompareTag("EndGame"))
        {
            StartCoroutine(EndGame());
        }
    }

    private IEnumerator Respawn()
    {
        isDead = true;
        Debug.Log("Respawning...");
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        characterController.enabled = false;
        if (checkPointManager == null)
        {
            Debug.Log("CheckPointManager not found");
            yield break;
        }
        else
        {
            int currentCheckPointIndex = checkPointManager.CurrentCheckPointIndex;
            if (currentCheckPointIndex >= respawnPoints.Length)
            {
                Debug.Log("Invalid CheckPointIndex");
                yield break;
            }
            transform.position = respawnPoints[currentCheckPointIndex].position;
            Debug.Log("Respawned at CheckPointIndex: " + currentCheckPointIndex);
            animator.SetTrigger("FadeIn");
            characterController.enabled = true;
            isDead = false;
        }
    }

    private IEnumerator EndGame()
    {
        Debug.Log("EndGame");
        animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
    }
}
