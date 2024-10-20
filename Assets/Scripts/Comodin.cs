using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comodin : MonoBehaviour
{
    public GameObject comodin;
    public void DeactivateCollider2DGameObject()
    {
        comodin.GetComponent<Collider2D>().gameObject.SetActive(false);
    }
}
