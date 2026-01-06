using UnityEngine;
using UnityEngine.UI;

public class RuneStone : MonoBehaviour
{
    [Header("Settings")]
    public GameObject mobToSpawn; 
    public float activationTime = 2.0f;

    [Header("Materiale (Vizual)")] 
    public Material materialAprins;

    [Header("UI References")]
    public GameObject interactionText;
    public Slider progressSlider;

    private bool playerInZone = false;
    private float currentTimer = 0;
    private bool mobSpawned = false;
    private bool isActivated = false;
    private GameObject spawnedInstance;
    public Renderer meshRenderer;

    void Start()
    {
        if (interactionText != null) interactionText.SetActive(false);
        if (progressSlider != null) progressSlider.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isActivated) return;

        // Pasul 1: Activarea pietrei
        if (playerInZone && !mobSpawned)
        {
            if (Input.GetKey(KeyCode.E))
            {
                currentTimer += Time.deltaTime;
                if (progressSlider != null)
                {
                    progressSlider.gameObject.SetActive(true);
                    progressSlider.value = currentTimer / activationTime;
                }

                if (currentTimer >= activationTime)
                {
                    SpawnGuardian();
                }
            }
            else
            {
                currentTimer = 0;
                if (progressSlider != null) progressSlider.gameObject.SetActive(false);
            }
        }

        if (mobSpawned && !isActivated)
        {
            // Scriptul GolemAI il distruge cu Destroy() dupa 5 secunde
            if (spawnedInstance == null)
            {
                CompleteActivation();
            }
        }
    }

    void SpawnGuardian()
    {
        mobSpawned = true;
        currentTimer = 0;

        Vector3 spawnPos = transform.position + (transform.forward * 3); // Apare la 3 metri in fata pietrei
        spawnedInstance = Instantiate(mobToSpawn, spawnPos, Quaternion.identity);
        spawnedInstance.SetActive(true);

        if (progressSlider != null) progressSlider.gameObject.SetActive(false);
        if (interactionText != null) interactionText.SetActive(false);
    }

    void CompleteActivation()
    {
        isActivated = true;

        if (materialAprins != null && meshRenderer != null)
        {
            meshRenderer.material = materialAprins;
        }
        if (GameManager.instance != null)
        {
            GameManager.instance.AddActivatedStone();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            if (!mobSpawned && interactionText != null) interactionText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            currentTimer = 0;
            if (interactionText != null) interactionText.SetActive(false);
            if (progressSlider != null) progressSlider.gameObject.SetActive(false);
        }
    }
}