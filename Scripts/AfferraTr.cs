using UnityEngine;
using Leap;
using Leap.Unity;
using System;

public class AfferraTr : MonoBehaviour
{
  public RigidHand mano;
  public float minGrab = 0.5f;
  private Collider colliso;
  private Transform padre;

  public void OnValidate()
  {
    IHandModel ihm = this.gameObject.GetComponent<IHandModel>();

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
    Controller c = new Controller();

    if (c.IsConnected && colliso == null && padre == null && other.tag != "Imprendibile")
    {
      SendMessageUpwards("StoAfferrando", true);
      colliso = other;
      padre = other.transform.parent;
    }
  }

  public void OnTriggerStay(Collider other)
  {
    if (other == colliso)
      mano.StartGrab(other, minGrab, padre);
  }

  public void OnTriggerExit(Collider other)
  {
    if (other == colliso)
    {
      mano.StopGrab(other, padre);
      colliso = null;
      padre = null;
      SendMessageUpwards("StoAfferrando", false);
    }
  }
}