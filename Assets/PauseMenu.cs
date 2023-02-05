using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused = false;
    public GameObject canvas;


    // Start is called before the first frame update
    void Start()
    {
        // Hide the pause menu at the start of the game
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        // Toggle the pause state
        isPaused = !isPaused;

        // Enable or disable the pause menu
        canvas.SetActive(isPaused);

        // Pause or resume the game
        Time.timeScale = isPaused ? 0 : 1;
    }
}
