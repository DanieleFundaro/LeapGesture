using UnityEngine;
using System.Collections;
using Leap.Unity;

public class EnableFarAndNear : MonoBehaviour
{
  public void OnTriggerEnter(Collider other)
  {
    FarAndNear fan = GetComponentInParent<FarAndNear>();

    if (other.GetComponentInParent<IHandModel>() != null && fan != null)
      fan.enabled = !fan.enabled;
  }
}
