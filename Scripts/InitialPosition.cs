using UnityEngine;
using System.Collections.Generic;

public class InitialPosition : MonoBehaviour
{
  private static Dictionary<Transform, Vector3> pi = new Dictionary<Transform, Vector3>();

  // Use this for initialization
  private void Start()
  {
    // Calcolo le posizioni iniziali di tutti gli oggetti presenti nella scena. All'occorrenza utilizzerò questi valori per effettuare qualche controllo
    MeshRenderer[] objsMeshRenderer = FindObjectsOfType<MeshRenderer>();

    foreach (MeshRenderer mr in objsMeshRenderer)
    {
      Transform obj = mr.transform, p = Utility.GetPrimoPadre(obj);
      Vector3 dir = new Vector3(obj.position.x, obj.position.y, obj.position.z);

      if (p != null)
      {
        dir.x -= p.position.x;
        dir.y -= p.position.y;
        dir.z -= p.position.z;
      }

      pi.Add(obj, dir);
    }
  }

  public static Dictionary<Transform, Vector3> Direzioni { get { return pi; } }
}