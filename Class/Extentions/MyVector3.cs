using UnityEngine;
using System.Collections;

namespace UnityEngine
{
  public static class MyVector3
  {
    /// <summary>
    /// Controlla se questo vettore è più lungo del vettore b, rispetto una direzione data dir. Verrà restituito in ogni caso in valore false, se questo vettore e la direzione non sono paralleli e coincidenti.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b">Vettore da paragonare.</param>
    /// <returns></returns>
    public static bool IsLongerThan(this Vector3 a, Vector3 b, Vector3 dir)
    {
      return a.normalized.Equals(dir.normalized) && a.magnitude >= b.magnitude;
    }
  }
}
