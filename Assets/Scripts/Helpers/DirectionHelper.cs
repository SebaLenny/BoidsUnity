
using System.Collections.Generic;
using UnityEngine;

public class DirectionHelper
{
    private static int samplePointsCount = 100;
    private static List<Vector3> pointsOnSphere;
    public static List<Vector3> PointsOnSphere
    {
        get
        {
            if (pointsOnSphere == null)
                GeneratePoints();
            return pointsOnSphere;
        }
    }

    /// Reference https://stackoverflow.com/a/44164075
    private static void GeneratePoints()
    {
        pointsOnSphere = new List<Vector3>();
        float thetaBase = Mathf.PI * (1 + Mathf.Sqrt(5));
        for (int i = 0; i < samplePointsCount; i++)
        {
            float k = i + .5f;
            float phi = Mathf.Acos(1 - 2 * k / samplePointsCount);
            float theta = thetaBase * k;
            float sinPhi = Mathf.Sin(phi);
            pointsOnSphere.Add(new Vector3(
                Mathf.Cos(theta) * sinPhi,
                Mathf.Sin(theta) * sinPhi,
                Mathf.Cos(phi)
            ));
        }
    }
}
