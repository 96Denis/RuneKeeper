using StarterAssets;
using UnityEngine;

public class DemoCheats : MonoBehaviour
{
    [Header("Referinte")]
    public PlayerCombat combatScript;
    public PlayerStats statsScript;
    public FirstPersonController movementScript;

    [Header("Setari Cheats")]
    public float superSpeed = 12f;
    public float superSprint = 20f;
    public float superDamage = 1000f;

    private float defaultSpeed;
    private float defaultSprint;
    private float defaultDamage;

    void Start()
    {
        // 1. Gasim scripturile de lupta automat
        combatScript = GetComponent<PlayerCombat>();
        statsScript = GetComponent<PlayerStats>();

        // 2. PROTECTIE: Cautam scriptul de miscare DOAR daca nu l-ai pus deja manual
        if (movementScript == null)
        {
            movementScript = GetComponentInChildren<FirstPersonController>();
        }

        // Verificam daca l-am gasit
        if (movementScript == null)
        {
            Debug.LogError("DEMO CHEATS: Tot nu gasesc 'FirstPersonController'! Asigura-te ca e tras in Inspector.");
        }
        else
        {
            // Salvam valorile originale
            defaultSpeed = movementScript.MoveSpeed;
            defaultSprint = movementScript.SprintSpeed;
        }

        if (combatScript != null)
        {
            defaultDamage = combatScript.attackDamage;
        }
    }

    void Update()
    {
        // --- CHEAT 1: VITEZA (Numpad 5 SAU tasta 5 de sus) ---
        // Acum merge si daca apesi 5 deasupra tastelor "R" si "T"
        if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (movementScript != null)
            {
                movementScript.MoveSpeed = superSpeed;
                movementScript.SprintSpeed = superSprint;
                //Debug.Log("CHEAT ACTIVAT: Flash Mode! (Speed)");
            }
        }

        // --- CHEAT 2: PUTERE (Numpad 6 SAU tasta 6 de sus) ---
        if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (combatScript != null) combatScript.attackDamage = superDamage;

            if (statsScript != null)
            {
                statsScript.maxHealth = 9999;
                statsScript.currentHealth = 9999;
                statsScript.healthBar.SetMaxHealth(9999);
                statsScript.healthBar.SetHealth(9999);
            }
            //Debug.Log("CHEAT ACTIVAT: One Punch Man! (Damage + HP)");
        }

        // --- RESET (Numpad 0 SAU tasta 0 de sus) ---
        if (Input.GetKeyDown(KeyCode.Keypad0) || Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (movementScript != null)
            {
                movementScript.MoveSpeed = defaultSpeed;
                movementScript.SprintSpeed = defaultSprint;
            }
            if (combatScript != null)
            {
                combatScript.attackDamage = defaultDamage;
            }
            //Debug.Log("CHEATS DEZACTIVATE: Back to normal.");
        }
    }
}