using UnityEngine;
using System.Collections;
using Leap;
using Leap.Unity;

public class ActivateComandi : MonoBehaviour
{
  public RigidHand mano;
  public GameObject[] comandi;

  // Update is called once per frame
  void Update()
  {
    Controller c = new Controller();

    if (mano.IsTracked)
    {
      Hand h = mano.GetLeapHand();

      for (int i = 0; i < comandi.Length; i++)
        comandi[i].SetActive(h.GrabStrength >= 0 && h.GrabStrength <= 0.1 && mano.GetPalmNormal().z < 0 && mano.GetPalmDirection().y > 0);
    }
  }
}

