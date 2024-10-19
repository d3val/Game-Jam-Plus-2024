using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    [SerializeField] float throwForce = 5;
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
        rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
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
        if (collision.CompareTag("Enemy") && isBeingThrowed)
        {
            //Hacer daño al enemigo
            Impact();
        }
    }
}
