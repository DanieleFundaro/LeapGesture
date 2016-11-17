using UnityEngine;
using System.Collections;

public class AddMeshCollider : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {
    MeshRenderer[] objsMeshRenderer = FindObjectsOfType<MeshRenderer>();

    foreach (MeshRenderer obj in objsMeshRenderer)
      if (Utility.TagDaEvitare(obj.transform))
      {
        Collider c = obj.GetComponent<Collider>();

        if (c == null)
        {
          obj.gameObject.AddComponent<MeshCollider>();
          c = obj.GetComponent<MeshCollider>();
          ((MeshCollider)c).convex = true;
        }

        c.isTrigger = true;
      }
  }
}