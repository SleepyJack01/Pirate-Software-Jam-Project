using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject firstSelectedButton;
    public static bool isPaused = false;

    public void OnControlsSchemeChanged()
    {
        if (playerInput.currentControlScheme == "Keyboard & Mouse")
        {
            eventSystem.SetSelectedGameObject(null);
        }
        else if (playerInput.currentControlScheme == "Gamepad")
        {
            eventSystem.SetSelectedGameObject(firstSelectedButton);
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        eventSystem.SetSelectedGameObject(null);
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        isPaused = false;
        Debug.Log("Quit to Main Menu");
        //SceneManager.LoadScene("MainMenu");
    }

    public void OnMenuButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
}
