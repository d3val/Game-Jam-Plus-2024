using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    [SerializeField] float torque = 10;
    [SerializeField] float lifeTime = 0.5f;
    bool isBeingThrowed = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Impact()
    {
        Debug.Log("Pum");
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public void Throw(Vector2 direction)
    {
        rb.isKinematic = false;
        isBeingThrowed = true;
        StartCoroutine(Fall());
        rb.AddForce(direction, ForceMode2D.Impulse);
        rb.AddTorque(torque, ForceMode2D.Impulse);
        rb.gravityScale = 1;
    }

    public void DetachParent()
    {
        transform.SetParent(null);
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isBeingThrowed)
        {
            return;
        }

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
}
