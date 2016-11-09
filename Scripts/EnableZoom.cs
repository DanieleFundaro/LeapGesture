using UnityEngine;
using Leap.Unity;

public class EnableZoom : MonoBehaviour
{
  public void Start()
  {
    Utility.Enable<Zoom>(transform, null, false);
  }

  public void OnTriggerEnter(Collider other)
  {
    Utility.Enable<Zoom>(transform, other);
  }
}
