using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

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
    bool isAttacking = false;
    bool wereRepeled = false;
    public float lifeTime = 2;
    public UnityEvent OnRepeled;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Obtener el Rigidbody2D
        player = GameObject.FindWithTag(targetTag).transform;
    }

    void Update()
    {
        if (player == null)
        {
            return;
        }
        CalculateSpriteDirection();
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
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(1); // Espera antes del impulso

        Vector2 direction = (player.position - transform.position).normalized; // Dirección hacia el jugador
        rb.AddForce(direction * impulseForce, ForceMode2D.Impulse); // Aplica el impulso

        isAttacking = true;
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
        if (wereRepeled)
        {
            if (collision.gameObject.CompareTag("Charger"))
            {
                collision.gameObject.GetComponent<Charger>().receiveDamage();
            }
            else if (collision.gameObject.CompareTag("BasicEnemy"))
            {
                collision.gameObject.GetComponent<BasicEnemy>().receiveDamage();
            }
            else if (collision.gameObject.CompareTag("Slime"))
            {
                collision.gameObject.GetComponent<Slime>().receiveDamage();
            }

        }

        /*if (collision.CompareTag("Player"))
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();

            if (player.isAttacking)
            {
                Debug.Log("Esta atacando!");
                //receiveDamage();
            }
            else
            {
                if (player != null)  // Verifica que el jugador tenga el script
                {
                    player.receiveDamage(damageAmount);  // Aplica daño
                
            }
        }
        if (collision.CompareTag("Muro")) // Si colisiona con un muro
        {

            rb.velocity = Vector2.zero;  // Detiene el movimiento
            StopAllCoroutines();
            isStopped = true;  // Marca que el enemigo está detenido
        }*/
    }

    // Recibir daño y destruir al enemigo
    public void receiveDamage()
    {
        if (isAttacking)
        {
            StartCoroutine(ChangeDirection());
            return;
        }

        Destroy(gameObject);
    }

    IEnumerator ChangeDirection()
    {
        wereRepeled = true;
        isAttacking = false;
        OnRepeled.Invoke();
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<SpriteRenderer>().color = Color.blue;
        Vector2 direction = (transform.position - player.position).normalized; // Dirección hacia el jugador
        rb.AddForce(direction * impulseForce, ForceMode2D.Impulse); // Aplica el 
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    void CalculateSpriteDirection()
    {
        Vector2 direction = player.position - transform.position;
        bool condition;
        if (direction.x < transform.position.x)
        {
            condition = true;
        }
        else
        {
            condition = false;
        }

        if (wereRepeled)
            condition = !condition;

        GetComponent<SpriteRenderer>().flipX = condition;
    }
}
