using UnityEngine;
using Leap.Unity;
using System.Collections.Generic;

public class FarAndNear : MonoBehaviour
{
  public RigidHand manoDestra, manoSinistra;
  private bool afferraZoom = false, spostamento = false;
  private Dictionary<Transform, Vector3> childsDir;
  private float offset = 0, velocita = 10f;

  void Start()
  {
    Init();
  }

  // Update is called once per frame
  void Update()
  {
    if (!afferraZoom && manoDestra.IsTracked && manoSinistra.IsTracked)
    {
      if (!spostamento)
      {
        MeshRenderer[] collMesh = FindObjectsOfType<MeshRenderer>();

        foreach (MeshRenderer mr in collMesh)
        {
          Transform t = mr.transform;

          if (t.tag != "Imprendibile" && t.tag != "MainCamera")
          {
            Transform padre = Utility.GetPrimoPadre(t);
            Vector3 dir = t.position - padre.position;
            childsDir.Add(t, dir);
          }
        }

        offset = (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()).magnitude;
        spostamento = true;
      }

      // Effettuo lo spostamento degli oggetti, controllando se è stato compiuto il giusto gesto (tutte e 2 le mani chiuse a pugno)
      if (spostamento && manoDestra.GetLeapHand().GrabAngle >= 2 && manoSinistra.GetLeapHand().GrabAngle >= 2)
      {
        float distanza = (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()).magnitude;
        Dictionary<Transform, Vector3> posIniziali = InitialPosition.Direzioni;

        foreach (KeyValuePair<Transform, Vector3> obj in childsDir)
        {
          Vector3 nuovaPosizione = obj.Value * ((distanza - offset) * velocita + 1);

          // Non permetto di scendere al di sotto del minimo della posizione di partenza, evitando quindi di far collassare tutto al centro.
          if (nuovaPosizione.IsLongerThan(posIniziali[obj.Key], obj.Value))
            obj.Key.position = Utility.GetPrimoPadre(obj.Key).position + nuovaPosizione;
        }
      }
      else
        Init();
    }
  }

  private void Init()
  {
    spostamento = false;
    childsDir = new Dictionary<Transform, Vector3>();
  }

  private void StoAfferrando(bool stoAfferrando)
  {
    afferraZoom = stoAfferrando;
  }

  private void InZoom(bool zoom)
  {
    afferraZoom = zoom;
  }
}