using UnityEngine;
using Leap.Unity;
using System.Collections.Generic;

public class FarAndNear : MonoBehaviour
{
  public RigidHand manoDestra, manoSinistra;
  private Dictionary<Transform, Vector3> childsDir;
  private Transform padreDestro, padreSinistro;
  private bool selezione = false, afferraZoom = false;
  private float offset = 0, velocita = 10f;

  // Use this for initialization
  void Start()
  {
    Init();
  }

  // Update is called once per frame
  void Update()
  {
    if (!afferraZoom)
    {
      DrawLineHand(manoDestra);
      DrawLineHand(manoSinistra);

      if (!selezione)
      {
        padreDestro = GetPadreColpito(manoDestra);
        padreSinistro = GetPadreColpito(manoSinistra);

        // Controllo se sia la mano destra che la mano sinistra stanno puntando sullo stesso assemblato di oggetti. In caso affermativo posso iniziare la fare di spostamento radiale dei figli
        if (padreDestro != null && padreSinistro != null && padreDestro.Equals(padreSinistro) && padreDestro.tag != "Imprendibile" && padreDestro.tag != "MainCamera")
        {
          // Mi salvo a parte tutti i figli del padre e mi calcolo la loro direzione radiale
          for (int i = 0; i < padreDestro.childCount; i++)
          {
            Transform child = padreDestro.GetChild(i);
            childsDir.Add(child, child.position - padreDestro.position);
          }

          selezione = true;
          offset = (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()).magnitude;
        }
      }

      // Effettuo lo spostamento degli oggetti, controllando se è stato compiuto il giusto gesto (tutte e 2 le mani chiuse a pugno)
      if (selezione && manoDestra.IsTracked && manoSinistra.IsTracked && manoDestra.GetLeapHand().GrabAngle >= 2 && manoSinistra.GetLeapHand().GrabAngle >= 2)
      {
        float distanza = (manoDestra.GetPalmPosition() - manoSinistra.GetPalmPosition()).magnitude;
        Dictionary<Transform, Vector3> posIniziali = InitialPosition.Posizioni;

        foreach (KeyValuePair<Transform, Vector3> obj in childsDir)
        {
          Vector3 nuovaPosizione = obj.Value * ((distanza - offset) * velocita + 1);

          // Non permetto di scendere al di sotto del minimo della posizione di partenza, evitando quindi di far collassare tutto al centro.
          if (nuovaPosizione.IsLongerThan(posIniziali[obj.Key], obj.Value))
            obj.Key.position = padreDestro.position + nuovaPosizione;
        }
      }
      else
        Init();
    }
    else
      Init();
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
      Utility.DrawLine(mano.GetPalmPosition(), mano.GetPalmPosition() + dir, Color.green, 0.05f);
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