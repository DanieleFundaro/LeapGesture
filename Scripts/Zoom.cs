using UnityEngine;
using Leap.Unity;

public class Zoom : MonoBehaviour
{
  public RigidHand manoDestra, manoSinistra;
  public float grabAngle = 2.6f;
  private float offsetZoom = 0;

  // Update is called once per frame
  void Update()
  {
    if (manoDestra.IsTracked && manoSinistra.IsTracked)
      if (manoDestra.GetLeapHand().GrabAngle >= grabAngle && manoSinistra.GetLeapHand().GrabAngle >= grabAngle && manoDestra.GetPalmNormal().z / manoSinistra.GetPalmNormal().z < 0 && manoDestra.GetPalmDirection().x < 0 && manoSinistra.GetPalmDirection().x > 0)
      {
        BroadcastMessage("InZoom", true);
        Camera.main.fieldOfView = (offsetZoom + Distanza());
      }
      else
      {
        offsetZoom = Camera.main.fieldOfView - Distanza();
        BroadcastMessage("InZoom", false);
      }
  }

  private float Distanza()
  {
    return (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()).magnitude * 100;
  }

  private Vector3 PuntoMedio()
  {
    return (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()) / 2f;
  }
}