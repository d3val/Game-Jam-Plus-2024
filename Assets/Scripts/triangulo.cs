using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triangulo : MonoBehaviour
{
    private void Start()
    {
        HolaMundo.instance.rederer.color = Color.yellow;
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }*/
}
