using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicObject : ThrowableItem
{

    public override IEnumerator Fall()
    {

        yield return new WaitForSeconds(lifeTime);
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;  // Detiene el movimiento
        rb.gravityScale = 0;
        rb.angularDrag = 5f;  // Aumentar la fricción angular para disipar la rotación
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
