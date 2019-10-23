using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MasterController : Singleton<MasterController>
{
    public Vector3 bounds;
    public bool aligment;
    [Range(0f, 5f)]
    public float aligmentStrenght = 1;
    public bool separation;
    [Range(0f, 5f)]
    public float separationStrenght = 1;
    public bool cohesion;
    [Range(0f, 5f)]
    public float cohesionStrenght = 1;



}
