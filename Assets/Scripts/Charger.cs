using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 50.0f;
    public string targetTag = "Player";
    public int health = 100;

    private Rigidbody2D rb;
    private Vector2 movement;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            movement = new Vector2(direction.x, direction.y);
        }
        else
        {
            movement = Vector2.zero;
        }

        transform.Translate(movement * speed * Time.deltaTime);
    }

    // Hacer daño a jugador
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //collision.GetComponent<PlayerMovement>().RecieveDamage();
        }
    }


    //Recibir daño
    private void receiveDamage()
    {
        Destroy(gameObject);
    }

}


