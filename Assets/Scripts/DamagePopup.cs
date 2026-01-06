using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    public float disappearSpeed = 0.1f;
    public float lifeTime = 1.2f;

    private TextMeshProUGUI textMesh;
    private Color textColor;
    private Transform cameraTransform;

    void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        textColor = textMesh.color;
        cameraTransform = Camera.main.transform;
    }

    public void Setup(int damageAmount)
    {
        textMesh.text = damageAmount.ToString();
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        transform.LookAt(transform.position + cameraTransform.forward);
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}