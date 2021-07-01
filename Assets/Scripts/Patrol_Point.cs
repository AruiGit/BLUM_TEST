using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol_Point : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.4f);
    }
}
