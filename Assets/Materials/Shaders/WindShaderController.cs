using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WindShaderController : MonoBehaviour {

    public Vector2 RandomOffsetRange;

	void Awake ()
    {
        List<GameObject> objects = GameObject.FindGameObjectsWithTag("Windy").ToList();
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        MeshRenderer renderer;

        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i].GetComponent<MeshFilter>() == null)
            {
                continue;
            }
            float r = Random.Range(RandomOffsetRange.x, RandomOffsetRange.y);
            props.SetFloat("_RandomOffset", r);
            props.SetVectorArray("_Bounds", FindBoundsInWorldSpace(objects[i].GetComponent<MeshFilter>().sharedMesh));
            renderer = objects[i].GetComponent<MeshRenderer>();
            renderer.SetPropertyBlock(props);
        }
    }

    List<Vector4> FindBoundsInWorldSpace(Mesh pMesh)
    {
        Mesh mesh = pMesh;
        mesh.RecalculateBounds();

        Bounds bounds = mesh.bounds;
        Vector3 center = transform.TransformPoint(bounds.center);
        Vector3 extents = bounds.extents;

        List<Vector4> corners = new List<Vector4>()
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
        return corners;
    }
}
