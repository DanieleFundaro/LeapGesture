using UnityEngine;
using Leap.Unity;

public class EnableFarAndNear : MonoBehaviour
{
  public void Start()
  {
    Utility.Enable<FarAndNear>(transform, null, false);
  }

  public void OnTriggerEnter(Collider other)
  {
    Utility.Enable<FarAndNear>(transform, other);
  }
}
