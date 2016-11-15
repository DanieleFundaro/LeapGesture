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

    Transform hm = GetComponentInParent<IHandModel>().transform.parent;
    IHandModel[] mani = hm.GetComponentsInChildren<IHandModel>();

    foreach (IHandModel ihm in mani)
      if (ihm is RigidHand)
      {
        EnableFarAndNear efn = ihm.transform.GetChild(5).GetChild(0).GetComponentInChildren<EnableFarAndNear>();

        if (efn != null)
          efn.Start();
      }
  }
}
