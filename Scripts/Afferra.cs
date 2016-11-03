using UnityEngine;
using Leap;
using Leap.Unity;

public class Afferra : MonoBehaviour
{
  public RigidHand mano;
  public float minGrab = 0.5f;
  private Transform colliso;

  public void OnValidate()
  {
    IHandModel ihm = this.gameObject.GetComponent<IHandModel>();

    if (ihm != null)
      mano = (RigidHand)ihm;
  }

  public void Start()
  {
    colliso = null;
  }

  public void OnTriggerEnter(Collider other)
  {
    Controller c = new Controller();

    if (c.IsConnected && colliso == null && other.tag != "Imprendibile")
    {
      SendMessageUpwards("StoAfferrando", true);
      colliso = GetPadre(other.transform);
    }
  }

  public void OnTriggerStay(Collider other)
  {
    if (colliso != null)
      mano.StartGrab(colliso, minGrab);
  }

  public void OnTriggerExit(Collider other)
  {
    if (colliso != null)
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
}