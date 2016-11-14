using UnityEngine;
using Leap.Unity;
using System.Collections.Generic;

public class RadialPinch : MonoBehaviour
{
  public RigidHand mano;
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
    Init();
  }

  public void OnTriggerEnter(Collider other)
  {
    if (!inZoom && colliso == null && other.tag != "Imprendibile" && other.tag != "MainCamera")
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
        mano.Pinch(colliso, transform, padre, InitialPosition.Posizioni[colliso], minPinch);
  }

  public void OnTriggerExit(Collider other)
  {
    if (!inZoom && colliso != null)
    {
      Init();
      SendMessageUpwards("StoAfferrando", false);
    }
  }

  private void Init()
  {
    colliso = null;
    padre = null;
  }

  private void InZoom(bool zoom)
  {
    inZoom = zoom;
  }
}