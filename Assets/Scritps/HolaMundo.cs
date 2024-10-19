using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolaMundo : MonoBehaviour
{
    public SpriteRenderer rederer;
    public GameObject otro;
    public float speed = 3.0f;
    public float force = 3.0f;
    public Rigidbody2D rigid;
    public static HolaMundo instance;

    int update, fixedUpdate;

    private void Awake()
    {
        instance = this;
        rederer = GetComponent<SpriteRenderer>();

    }

    // Start is called before the first frame update
    void Start()
    {

      //  rederer.color = Color.red;
        rigid = GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * force , ForceMode2D.Impulse);

    }
/*
    // Update is called once per frame
    void Update()
    {
        // transform.Translate(Vector2.right * speed * Time.deltaTime, Space.Self);
        Debug.Log("Llevo update: " + update);
        update++;
    }

    private void FixedUpdate()
    {
        Debug.Log("Llevo fixed: " + fixedUpdate);
        fixedUpdate++;
    }


    */
}
