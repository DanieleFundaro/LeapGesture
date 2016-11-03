using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;

public class ActivateComandi : MonoBehaviour
{
  public RigidHand mano;
  public GameObject[] comandi;

  public void OnValidate()
  {
    RigidHand rh = (RigidHand)this.GetComponent<IHandModel>();

    if (rh != null)
      mano = rh;
  }

  // Update is called once per frame
  void Update()
  {
    Controller c = new Controller();

    if (c.IsConnected)
    {
      Hand h = c.Frame().Hand(mano.LeapID());

      if (h != null)
        for (int i = 0; i < comandi.Length; i++)
          comandi[i].SetActive(h.GrabStrength >= 0 && h.GrabStrength <= 0.1 && mano.GetPalmNormal().z < 0 && mano.GetPalmDirection().y > 0);
    }
  }
}

