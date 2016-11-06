using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;

public class ResetAll : MonoBehaviour
{
  private Dictionary<Transform, Vector3> posIniziali;
  private Dictionary<Transform, Quaternion> rotIniziali;

  // Use this for initialization
  void Start()
  {
    // Calcolo le posizioni iniziali di tutti gli oggetti presenti nella scena. All'occorrenza utilizzerò questi valori per effettuare qualche controllo
    posIniziali = new Dictionary<Transform, Vector3>();
    rotIniziali = new Dictionary<Transform, Quaternion>();
    Transform[] objs = FindObjectsOfType<Transform>();

    foreach (Transform obj in objs)
    {
      posIniziali.Add(obj, new Vector3(obj.position.x, obj.position.y, obj.position.z));
      rotIniziali.Add(obj, new Quaternion(obj.rotation.x, obj.rotation.y, obj.rotation.z, obj.rotation.w));
    }
  }

  public void OnTriggerEnter(Collider other)
  {
    if (other.GetComponentInParent<IHandModel>() != null)
    {
      other.SendMessageUpwards("InZoom", true);

      Transform[] objs = FindObjectsOfType<Transform>();

      foreach (Transform obj in objs)
        if (obj.tag != "Imprendibile" && obj.GetComponentInParent<IHandModel>() == null)
          try
          {
            obj.position = posIniziali[obj];
            obj.rotation = rotIniziali[obj];
          }
          catch (KeyNotFoundException)
          {
            ;
          }

      other.SendMessageUpwards("InZoom", false);
    }
  }
}
