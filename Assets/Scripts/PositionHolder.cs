using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionHolder : MonoBehaviour
{
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        transform.position = Vector3.zero;
    }
}
