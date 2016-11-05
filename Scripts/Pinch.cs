using UnityEngine;
using System.Collections;
using Leap.Unity;

public class Pinch : MonoBehaviour
{
  public RigidHand mano;
  public float minPinch = 0.9f;
  private Collider colliso;
  private Transform padre;

  public void OnValidate()
  {
    IHandModel ihm = gameObject.GetComponentInParent<IHandModel>();

    if (ihm != null)
      mano = (RigidHand)ihm;
  }

  public void Start()
  {
    colliso = null;
    padre = null;
  }
  public void OnTriggerEnter(Collider other)
  {
    if(colliso == null && other.tag != "Imprendibile")
    {
      SendMessageUpwards("StoAfferrando", true);
      colliso = other;
      padre = other.transform.parent;
    }
  }

  public void OnTriggerStay(Collider other)
  {
    if (colliso != null)
      mano.Pinch(colliso, padre, transform, minPinch, null);
  }

  public void OnTriggerExit(Collider other)
  {
    if(colliso != null)
    {
      mano.StopPinch(colliso, padre);
      colliso = null;
      padre = null;
      SendMessageUpwards("StoAfferrando", false);
    }
  }
}
