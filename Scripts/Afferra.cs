using UnityEngine;
using Leap.Unity;

public class Afferra : MonoBehaviour
{
  public RigidHand mano;
  public float minGrab = 0.5f;
  private Transform colliso;
  private bool inZoom = false;

  public void OnValidate()
  {
    IHandModel ihm = gameObject.GetComponentInParent<IHandModel>();

    if (ihm != null)
      mano = (RigidHand)ihm;
  }

  public void Start()
  {
    colliso = null;
  }

  public void OnTriggerEnter(Collider other)
  {
    if (!inZoom && colliso == null && other.tag != "Imprendibile" && other.tag != "MainCamera")
    {
      SendMessageUpwards("StoAfferrando", true);
      colliso = GetPadre(other.transform);
    }
  }

  public void OnTriggerStay(Collider other)
  {
    if (!inZoom && colliso != null)
      mano.StartGrab(colliso, minGrab);
  }

  public void OnTriggerExit(Collider other)
  {
    if (!inZoom && colliso != null)
    {
      mano.StopGrab(colliso);
      colliso = null;
      SendMessageUpwards("StoAfferrando", false);
    }
  }

  private Transform GetPadre(Transform other)
  {
    if (other == null)
      return null;

    if (other.parent == null)
      return other;

    return GetPadre(other.transform.parent);
  }

  private void InZoom(bool zoom)
  {
    inZoom = zoom;
  }
}