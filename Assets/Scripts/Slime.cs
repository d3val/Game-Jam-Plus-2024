using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 8.0f;
    public float attackRadius = 2.0f;
    public float attackChargeTime = 1.0f;
    public float attackSpeed = 10.0f;
    public float speed = 5.0f;
    public string targetTag = "Player";

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isAttacking = false;
    private float attackTimer = 0.0f;
    private bool getPosition = true;
    private Vector3 tempPosition;
    private bool wait = false;
    private float tempSpeed;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wait)
        {
            return;
        }
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRadius)
        {
            tempSpeed = speed;
            if (distanceToPlayer < attackRadius)
            {
                if (getPosition)
                {
                    tempPosition = player.position;
                    getPosition = false;
                    isAttacking = true;
                    StartCoroutine(prepareAttack());
                }
                tempSpeed = attackSpeed;
            }
            if (isAttacking)
            {
                Vector2 direction = (tempPosition - transform.position).normalized;
                movement = new Vector2(direction.x, direction.y);

                //Codigo para experar un tiempo de aturdimiento, luego establecer isAtacking en false

                
                if(Vector2.Distance(transform.position, tempPosition) <=0.1)
                {
                    StartCoroutine(prepareAttack());

                    isAttacking = false;
                    getPosition = true;
                }
            }
            else
            {
                Vector2 direction = (player.position - transform.position).normalized;
                movement = new Vector2(direction.x, direction.y);
            }
            
        }
        else
        {
            movement = Vector2.zero;
        }

        transform.Translate(movement * tempSpeed * Time.deltaTime);
    }

    private IEnumerator prepareAttack()
    {

        wait = true;
        yield return new WaitForSeconds(0.5f);
        wait = false;
    }


    // Hacer daño a jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //collision.GetComponent<PlayerMovement>().RecieveDamage();
            Destroy(gameObject);
        }
    }


    //Recibir daño
    private void receiveDamage()
    {
        Destroy(gameObject);
    }

}


