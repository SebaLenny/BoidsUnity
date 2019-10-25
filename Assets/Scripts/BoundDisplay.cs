using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundDisplay : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(Vector3.zero, MasterController.Instance.bounds * 2);
        Gizmos.color = new Color(1, 1, 1, 0.05f);
        Gizmos.DrawCube(Vector3.zero, MasterController.Instance.bounds * 2);
    }
}