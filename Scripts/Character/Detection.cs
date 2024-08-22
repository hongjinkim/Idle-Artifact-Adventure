using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public Vector2 point = new Vector2(0, 0);
    public Vector2 size = new Vector2(2, 2);
    public float angle = 0;
    public LayerMask layerMask;
    public List<GameObject> detectedObj = new List<GameObject>();

    private void Update()
    {
        DetectObjects();
    }
    public GameObject GetClosestObject()
    {
        if (detectedObj.Count > 0)
        {
            return detectedObj[0];
        }
        return null;
    }
    public void DetectObjects()
    {
        detectedObj.Clear();
        Collider2D[] detectedColliders = Physics2D.OverlapBoxAll(point, size, angle, layerMask);
        foreach (Collider2D collider in detectedColliders)
        {
            if (((1 << collider.gameObject.layer) & layerMask) != 0)
            {
                detectedObj.Add(collider.gameObject);
            }
        }
        detectedObj.Sort((a, b) =>
        {
            float distA = Vector3.Distance(transform.position, a.transform.position);
            float distB = Vector3.Distance(transform.position, b.transform.position);
            return distA.CompareTo(distB);
        });
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(point, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);
    }
}