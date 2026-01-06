using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{
    [Header("Setari AI")]
    public float detectionRange = 10f; // Distanta de la care vede
    public float attackRange = 2.5f;   // Distanta de la care musca
    public float damageDealt = 10f;
    public float timeBetweenAttacks = 1.5f;

    [Header("Referinte")]
    public Transform player;

    private NavMeshAgent agent;
    private Animator animator;
    private EnemyHealth healthScript;

    private float attackTimer = 0f;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthScript = GetComponent<EnemyHealth>();

        // Gasire jucator automata
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 3.5f;
    }

    void Update()
    {
        if (isDead) return;
        if (healthScript != null && healthScript.health <= 0)
        {
            Die();
            return;
        }

        // distanta pana la jucator
        float distance = Vector3.Distance(transform.position, player.position);

        // --- LOGICA DE ZONA ---

        // CAZ A: Jucatorul e prea departe
        if (distance > detectionRange)
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
        }
        // CAZ B: Jucatorul e in zona, dar nu destul de aproape sa muste 
        else if (distance > attackRange && distance <= detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
        }
        // CAZ C: Jucatorul e langa paianjen -> ataca
        else if (distance <= attackRange)
        {
            agent.isStopped = true; 
            animator.SetBool("isWalking", false);
            AttackLogic();
        }
    }

    void AttackLogic()
    {
        // Rotire spre jucator
        Vector3 direction = (player.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks)
        {
            animator.SetTrigger("attack"); // Declansare animatie de atac
            animator.SetBool("isAttacking", true);

            // Da damage cu o mica intarziere (cand animația loveste efectiv)
            StartCoroutine(DealDamage(0.5f));

            attackTimer = 0f;
        }
    }

    IEnumerator DealDamage(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Verificam daca jucatorul e inca in range si viu
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange + 1f)
        {
            PlayerStats stats = player.GetComponentInParent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage((int)damageDealt, transform);
            }
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        agent.enabled = false;
        GetComponent<Collider>().enabled = false;

        animator.SetTrigger("die"); 

        Destroy(gameObject, 4f);
    }

    // Desenam cercuri in Editor ca sa vezi zona (Vizualizare Debug)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Zona Galbena = Te vede

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Zona Rosie = Te musca
    }
}