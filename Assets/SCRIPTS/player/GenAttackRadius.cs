using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenAttackRadius : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public int subdivisions = 10;
    [SerializeField] private PlayerUnit stats;
    
    public void GenerateAttackRadius() {
        lineRenderer = gameObject.GetComponent<LineRenderer>();

        float angleStep = 2f * Mathf.PI / subdivisions;
        lineRenderer.positionCount = subdivisions;

        for (int i = 0; i < subdivisions; i++) {
            float xPosition = stats.AttackRange * Mathf.Cos(angleStep * i);
            float zPosition = stats.AttackRange * Mathf.Sin(angleStep * i);

            Vector3 pointInCircle = new Vector3(xPosition, zPosition, 0f);

            lineRenderer.SetPosition(i, pointInCircle);
        }
    }
}
