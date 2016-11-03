using UnityEngine;
using System.Collections;
using Leap.Unity;

public class EnableExplosion : MonoBehaviour
{
  public void OnTriggerEnter(Collider other)
  {
    Explosion ex = GetComponentInParent<Explosion>();

    if (other.GetComponentInParent<IHandModel>() != null && ex != null)
      ex.enabled = !ex.enabled;
  }
}