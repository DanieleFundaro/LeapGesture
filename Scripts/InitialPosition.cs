using UnityEngine;
using System.Collections.Generic;

public class InitialPosition : MonoBehaviour
{
  private static Dictionary<Transform, Vector3> pi = new Dictionary<Transform, Vector3>();

  // Use this for initialization
  private void Start()
  {
    // Calcolo le posizioni iniziali di tutti gli oggetti presenti nella scena. All'occorrenza utilizzerò questi valori per effettuare qualche controllo
    MeshRenderer[] objs = FindObjectsOfType<MeshRenderer>();

    foreach (MeshRenderer obj in objs)
    {
      Transform p = Utility.GetPrimoPadre(obj.transform);
      Vector3 dir = new Vector3(obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);

      if (p != null)
      {
        dir.x -= p.position.x;
        dir.y -= p.position.y;
        dir.z -= p.position.z;
      }

      pi.Add(obj.transform, dir);
    }
  }

  public static Dictionary<Transform, Vector3> Direzioni { get { return pi; } }
}