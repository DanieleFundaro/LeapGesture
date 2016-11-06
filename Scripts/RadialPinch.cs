using UnityEngine;
using System.Collections;
using Leap.Unity;
using System.Collections.Generic;

public class RadialPinch : MonoBehaviour
{
  public RigidHand mano;
  private Dictionary<Transform, Vector3> localPosIniziali;
  public float minPinch = 0.9f;
  private Transform colliso, padre;
  private bool inZoom = false;

  public void OnValidate()
  {
    IHandModel ihm = gameObject.GetComponentInParent<IHandModel>();

    if (ihm != null)
      mano = (RigidHand)ihm;
  }

  public void Start()
  {
    // Calcolo le posizioni iniziali di tutti gli oggetti presenti nella scena. All'occorrenza utilizzerò questi valori per effettuare qualche controllo
    localPosIniziali = new Dictionary<Transform, Vector3>();
    Transform[] objs = FindObjectsOfType<Transform>();

    foreach (Transform obj in objs)
      localPosIniziali.Add(obj, new Vector3(obj.localPosition.x, obj.localPosition.y, obj.localPosition.z));

    colliso = null;
    padre = null;
  }

  public void OnTriggerEnter(Collider other)
  {
    if (!inZoom && colliso == null && other.tag != "Imprendibile")
    {
      padre = other.transform.parent;

      if (padre != null)
      {
        SendMessageUpwards("StoAfferrando", true);
        colliso = other.transform;
      }
    }
  }

  public void OnTriggerStay(Collider other)
  {
    if (!inZoom && colliso != null && padre != null)
      mano.Pinch(colliso, padre, transform, localPosIniziali[colliso], minPinch);
  }

  public void OnTriggerExit(Collider other)
  {
    if (!inZoom && colliso != null)
    {
      colliso = null;
      padre = null;
      SendMessageUpwards("StoAfferrando", false);
    }
  }

  private void InZoom(bool zoom)
  {
    inZoom = zoom;
  }
}