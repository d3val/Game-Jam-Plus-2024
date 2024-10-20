using System.Collections;
using UnityEngine;

public class Charger : MonoBehaviour
{
    public Transform player;           // Referencia al jugador
    public float detectionRadius = 5.0f;  // Radio de detección
    public float impulseForce = 50.0f; // Fuerza del impulso
    public string targetTag = "Player";  // Tag del jugador

    private Rigidbody2D rb;          // Referencia al Rigidbody2D
    private bool hasDetected = false;  // Si ya detectó al jugador
    private bool wait = false;         // Control del tiempo de espera
    private bool prepare = true;       // Control de preparación del ataque
    private bool isStopped = false;    // Si el enemigo se ha detenido por una colisión
    public int damageAmount = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtener el Rigidbody2D
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }
        if (isStopped)
        {
            isStopped = false;
            hasDetected = false;

        }

        if (hasDetected || wait) return; // Si ya detectó o está detenido, no hace nada más


        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRadius && prepare)
        {
            StartCoroutine(PrepareAttack()); // Inicia la preparación del ataque
        }
    }

    private IEnumerator PrepareAttack()
    {
        wait = true;
        yield return new WaitForSeconds(1); // Espera antes del impulso

        Vector2 direction = (player.position - transform.position).normalized; // Dirección hacia el jugador
        rb.AddForce(direction * impulseForce, ForceMode2D.Impulse); // Aplica el impulso

        hasDetected = true; // Marca que ya detectó al jugador
        wait = false;
        isStopped = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Muro")) // Si colisiona con un muro
        {
            
            rb.velocity = Vector2.zero;  // Detiene el movimiento
            isStopped = true;  // Marca que el enemigo está detenido
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();

            if (player.isAttacking)
            {
                Debug.Log("Esta atacando!");
                receiveDamage();
            }
            else
            {
                if (player != null)  // Verifica que el jugador tenga el script
                {
                    player.receiveDamage(damageAmount);  // Aplica daño
                }
            }
        }
        if (collision.CompareTag("Muro")) // Si colisiona con un muro
        {
            
            rb.velocity = Vector2.zero;  // Detiene el movimiento
            isStopped = true;  // Marca que el enemigo está detenido
        }
    }

    // Recibir daño y destruir al enemigo
    public void receiveDamage()
    {
        Destroy(gameObject);
    }
}
