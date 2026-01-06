using UnityEngine;
using UnityEngine.SceneManagement; // Necesar daca vrei buton de Quit to Main Menu
#if UNITY_EDITOR
using UnityEditor; // Asta ne lasa sa controlam Editorul
#endif

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI; 

    void Update()
    {
        if (Time.timeScale == 0f && !GameIsPaused) return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); 
        Time.timeScale = 1f; 
        GameIsPaused = false;

        // Blocam cursorul inapoi pentru joc
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true); // Arata meniul
        Time.timeScale = 0f; // Ingheata timpul (inamicii nu te mai ataca)
        GameIsPaused = true;

        // Deblocam cursorul ca sa poti apasa butoane
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
        #endif
    }
}