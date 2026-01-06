using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRestarter : MonoBehaviour
{
    public void RestartGame()
    {
        // Deblocam timpul in caz ca e oprit
        Time.timeScale = 1f;
        // Reincarcam scena curenta
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}