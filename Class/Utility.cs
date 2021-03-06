﻿using Leap.Unity;
using UnityEngine;

public class Utility
{
  public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
  {
    GameObject myLine = new GameObject("Line");
    myLine.transform.position = start;
    myLine.AddComponent<LineRenderer>();

    LineRenderer lr = myLine.GetComponent<LineRenderer>();
    lr.material = Resources.Load<Material>("LineMaterial");
    lr.material.color = color;
    lr.SetWidth(0.025f, 0.025f);
    lr.SetPosition(0, start);
    lr.SetPosition(1, end);
    GameObject.Destroy(myLine, duration);
  }

  public static void Enable<T>(Transform obj, Collider other, bool triggerSetting = true) where T : Behaviour
  {
    T b = obj.GetComponentInParent<T>();

    if (b != null)
    {
      if (triggerSetting)
        if (other.GetComponentInParent<FingerModel>() != null)
          b.enabled = !b.enabled;

      MeshRenderer[] mesh = obj.GetComponentsInChildren<MeshRenderer>();

      foreach (MeshRenderer mr in mesh)
      {
        Color c = mr.material.color;
        c.a = b.enabled ? 1 : 0;
        mr.material.color = c;
      }
    }
  }
  
  public static Transform GetPrimoPadre(Transform other)
  {
    if (other == null)
      return null;

    if (other.parent == null)
      return other;

    return GetPrimoPadre(other.parent);
  }

  public static bool TagDaEvitare(Transform obj)
  {
    return obj.tag != "Imprendibile" && obj.tag != "MainCamera";
  }
}