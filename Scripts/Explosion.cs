using UnityEngine;
using Leap;
using Leap.Unity;
using System;
using UnityEditor;

public class Explosion : MonoBehaviour
{
  public RigidHand mano;
  private bool afferraZoom = false;

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
    if (!afferraZoom)
    {
      Controller cont = new Controller();

      if (cont.IsConnected)
      {
        Vector3 dir = mano.GetPalmNormal();
        dir.Normalize();
        Ray raggio = new Ray(Vector3.MoveTowards(mano.GetPalmPosition(), dir, 0.07f), dir);

        mano.Explosion(raggio, "Imprendibile", "MainCamera");
      }
    }
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