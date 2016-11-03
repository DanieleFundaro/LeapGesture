using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using UnityEditor;

public class Explosion : MonoBehaviour
{
  public RigidHand mano;
  private ObjectsMove om = new ObjectsMove();
  private bool selezione = false, esplodi = false, afferra = false;

  public void OnValidate()
  {
    IHandModel ihm = this.gameObject.GetComponent<IHandModel>();

    if (ihm != null)
      mano = (RigidHand)ihm;
  }

  // Update is called once per frame
  public void Update()
  {
    // L'utente deve fare una cosa per volta, o afferra o fa esplodere
    if (!afferra)
    {
      Controller cont = new Controller();

      if (cont.IsConnected && !esplodi)
      {
        Vector3 dir = mano.GetPalmNormal();
        dir.Normalize();

        // Calcolo il raggio e distanzio il punto di inizio dal palmo della mano di 0.1, così non colpisco le dita della stessa mano
        Ray raggio = new Ray(Vector3.MoveTowards(mano.GetPalmPosition(), dir, 0.1f), dir);
        RaycastHit colpito = new RaycastHit();

        // Disegna la linea per usarla come puntatore, così da facilitare la selezione degli oggetti
        DrawLine(mano.GetPalmPosition(), mano.GetPalmPosition() + dir, Color.green, 0.05f);

        // Controllo se è stato puntato un oggetto
        if (Physics.Raycast(raggio, out colpito))
          if (colpito.collider != null)
          {
            Transform padre = colpito.collider.transform.parent;

            // Deve essere un assemblato di oggetti e non devo considerare, ovviamente, l'altra mano
            if (padre != null)
            {
              RigidHand rh = (RigidHand)padre.GetComponentInParent<IHandModel>();

              if (rh == null)
              {
                om = mano.SelectObjects(padre);
                selezione = true;
              }
            }
          }

        // Controllo del gesto tipico per l'esplosione
        esplodi = selezione && mano.ExplosionGesture();
      }

      // Parte grafica dell'esplosione. Sposto tutti gli oggetti, già selezionati, di una distanza calcolata in precedenza
      if (esplodi)
      {
        esplodi = mano.PlayExplosion(om);
        selezione = false;
      }
    }
  }

  private void StoAfferrando(bool stoAfferrando)
  {
    afferra = stoAfferrando;
  }

  private void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
  {
    GameObject myLine = new GameObject();
    myLine.transform.position = start;
    myLine.AddComponent<LineRenderer>();

    LineRenderer lr = myLine.GetComponent<LineRenderer>();
    lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
    lr.SetColors(color, color);
    lr.SetWidth(0.0025f, 0.0025f);
    lr.SetPosition(0, start);
    lr.SetPosition(1, end);
    GameObject.Destroy(myLine, duration);
  }
}