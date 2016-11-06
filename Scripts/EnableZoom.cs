using UnityEngine;
using Leap.Unity;

public class EnableZoom : MonoBehaviour
{
  public void OnTriggerEnter(Collider other)
  {
    Zoom z = GetComponentInParent<Zoom>();

    if (other.GetComponentInParent<IHandModel>() != null && z != null)
      z.enabled = !z.enabled;
  }
}
