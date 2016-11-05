using UnityEngine;
using System.Collections;

namespace UnityEngine
{
  public static class MyVector3
  {
    /// <summary>
    /// Controlla se questo vettore è più lungo del vettore b, rispetto a un punto center centrale. Verrà restituito in ogni caso in valore false, se i due vettori non sono paralleli e coincidenti.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b">Vettore da paragonare.</param>
    /// <param name="center">Punto centrale per effettuare il controllo della direzione.</param>
    /// <returns></returns>
    public static bool IsLongerThan(this Vector3 a, Vector3 b, Vector3 center)
    {
      // Controllo se la lunghezza del punto a è maggiore rispetto alla lunghezza del punto b, rispetto a un punto centrale.
      // I 2 punti dovranno condividere la stessa direzione. Se così non è, restituisco il valore false a priori.
      Vector3 p1 = a - center, p2 = b - center;

      return p1.normalized.Equals(p2.normalized) && p1.magnitude > p2.magnitude;
    }
  }
}
