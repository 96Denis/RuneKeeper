using UnityEngine;

public class LumberjackAI : MonoBehaviour
{
    private Animator anim;
    public float moveSpeed = 1.0f;
    public Transform targetTree;
    public float choppingDistance = 1f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (targetTree != null)
        {
            float distanceToTree = Vector3.Distance(transform.position, targetTree.position);

            if (distanceToTree > choppingDistance)
            {
                // MERS
                anim.SetBool("IsWalking", true);
                anim.ResetTrigger("StartChopping");

                Vector3 direction = (targetTree.position - transform.position).normalized;
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
                }
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                // TĂIERE LOOP AUTOMAT (fără isChopping manual!)
                anim.SetBool("IsWalking", false);

                // NOU: Detectează SFÂRȘIT animație AUTOMAT
                AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
                bool isLumberingFinished = state.IsName("Lumbering") && state.normalizedTime >= 0.95f;  // 95% ca să nu rateze

                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Lumbering") || isLumberingFinished)
                {
                    anim.SetTrigger("StartChopping");  // Declanșează dacă NU e în Lumbering sau a terminat
                }
            }
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }
}