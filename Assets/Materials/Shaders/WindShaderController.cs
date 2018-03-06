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

        foreach (GameObject obj in objects)
        {
            float r = Random.Range(RandomOffsetRange.x, RandomOffsetRange.y);
            props.SetFloat("_RandomOffset", r);
            props.SetVectorArray("_Bounds", FindBoundsInWorldSpace(obj.GetComponent<MeshFilter>().mesh));
            renderer = obj.GetComponent<MeshRenderer>();
            renderer.SetPropertyBlock(props);
        }
    }

    List<Vector4> FindBoundsInWorldSpace(Mesh pMesh)
    {
        Mesh mesh = gameObject.GetComponent<MeshFilter>().mesh;
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
