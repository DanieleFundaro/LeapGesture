using UnityEngine;
using System.Collections;
using Leap.Unity;

public class EnableImplosion : MonoBehaviour
{
  public void Start()
  {
    Utility.Enable<Implosion>(transform, null, false);
  }

  public void OnTriggerEnter(Collider other)
  {
    Utility.Enable<Implosion>(transform, other);

    if(enabled)
    {
      Explosion ex = GetComponentInParent<Explosion>();
      ex.enabled = false;
      EnableExplosion eex = ex.GetComponentInChildren<EnableExplosion>();
      eex.Start();
    }
  }
}