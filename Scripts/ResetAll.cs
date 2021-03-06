﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;

public class ResetAll : MonoBehaviour
{
  private Dictionary<Transform, Vector3> posIniziali;
  private Dictionary<Transform, Quaternion> rotIniziali;
  private Transform[] objsInScene;
  private float tempo = 0, tempoMax = 1.0f, fieldOfView;
  private bool start = false;

  // Use this for initialization
  void Start()
  {
    // Calcolo le posizioni iniziali di tutti gli oggetti presenti nella scena. All'occorrenza utilizzerò questi valori per effettuare qualche controllo
    posIniziali = new Dictionary<Transform, Vector3>();
    rotIniziali = new Dictionary<Transform, Quaternion>();
    Transform[] objs = FindObjectsOfType<Transform>();

    foreach (Transform obj in objs)
    {
      posIniziali.Add(obj, new Vector3(obj.position.x, obj.position.y, obj.position.z));
      rotIniziali.Add(obj, new Quaternion(obj.rotation.x, obj.rotation.y, obj.rotation.z, obj.rotation.w));
    }

    fieldOfView = Camera.main.fieldOfView;
  }

  public void OnTriggerEnter(Collider other)
  {
    if (other.GetComponentInParent<IHandModel>() != null && tempo == 0 && other.tag != "Imprendibile")
    {
      objsInScene = FindObjectsOfType<Transform>();
      transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
      Camera.main.fieldOfView = fieldOfView;
      start = true;
    }
  }

  public void Update()
  {
    if (start)
    {
      tempo += Time.deltaTime;
      float percento = tempo > tempoMax ? 1 : tempo / tempoMax;

      foreach (Transform obj in objsInScene)
        if (obj != null && obj.tag != "Imprendibile" && obj.GetComponentInParent<IHandModel>() == null || obj.Equals(transform))
          try
          {
            obj.position = Vector3.Lerp(obj.position, posIniziali[obj], percento);
            obj.rotation = Quaternion.Slerp(obj.rotation, rotIniziali[obj], percento);
          }
          catch (KeyNotFoundException)
          {
            ;
          }

      if (percento == 1)
      {
        start = false;
        tempo = 0;
      }
    }
  }
}
