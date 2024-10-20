using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    [SerializeField] Vector3 offSet;
    [SerializeField] GameObject target;

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + offSet;
    }
}
