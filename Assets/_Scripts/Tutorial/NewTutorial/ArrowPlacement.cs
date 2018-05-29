using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ArrowPlacement : MonoBehaviour
{
    public List<Transform> TransformsToPutArrowOn;
    public Vector3 Offset;
    public Mesh ArrowMesh;

    private void OnDrawGizmosSelected()
    {
        if (ArrowMesh != null)
        {
            foreach (Transform transform in TransformsToPutArrowOn)
            {
                Gizmos.DrawWireMesh(ArrowMesh, transform.position + Offset, Quaternion.Euler(new Vector3(180,90,0)));
            }
        }
    }
}

