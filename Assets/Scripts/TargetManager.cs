﻿using UnityEngine;

[RequireComponent(typeof(BoidController))]
public class TargetManager : MonoBehaviour
{
    private BoidController bc;
    private void Awake()
    {
        bc = GetComponent<BoidController>();
    }

    private void Start()
    {
        bc.currentTarget = bc.ruleSet.course.getFirstTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (bc.currentTarget == other.gameObject)
        {
            bc.currentTarget = bc.ruleSet.course.getNextTarget(other.gameObject);
        }
    }
}