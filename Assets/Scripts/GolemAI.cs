using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;

public class GolemAI : MonoBehaviour
{
    public Transform player; // Tinta
    public float attackRange = 3f; // Distanta de la care loveste
    public float damageDealt = 20f; // Cat damage da
    public float timeBetweenAttacks = 2f; // Viteza de atac

    public float hitDelay = 0.8f;

    private NavMeshAgent agent;
    private Animator animator;
    private EnemyHealth healthScript; // Ca sa stim cand moare
    private float attackTimer = 0f;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        healthScript = GetComponent<EnemyHealth>();

        // Gasim jucatorul automat daca nu e pus manual
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // 1. Daca e mort, nu facem nimic
        // (Presupunand ca scriptul EnemyHealth are o variabila publica currentHealth)
        // Daca nu poti accesa healthScript.currentHealth, sterge verificarea asta momentan
        if (healthScript != null && healthScript.health <= 0)
        {
            if (!isDead) Die();
            return;
        }

        if (player == null) return;
        // 2. Calculam distanta
        float distance = Vector3.Distance(transform.position, player.position);

        // 3. Logica de Miscare vs Atac
        if (distance <= attackRange)
        {
            // E langa jucator -> Ataca!
            agent.isStopped = true; // Se opreste
            animator.SetBool("isWalking", false);
            AttackLogic();
        }
        else
        {
            // E departe -> Urmareste!
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false); // Oprim atacul
        }
    }

    void AttackLogic()
    {
        // Roteste spre jucator
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        attackTimer += Time.deltaTime;
        if (attackTimer >= timeBetweenAttacks)
        {
            // 1. Pornim animatia vizuala
            animator.SetTrigger("attack");
            animator.SetBool("isAttacking", true);

            // 2. Pornim timer-ul pentru damage (NU dam damage instant)
            StartCoroutine(ApplyDamageDelayed());

            attackTimer = 0f;
        }
    }

    IEnumerator ApplyDamageDelayed()
    {
        // Asteptam X secunde (cat dureaza ridicarea pumnului)
        yield return new WaitForSeconds(hitDelay);

        // Verificam din nou daca jucatorul mai exista si e aproape (optional)
        if (player != null)
        {
            // Cautam scriptul de viata
            PlayerStats stats = player.GetComponentInParent<PlayerStats>();

            if (stats != null)
            {
                stats.TakeDamage((int)damageDealt, transform);
            }
        }
    }

    void Die()
    {
        isDead = true;
        agent.enabled = false; // Nu se mai misca
        GetComponent<Collider>().enabled = false; // Nu il mai poti lovi
        animator.SetTrigger("die"); // Animatia de moarte (nu exista momentan)
        StartCoroutine(CadereDramatica());
        Destroy(gameObject, 4f);
    }

    System.Collections.IEnumerator CadereDramatica()
    {
        float timer = 0;
        Quaternion startRot = transform.rotation;
        // Nota: Daca cade in fata, pune 90 in loc de -90
        Quaternion endRot = startRot * Quaternion.Euler(-90, 0, 0);

        while (timer < 1f)
        {
            timer += Time.deltaTime * 1.5f; // Viteza caderii
            transform.rotation = Quaternion.Slerp(startRot, endRot, timer);
            yield return null;
        }
    }
}