using System.Collections;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public Transform player;               // Referencia al jugador
    public float detectionRadius = 5.0f;   // Radio de detecci�n
    public float jumpDistance = 1.0f;      // Distancia recorrida por cada salto
    public float jumpSpeed = 5.0f;         // Velocidad del salto
    public float pauseTime = 0.5f;         // Tiempo de pausa entre saltos
    public float jumpInterval = 1.0f;      // Tiempo entre intentos de salto

    private Rigidbody2D rb;                // Referencia al Rigidbody2D
    private bool isJumping = false;        // Controla si est� saltando
    private float jumpTimer = 0.0f;        // Controla el intervalo entre saltos

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Inicializa el Rigidbody2D
        rb.gravityScale = 0;               // Desactiva la gravedad si no la necesitas
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


        if (distanceToPlayer <= detectionRadius)  // Si el jugador est� en rango
        {
            jumpTimer += Time.deltaTime;

            if (!isJumping && jumpTimer >= jumpInterval)  // Inicia un salto si no est� saltando
            {
                StartCoroutine(JumpTowardsPlayer());  // Ejecuta la corrutina del salto
                jumpTimer = 0;  // Reinicia el temporizador de salto
            }
        }
    }

    private IEnumerator JumpTowardsPlayer()
    {
        isJumping = true;  // Marca que el slime est� en movimiento

        // Calcula la direcci�n hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 targetPosition = rb.position + direction * jumpDistance;

        // Desactiva la f�sica moment�neamente durante el salto
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        // Mueve el slime hacia la posici�n objetivo
        while (Vector2.Distance(rb.position, targetPosition) > 0.1f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, jumpSpeed * Time.deltaTime));
            yield return null;  // Espera un frame antes de continuar
        }

        // Pausa en la nueva posici�n
        yield return new WaitForSeconds(pauseTime);

        // Reactiva la f�sica y el movimiento
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;

        isJumping = false;  // Marca que termin� el salto
    }
}
