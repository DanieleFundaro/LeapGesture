using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using Leap;

public class Implosion : MonoBehaviour
{
  public RigidHand mano;
  private bool afferraZoom = false;
  private Dictionary<Transform, Vector3> localPosIniziali;

  // Use this for initialization
  void Start()
  {
    localPosIniziali = Utility.CalcoloPositionTransform();
  }

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
        FingerModel dito = mano.fingers[1];
        Ray raggio = new Ray(dito.GetBoneCenter(3), dito.GetBoneDirection(3));

        mano.Implosion(localPosIniziali, raggio, Color.blue, "Imprendibile", "MainCamera");
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