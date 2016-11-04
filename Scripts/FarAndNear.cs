using UnityEngine;
using Leap.Unity;
using System.Collections.Generic;

public class FarAndNear : MonoBehaviour
{
  public RigidHand manoDestra, manoSinistra;
  private List<Vector3> posIniziali = new List<Vector3>();
  private Dictionary<Transform, Vector3> childsDir;
  private Transform padreDestro, padreSinistro;
  private bool selezione = false, sposta = false, afferra = false;
  private float offset = 0, velocita = 10f;

  // Use this for initialization
  void Start()
  {
    posIniziali = new List<Vector3>();
    Transform[] objs = FindObjectsOfType<Transform>();

    foreach (Transform obj in objs)
      posIniziali.Add(new Vector3(obj.position.x, obj.position.y, obj.position.z));

    Init();
  }

  // Update is called once per frame
  void Update()
  {
    if (!afferra)
    {
      DrawLineHand(manoDestra);
      DrawLineHand(manoSinistra);

      if (!selezione)
      {
        padreDestro = GetPadreColpito(manoDestra);
        padreSinistro = GetPadreColpito(manoSinistra);

        if (padreDestro != null)// && padreSinistro != null && padreDestro.Equals(padreSinistro))
        {
          for (int i = 0; i < padreDestro.childCount; i++)
          {
            Transform child = padreDestro.GetChild(i);
            childsDir.Add(child, child.position - padreDestro.position);
          }

          selezione = true;
          offset = (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()).magnitude;
        }
      }

      if (selezione && manoDestra.GetLeapHand().GrabAngle >= 2 && manoSinistra.GetLeapHand().GrabAngle >= 2)
      {
        foreach (KeyValuePair<Transform, Vector3> obj in childsDir)
        {
          float distanza = (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()).magnitude;
          obj.Key.position = padreDestro.position + obj.Value * ((distanza - offset) * velocita + 1);
        }
      }
    }
    else
      Init();
  }

  private void StoAfferrando(bool stoAfferrando)
  {
    afferra = stoAfferrando;
  }

  private void Init()
  {
    selezione = false;
    childsDir = new Dictionary<Transform, Vector3>();
    padreDestro = null;
    padreSinistro = null;
  }

  private Transform GetPadreColpito(RigidHand mano)
  {
    // Se è null vuol dire che la mano non è presente nella scena, quindi è unitile effettuare i calcoli
    if (mano.IsTracked)
    {
      Vector3 dir = mano.GetPalmNormal();

      // Calcolo il raggio e distanzio il punto di inizio dal palmo della mano di 0.1, così non colpisco le dita della stessa mano
      Ray raggio = new Ray(Vector3.MoveTowards(mano.GetPalmPosition(), dir, 0.1f), dir);
      RaycastHit colpito = new RaycastHit();

      if (Physics.Raycast(raggio, out colpito))
        if (colpito.collider != null)
        {
          Transform padre = colpito.collider.transform.parent;

          // Deve essere un assemblato di oggetti e non devo considerare, ovviamente, l'altra mano
          if (padre != null)
          {
            RigidHand rh = (RigidHand)padre.GetComponentInParent<IHandModel>();

            if (rh == null)
              return padre;
          }
        }
    }

    return null;
  }

  private void DrawLineHand(RigidHand mano)
  {
    Vector3 dir = mano.GetPalmNormal();

    // Disegna la linea per usarla come puntatore, così da facilitare la selezione degli oggetti
    if (mano.IsTracked)
    {
      GameObject myLine = new GameObject();
      myLine.transform.position = mano.GetPalmPosition();
      myLine.AddComponent<LineRenderer>();

      LineRenderer lr = myLine.GetComponent<LineRenderer>();
      lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
      lr.SetColors(Color.green, Color.yellow);
      lr.SetWidth(0.0025f, 0.0025f);
      lr.SetPosition(0, mano.GetPalmPosition());
      lr.SetPosition(1, mano.GetPalmPosition() + dir);
      GameObject.Destroy(myLine, 0.05f);
    }
  }
}