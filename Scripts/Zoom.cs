using UnityEngine;
using Leap.Unity;
using Leap;

public class Zoom : MonoBehaviour
{
  public RigidHand manoDestra, manoSinistra;
  public float grabAngleMin = 2.6f, grabAngleMax = 2.8f, velocità = 10f;
  private float offset = 0, offset1 = 0;

  // Update is called once per frame
  void Update()
  {
    if (manoDestra.IsTracked && manoSinistra.IsTracked)
    {
      Hand r = manoDestra.GetLeapHand(), l = manoSinistra.GetLeapHand();
      Vector3 rn = manoDestra.GetPalmNormal(), rd = manoDestra.GetPalmDirection(), ln = manoSinistra.GetPalmNormal(), ld = manoSinistra.GetPalmDirection();

      if (r.GrabAngle >= grabAngleMin && r.GrabAngle <= grabAngleMax && l.GrabAngle >= grabAngleMin && l.GrabAngle <= grabAngleMax && rn.z / ln.z < 0 && rd.x < 0 && ld.x > 0)
      {
        BroadcastMessage("InZoom", true);
        Camera.main.fieldOfView = (Distanza() - offset);
      }
      else if (r.GrabStrength == 0 && l.GrabStrength == 0 && rn.z < 0 && ln.z < 0 && rd.y > 0 && ld.y > 0)
      {
        BroadcastMessage("InZoom", true);
        Camera.main.fieldOfView = Distanza1() - offset1;
      }
      else
      {
        offset = Distanza() - Camera.main.fieldOfView;
        offset1 = Distanza1() - Camera.main.fieldOfView;
        BroadcastMessage("InZoom", false);
      }
    }
  }

  private float Distanza()
  {
    return (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()).magnitude * velocità;
  }

  private float Distanza1()
  {
    float offset = Mathf.Abs(manoDestra.GetPalmPosition().z) > Mathf.Abs(manoSinistra.GetPalmPosition().z) ? manoDestra.GetPalmPosition().z : manoSinistra.GetPalmPosition().z;
    return offset * velocità;
  }
}