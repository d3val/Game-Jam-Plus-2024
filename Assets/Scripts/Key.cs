using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public void RemoveKey()
    {
        LockedDoor.instance.RemoveKey(this.gameObject);
        Destroy(this.gameObject);
    }
}
