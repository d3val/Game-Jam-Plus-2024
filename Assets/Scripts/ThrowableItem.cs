using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableItem : MonoBehaviour
{
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Impact()
    {
        Debug.Log("Pum");
    }

    public void Throw(Vector2 direction)
    {
        rb.AddForce(direction, ForceMode2D.Impulse);
    }

    public void DetachParent()
    {
        transform.SetParent(null);
    }
    
}
