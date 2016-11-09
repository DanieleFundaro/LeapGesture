using Leap.Unity;
using UnityEngine;

public class Utility
{
  public static void Enable<T>(Transform obj, Collider other, bool startSetting = true) where T : Behaviour
  {
    T b = obj.GetComponentInParent<T>();

    if (b != null)
    {
      if (startSetting)
        if (other.GetComponentInParent<IHandModel>() != null)
          b.enabled = !b.enabled;

      MeshRenderer[] mesh = obj.GetComponentsInChildren<MeshRenderer>();

      foreach (MeshRenderer mr in mesh)
      {
        Color c = mr.material.color;
        c.a = b.enabled ? 255 : 0;
        mr.material.color = c;
      }
    }
  }

  public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
  {
    GameObject myLine = new GameObject();
    myLine.transform.position = start;
    myLine.AddComponent<LineRenderer>();

    LineRenderer lr = myLine.GetComponent<LineRenderer>();
    lr.material = new Material(Shader.Find("Custom/LineShader"));
    lr.material.color = color;
    lr.SetWidth(0.0025f, 0.0025f);
    lr.SetPosition(0, start);
    lr.SetPosition(1, end);
    GameObject.Destroy(myLine, duration);
  }
}