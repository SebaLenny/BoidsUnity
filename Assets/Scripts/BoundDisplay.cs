using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundDisplay : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, MasterController.Instance.bounds * 2);
        //Gizmos.DrawCube(Vector3.zero, bounds);
    }
}
