using StarterAssets;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System.Collections;
using Cinemachine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    [Header("Death Settings")]
    public GameObject deathScreenUI;

    [Header("Damage Effects")]
    public Volume damageVolume;     
    public float vignetteIntensity = 0.6f; 
    public float vignetteDuration = 0.5f; 
    public float shakeDuration = 0.2f; 
    public float shakeMagnitude = 0.3f; 

    [Header("Screen Shake")]
    public float shakeForce = 1f; // putere shake
    private CinemachineImpulseSource impulseSource;

    [Header("Damage Indicator")]
    public GameObject damageIndicatorPrefab;
    public Transform indicatorCanvasArea;  

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public StaminaBar staminaBar;
    public float drainRate = 5f; // drain rate 
    public float regenRate = 10f; // charge rate

    private StarterAssetsInputs _input;
    private Vignette _vignette; // Componenta interna de Vignette
    private Transform cameraTransform; // Referinta la camera pentru Shake
    private Vector3 originalCameraPos;
    private bool isDead = false;

    void Start()
    {
        //init health
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        //init stamina
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);

        _input = GetComponentInChildren<StarterAssetsInputs>();
        impulseSource = GetComponent<CinemachineImpulseSource>();

        // Initializam Vigneta din Volum
        if (damageVolume != null && damageVolume.profile.TryGet(out _vignette))
        {
            _vignette.intensity.value = 0f; // Asiguram ca e invizibila la start
        }

        if (deathScreenUI != null) deathScreenUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (_input != null) _input.cursorInputForLook = true;
    }

    void Update()
    {
        if (isDead)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return; 
        }
        HandleStamina();
    }

    void HandleStamina()
    {
        bool isSprinting = _input.sprint && _input.move != Vector2.zero;

        if (isSprinting && currentStamina > 0)
        {
            currentStamina -= drainRate * Time.deltaTime;
            if (currentStamina < 0)
                currentStamina = 0;
        }
        else
        {
            currentStamina += regenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaBar.SetStamina(currentStamina, maxStamina);

        if (currentStamina <= 0)
        {
            _input.sprint = false; // no sprinting when no stamina:)
        }
    }

    public bool HasStamina(float amount)
    {
        return currentStamina >= amount;
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina < 0) currentStamina = 0;

        if (staminaBar != null) staminaBar.SetStamina(currentStamina, maxStamina);
    }

    public void TakeDamage(int damage, Transform attacker = null)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (healthBar != null) healthBar.SetHealth(currentHealth);

        // Daca avem un atacator si avem indicatorul setat, il afisam
        if (attacker != null && damageIndicatorPrefab != null && indicatorCanvasArea != null)
        {
            CreateDamageIndicator(attacker);
        }
        //efecte damage
        StartCoroutine(TriggerDamageEffects());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (deathScreenUI != null) deathScreenUI.SetActive(true);
        if (_input != null)
        {
            _input.cursorInputForLook = false;
        }
        StartCoroutine(UnlockCursor());
        Time.timeScale = 0f;
    }

    IEnumerator UnlockCursor()
    {
        yield return new WaitForEndOfFrame();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator TriggerDamageEffects()
    {
        // A. Pornim Vigneta
        if (_vignette != null) _vignette.intensity.value = vignetteIntensity;

        // B. SCREEN SHAKE CU CINEMACHINE (Mult mai simplu!)
        if (impulseSource != null)
        {
            // Genereaza un impuls instantaneu
            impulseSource.GenerateImpulse(shakeForce);
        }

        // C. Fade Out Vigneta
        float fadeSpeed = 1f / vignetteDuration;
        while (_vignette != null && _vignette.intensity.value > 0)
        {
            _vignette.intensity.value -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    void CreateDamageIndicator(Transform attacker)
    {
        // 1. Cream obiectul
        GameObject newIndicator = Instantiate(damageIndicatorPrefab, indicatorCanvasArea);

        // 2. Luam scriptul
        DamageIndicator indicatorScript = newIndicator.GetComponent<DamageIndicator>();

        if (indicatorScript != null)
        {
            // 3. APELAM SETUP IMEDIAT
            // Asta va seta pozitia si va roti sageata inainte ca Unity sa deseneze primul cadru
            indicatorScript.Setup(attacker);
        }
    }
}
