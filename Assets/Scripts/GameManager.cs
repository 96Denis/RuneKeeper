using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Quest UI")]
    public TextMeshProUGUI questText;

    [Header("Scor")]
    public int stonesActivated = 0;
    public int totalStones = 4; 

    [Header("UI")]
    public GameObject winScreenUI; 

    void Awake()
    {
        Application.targetFrameRate = 60;
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
     if (winScreenUI != null) winScreenUI.SetActive(false);
        UpdateQuestText();
    }

    public void AddActivatedStone()
    {
        stonesActivated++;
        UpdateQuestText();

        if (stonesActivated >= totalStones)
        {
            WinGame();
        }
    }

    void UpdateQuestText()
    {
        if (questText != null)
        {
            questText.text = "Runes Sealed: " + stonesActivated + " / " + totalStones;
        }
    }

    void WinGame()
    {
        // 1. Activare ecran de final
        if (winScreenUI != null) winScreenUI.SetActive(true);

        // 2. Deblocare Mouse-ul (ca la Death Screen)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 3. Oprire timpul
        Time.timeScale = 0f;
    }
}