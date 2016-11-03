using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;

public class Afferra : MonoBehaviour
{
  public HandModel mano;
  public float minGrab = 0.1f;
  private Vector3 offset, normale;
  private Collider colliso;

  void Start()
  {
    offset = Vector3.zero;
    colliso = null;
  }

  void Update()
  {
    if (offset != Vector3.zero && colliso != null)
    {
      Controller c = new Controller();
      Frame f = c.Frame();
      Hand h = f.Hand(mano.LeapID());  // Prendo la mano 0 perchè per ora lo script è attaccato nelle singole dita di ogni mano, quindi la mano da considerare è sempre la prima dell'array

      if (h.GrabStrength >= minGrab)
      {
        // Effettuo lo spostamento dell'oggetto, controllando continuamente dove si trova il palmo della mano, mantenendo sempre la stessa distanza (calcolata già prima) da quest'ultimo
        colliso.transform.position = (mano.palm.position + offset);

        // Effettuo la rotazione dell'oggetto, calcolando il nuovo vettore della normale del palmo della mano
        Vector3 nuovaNormale = h.PalmNormal.ToVector3();
        nuovaNormale.z *= -1.0f;    // Moltiplico per -1.0f perchè l'asse z della Leap Motion ha il verso positivo che punta sull'utente e non sullo schermo
        colliso.transform.rotation = Quaternion.FromToRotation(normale, nuovaNormale);
      }
      else
        Rilascio();
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    Controller c = new Controller();
    Frame f = c.Frame();
    Hand h = f.Hand(mano.LeapID());

    if (h.GrabStrength >= minGrab)
    {
      // Intercettazione dell'oggetto colliso e calcolo della distanza dal palmo della mano. Durante lo spostamento questa distanza deve restare invariata
      colliso = collision.collider;
      AbilitaDisabilitaSpostamento(true);
      offset = colliso.transform.position - mano.palm.position;

      // Calcolo il vettore normale del palmo della mano per poter effettuare successivamente la rotazione dell'oggetto
      normale = h.PalmNormal.ToVector3();
      normale.z *= -1.0f;           // Moltiplico per -1.0f perchè l'asse z della Leap Motion ha il verso positivo che punta sull'utente e non sullo schermo
    }
  }

  private void Rilascio()
  {
    offset = Vector3.zero;
    AbilitaDisabilitaSpostamento(false);
  }

  private void AbilitaDisabilitaSpostamento(bool abilitato)
  {
    Rigidbody corpo = colliso.GetComponent<Rigidbody>();

    if (corpo != null)
    {
      corpo.isKinematic = abilitato;
      corpo.useGravity = !abilitato;
      //corpo.constraints = abilitato ? RigidbodyConstraints.FreezeAll : RigidbodyConstraints.None;
      colliso.GetComponent<MeshCollider>().isTrigger = abilitato;
      colliso.enabled = !abilitato;
    }
  }
}