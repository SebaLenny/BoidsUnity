using System.Collections.Generic;
using UnityEngine;

public class Course : MonoBehaviour
{
    public bool looping = false;
    public bool showPath = false;
    public LinkedList<GameObject> nodes;

    private void Awake()
    {
        nodes = new LinkedList<GameObject>();
        foreach (Transform child in transform)
        {
            nodes.AddLast(child.gameObject);
        }
    }

    private void Update()
    {
        if (nodes.Count < 2)
            return;
        var node = nodes.First;
        while (node != null && node.Next != null)
        {
            Vector3 a = node.Value.transform.position;
            node = node.Next;
            Vector3 b = node.Value.transform.position;
            Debug.DrawLine(a, b);
        }
        if (looping)
            Debug.DrawLine(nodes.First.Value.transform.position, nodes.Last.Value.transform.position);
    }

    public GameObject getNextTarget(GameObject reachedTarget)
    {
        if (nodes.Last.Value == reachedTarget && looping)
            return getFirstTarget();
        return nodes.Find(reachedTarget).Next?.Value;
    }

    public GameObject getFirstTarget()
    {
        return nodes.First.Value;
    }
}
