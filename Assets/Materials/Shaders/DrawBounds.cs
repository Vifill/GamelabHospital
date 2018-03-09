using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBounds : MonoBehaviour
{
    void OnDrawGizmos()
    {
        if (gameObject.GetComponent<MeshFilter>() == null)
        {
            return;
        }

        Mesh mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
        mesh.RecalculateBounds();
        Bounds bounds = mesh.bounds;
        
        Vector3 extents = bounds.extents;
        Vector3 center = transform.TransformPoint(bounds.center);
        Vector3[] corners = new Vector3[]
        {
            center + extents,
            center + new Vector3(extents.x, extents.y, -extents.z),
            center + new Vector3(extents.x, -extents.y,-extents.z),
            center + new Vector3(extents.x, -extents.y, extents.z),
            center + new Vector3(-extents.x, -extents.y, extents.z),
            center + new Vector3(-extents.x, extents.y, -extents.z),
            center + new Vector3(-extents.x, extents.y, extents.z),
            center - extents
        };

        Gizmos.color = Color.red;
        for (int i = 0; i < corners.Length; i++)
        {
            Gizmos.DrawSphere(corners[i], 0.2f);
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube( transform.TransformPoint(bounds.center), bounds.size);
    }
}