using UnityEngine;
using System.Collections;
using Leap.Unity;

public class EnableExplosion : MonoBehaviour
{
  public void Start()
  {
    Utility.Enable<Explosion>(transform, null, false);
  }

  public void OnTriggerEnter(Collider other)
  {
    Utility.Enable<Explosion>(transform, other);
  }
}