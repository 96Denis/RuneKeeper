using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    // FIX: Am schimbat in 'private' si cu litera mica pentru a se potrivi cu restul codului
    private Vector3 damageLocation;

    public Transform DamageImagePivot;
    public CanvasGroup DamageImageCanvasGroup;

    public float FadeTime = 1.5f;
    private float timer;

    void Start()
    {
        timer = FadeTime;
        // Centram pivotul pe ecran (siguranta)
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    // Aceasta functie este apelata manual imediat dupa ce cream obiectul
    public void Setup(Transform attacker)
    {
        if (attacker != null)
        {
            // Acum numele variabilei corespunde cu cel de sus
            damageLocation = attacker.position;
        }

        // FORTAM rotatia ACUM
        RotateIndicator();
    }

    void Update()
    {
        // 1. Logica de Fade
        timer -= Time.deltaTime;
        if (DamageImageCanvasGroup != null)
            DamageImageCanvasGroup.alpha = timer / FadeTime;

        if (timer <= 0) Destroy(gameObject);

        // 2. Logica de Rotatie FPS
        RotateIndicator();
    }

    void RotateIndicator()
    {
        if (Camera.main == null) return;

        // FIX: Folosim variabila 'damageLocation' (cu litera mica)
        Vector3 directionToEnemy = damageLocation - Camera.main.transform.position;
        directionToEnemy.y = 0;

        Vector3 playerLookingDirection = Camera.main.transform.forward;
        playerLookingDirection.y = 0;

        float angle = Vector3.SignedAngle(playerLookingDirection, directionToEnemy, Vector3.up);

        if (DamageImagePivot != null)
        {
            DamageImagePivot.localRotation = Quaternion.Euler(0, 0, -angle);
        }
    }
}