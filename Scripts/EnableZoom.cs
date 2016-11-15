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

    Transform hm = GetComponentInParent<IHandModel>().transform.parent;
    IHandModel[] mani = hm.GetComponentsInChildren<IHandModel>();

    foreach (IHandModel ihm in mani)
      if (ihm is RigidHand)
      {
        EnableZoom ez = ihm.transform.GetChild(5).GetChild(0).GetComponentInChildren<EnableZoom>();

        if (ez != null)
          ez.Start();
      }
  }
}
