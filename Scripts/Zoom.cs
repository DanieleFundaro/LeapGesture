using UnityEngine;
using Leap.Unity;

public class Zoom : MonoBehaviour
{
  public RigidHand manoDestra, manoSinistra;
  public float grabAngle = 2.6f;
  private float offset = 0;

  // Update is called once per frame
  void Update()
  {
    if (manoDestra.IsTracked && manoSinistra.IsTracked)
      if (manoDestra.GetLeapHand().GrabAngle >= grabAngle && manoSinistra.GetLeapHand().GrabAngle >= grabAngle && manoDestra.GetPalmNormal().z / manoSinistra.GetPalmNormal().z < 0 && manoDestra.GetPalmDirection().x / manoSinistra.GetPalmDirection().x < 0)
      {
        SendMessageUpwards("InZoom", true);
        Camera.main.fieldOfView = (offset - Distanza());
      }
      else
      {
        offset = Distanza() + Camera.main.fieldOfView;
        SendMessageUpwards("InZoom", false);
      }
  }

  private float Distanza()
  {
    return (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()).magnitude * 100;
  }
}