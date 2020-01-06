using UnityEngine;

public class CollisionUpcaster : MonoBehaviour
{
    Course course;
    private void Start()
    {
        course = GetComponentInParent<Course>();
    }
    private void OnTriggerEnter(Collider other)
    {
        course.TriggerFinish();
    }
}