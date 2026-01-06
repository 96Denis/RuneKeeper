using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Setari Atac")]
    public float attackDamage = 20f;
    public float attackRange = 3f;
    public float attackRate = 1.5f;
    private float nextAttackTime = 0f;
    public float comboResetTime = 2f;
    private int currentAttackIndex = 0; 
    private float lastAttackTime = 0f;

    [Header("Referinte")]
    public Camera cam;
    public LayerMask enemyLayers;
    public Animator weaponAnimator;
    public CharacterController playerController;
    public PlayerStats playerStats;
    public float attackStaminaCost = 10f;

    [Header("Efecte Vizuale si Audio")]
    public GameObject hitVFX;
    public GameObject hitDecal;

    public AudioSource sourceDeSabie; 
    public AudioClip sunetSwing;      
    public AudioClip sunetHit;

    public GameObject damagePopupPrefab;

    void Start()
    {
        if(playerController == null)
        {
            playerController = GetComponentInChildren<CharacterController>();
        }
        playerStats = GetComponentInParent<PlayerStats>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        // 1. Animatia de mers
        HandleWalkingAnimation();

        // 2. Resetare Combo (daca a trecut prea mult timp de la ultimul atac)
        if (Time.time - lastAttackTime > comboResetTime)
        {
            currentAttackIndex = 0; // Revenire la primul atac
        }

        // 3. ATACUL
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void HandleWalkingAnimation()
    {
        if (weaponAnimator != null && playerController != null)
        {
            // Calculam viteza doar pe orizontala (X si Z), ignoram saritura (Y)
            Vector3 horizontalVelocity = new Vector3(playerController.velocity.x, 0, playerController.velocity.z);
            float speed = horizontalVelocity.magnitude;

            // Daca viteza e mai mare de 0.1, inseamna ca mergem
            bool isMoving = speed > 0.1f;
            weaponAnimator.SetBool("IsWalking", isMoving);
        }
    }

    void Attack()
    {
        if (playerStats != null)
        {
            if (!playerStats.HasStamina(attackStaminaCost)) return; 

            playerStats.UseStamina(attackStaminaCost); 
        }

        if (weaponAnimator != null)
        {
            // Setam indexul curent (0 pentru Atac 1, 1 pentru Atac 2)
            weaponAnimator.SetInteger("AttackIndex", currentAttackIndex);

            // Declansam animatia
            weaponAnimator.SetTrigger("Attack");

            // Schimbam indexul pentru data viitoare:
            // Daca e 0 devine 1. Daca e 1 devine 0.
            if (currentAttackIndex == 0) currentAttackIndex = 1;
            else currentAttackIndex = 0;

            lastAttackTime = Time.time; // Tinem minte cand am atacat ultima oara
        }

        if (sourceDeSabie != null && sunetSwing != null)
        {
            sourceDeSabie.pitch = Random.Range(0.9f, 1.1f);
            sourceDeSabie.PlayOneShot(sunetSwing);
        }

        // 3. Raycast
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, attackRange, enemyLayers))
        {
            // Efecte vizuale la impact
            if (hitVFX != null)
            {
                GameObject vfx = Instantiate(hitVFX, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(vfx, 2f);
            }

            if (hitDecal != null)
            {
                GameObject decal = Instantiate(hitDecal, hit.point, Quaternion.LookRotation(hit.normal));
                decal.transform.SetParent(hit.transform);
                Destroy(decal, 5f);
            }

            if (sunetHit != null) AudioSource.PlayClipAtPoint(sunetHit, hit.point);

            // --- DAMAGE ---
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy == null) enemy = hit.transform.GetComponentInParent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                if (damagePopupPrefab != null)
                {
                    Vector3 popupPos = hit.point + (cam.transform.position - hit.point).normalized * 0.2f;
                    GameObject popup = Instantiate(damagePopupPrefab, popupPos, Quaternion.identity);
                    popup.GetComponent<DamagePopup>().Setup((int)attackDamage);
                }
            }
        }
    }
}