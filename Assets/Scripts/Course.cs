using System.Collections.Generic;
using UnityEngine;

public class Course : MonoBehaviour
{
    public bool looping = false;
    public bool showPath = false;
    public List<GameObject> nodes;

    private void OnValidate()
    {
        if (nodes != null)
        {
            foreach (Transform child in transform)
            {
                if (nodes.IndexOf(child.gameObject) == -1)
                    nodes.Add(child.gameObject);
            }
        }
    }

    private void Update()
    {
        if (showPath && nodes.Count > 1)
        {
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                Debug.DrawLine(nodes[i].transform.position, nodes[i + 1].transform.position);
            }
            if (looping)
            {
                Debug.DrawLine(nodes[nodes.Count - 1].transform.position, nodes[0].transform.position);
            }
        }
    }
    public GameObject getNextTarget(GameObject reachedTarget)
    {
        int indexOfReached = nodes.IndexOf(reachedTarget);
        if (indexOfReached == -1) return null;
        int nextIndex = (indexOfReached + 1) % nodes.Count;
        if (looping) return nodes[nextIndex];
        else if (nextIndex != 0) return nodes[nextIndex];
        else return null;
    }

    public GameObject getFirstTarget()
    {
        return nodes?[0];
    }
}
