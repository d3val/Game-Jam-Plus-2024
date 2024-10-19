using System.Collections;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public Transform player;               // Referencia al jugador
    public float detectionRadius = 5.0f;   // Radio de detección
    public float jumpDistance = 1.0f;      // Distancia recorrida por cada salto
    public float jumpSpeed = 5.0f;         // Velocidad del salto
    public float pauseTime = 0.5f;         // Tiempo de pausa entre saltos
    public float jumpInterval = 1.0f;      // Tiempo entre intentos de salto

    private Rigidbody2D rb;                // Referencia al Rigidbody2D
    private bool isJumping = false;        // Controla si está saltando
    private float jumpTimer = 0.0f;        // Controla el intervalo entre saltos

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Inicializa el Rigidbody2D
        rb.gravityScale = 0;               // Desactiva la gravedad si no la necesitas
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


        if (distanceToPlayer <= detectionRadius)  // Si el jugador está en rango
        {
            jumpTimer += Time.deltaTime;

            if (!isJumping && jumpTimer >= jumpInterval)  // Inicia un salto si no está saltando
            {
                StartCoroutine(JumpTowardsPlayer());  // Ejecuta la corrutina del salto
                jumpTimer = 0;  // Reinicia el temporizador de salto
            }
        }
    }

    private IEnumerator JumpTowardsPlayer()
    {
        isJumping = true;  // Marca que el slime está en movimiento

        // Calcula la dirección hacia el jugador
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 targetPosition = rb.position + direction * jumpDistance;

        // Desactiva la física momentáneamente durante el salto
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        // Mueve el slime hacia la posición objetivo
        while (Vector2.Distance(rb.position, targetPosition) > 0.1f)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, jumpSpeed * Time.deltaTime));
            yield return null;  // Espera un frame antes de continuar
        }

        // Pausa en la nueva posición
        yield return new WaitForSeconds(pauseTime);

        // Reactiva la física y el movimiento
        rb.isKinematic = false;
        rb.velocity = Vector2.zero;

        isJumping = false;  // Marca que terminó el salto
    }
}
