using UnityEngine;
using Leap.Unity;

public class Zoom : MonoBehaviour
{
  public RigidHand manoDestra, manoSinistra;
  public float grabAngleMin = 2.6f, grabAngleMax = 2.8f;
  private float offset = 0;

  // Update is called once per frame
  void Update()
  {
    if (manoDestra.IsTracked && manoSinistra.IsTracked)
      if (manoDestra.GetLeapHand().GrabAngle >= grabAngleMin && manoDestra.GetLeapHand().GrabAngle <= grabAngleMax  && manoSinistra.GetLeapHand().GrabAngle >= grabAngleMin && manoSinistra.GetLeapHand().GrabAngle <= grabAngleMax && manoDestra.GetPalmNormal().z / manoSinistra.GetPalmNormal().z < 0 && manoDestra.GetPalmDirection().x < 0 && manoSinistra.GetPalmDirection().x > 0)
      {
        BroadcastMessage("InZoom", true);
        Camera.main.fieldOfView = (offset + Distanza());
      }
      else
      {
        offset = Camera.main.fieldOfView - Distanza();
        BroadcastMessage("InZoom", false);
      }
  }

  private float Distanza()
  {
    return (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()).magnitude * 100;
  }
}