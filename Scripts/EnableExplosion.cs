using UnityEngine;
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

    if (enabled && other.GetComponentInParent<IHandModel>() != null)
    {
      Implosion im = GetComponentInParent<Implosion>();
      im.enabled = false;
      EnableImplosion eim = im.GetComponentInChildren<EnableImplosion>();
      eim.Start();
    }
  }
}