using System.Collections.Generic;
using UnityEngine;

namespace Leap
{
  namespace Unity
  {
    public static class MyRigidHand
    {
      #region Start and stop grab

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di presa.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto.</param>
      /// <param name="tagUntouchable">Tag appartenente agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void StartGrab(this RigidHand hand, Collider obj, float min, Transform parent, string tagUntouchable)
      {
        Hand h = hand.GetLeapHand();

        if (h != null && h.GrabStrength >= min && obj.tag != tagUntouchable && (parent != null || parent != hand.GetComponentInParent<IHandModel>()))
          obj.transform.SetParent(hand.palm);
        else
          StopGrab(hand, obj, parent);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di presa.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto.</param>
      public static void StartGrab(this RigidHand hand, Collider obj, float min, Transform parent)
      {
        StartGrab(hand, obj, min, parent, null);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di presa.</param>
      /// <param name="tagUntouchable">Tag appartenente agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void StartGrab(this RigidHand hand, Collider obj, float min, string tagUntouchable)
      {
        StartGrab(hand, obj, min, null, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto.</param>
      /// <param name="tagUntouchable">Tag appartenente agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void StartGrab(this RigidHand hand, Collider obj, Transform parent, string tagUntouchable)
      {
        StartGrab(hand, obj, 0.5f, parent, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di presa.</param>
      public static void StartGrab(this RigidHand hand, Collider obj, float min)
      {
        StartGrab(hand, obj, min, null, null);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto.</param>
      public static void StartGrab(this RigidHand hand, Collider obj, Transform parent)
      {
        StartGrab(hand, obj, 0.5f, parent, null);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="tagUntouchable">Tag appartenente agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void StartGrab(this RigidHand hand, Collider obj, string tagUntouchable)
      {
        StartGrab(hand, obj, 0.5f, null, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      public static void StartGrab(this RigidHand hand, Collider obj)
      {
        StartGrab(hand, obj, 0.5f, null, null);
      }

      /// <summary>
      /// Rilascio definitivo dell'oggetto obj afferrato.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto afferrato.</param>
      /// <param name="parent">Eventuale genitore da assegnare all'oggetto.</param>
      public static void StopGrab(this RigidHand hand, Collider obj, Transform parent)
      {
        obj.transform.parent = parent;
      }

      /// <summary>
      /// Rilascio definitivo dell'oggetto obj afferrato.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto afferrato.</param>
      public static void StopGrab(this RigidHand hand, Collider obj)
      {
        StopGrab(hand, obj, null);
      }

      #endregion

      #region Explosion effect
      
      private static float tempo = 0, tempoMax = 0.5f;

      /// <summary>
      /// Rende gli oggetti, figli di parent, selezionati, quindi pronti per essere esplosi, entro il tempo temp.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="parent">Oggetto padre che si vuole far esplodere.</param>
      /// <param name="temp">Tempo massimo a disposizione per poter effettuare l'esplosione, superato il quale bisogna rendere, di nuovo, gli oggetti selezinati.</param>
      public static ObjectsMove SelectObjects(this RigidHand hand, Transform parent, float temp)
      {
        ObjectsMove om = new ObjectsMove();
        int count = parent.childCount;

        // Per tutti i figli calcolo il punto iniziale e finale della traiettoria di esplosione
        for (int i = 0; i < count; i++)
        {
          Transform figlio = parent.GetChild(i);
          Vector3 startP = parent.position, startC = figlio.position;
          Vector3 v = startC - startP;

          // Rendo l'oggetto colpito "esplodibile", se già non è stato fatto in precedenza
          if (figlio.GetComponent<ParticleSystem>() == null)
            figlio.gameObject.AddComponent<ParticleSystem>();

          om.Add(new ObjectMove(figlio, startC, startC + v));
        }

        tempo = 0;

        // Effettuo la distruzione delle particelle che sono state aggiunge agli ogetti (se ce ne sono).
        // Se supero il tempo, dovrò ripetere l'operazione
        ParticleSystem[] ps1 = SceneSettings.FindObjectsOfType<ParticleSystem>();

        foreach (ParticleSystem p in ps1)
          UnityEngine.Object.Destroy(p, temp);

        return om;
      }

      /// <summary>
      /// Rende gli oggetti, figli di parent, selezionati, quindi pronti per essere esplosi, entro 3 secondi di tempo.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="parent">Oggetto padre che si vuole far esplodere.</param>
      public static ObjectsMove SelectObjects(this RigidHand hand, Transform parent)
      {
        return SelectObjects(hand, parent, 3);
      }

      /// <summary>
      /// Gesto di esplosione. Restituisce true se è stato effettuato correttamente il tipico gesto di esplosione/apertura di un assemblato di oggetti, false altrimenti.
      /// </summary>
      /// <param name="hand"></param>
      /// <returns></returns>
      public static bool ExplosionGesture(this RigidHand hand)
      {
        return Gesture(hand, 0, 1);
      }

      /// <summary>
      /// Gesto di implosione. Restituisce true se è stato effettuato correttamente il tipico gesto di implosione/chiusura di un assemblato di oggetti, false altrimenti.
      /// </summary>
      /// <param name="hand"></param>
      /// <returns></returns>
      public static bool ImplosionGesture(this RigidHand hand)
      {
        return Gesture(hand, 1, 0);
      }

      /// <summary>
      /// Grafica dell'esplosione. Restituisce il valore true finchè il movimento degli oggetti è attivo, restituisce il valore false una volta arrivati a destinazione.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="items">Collezione degli oggetti che si vuole spostare.</param>
      /// <returns></returns>
      public static bool PlayExplosion(this RigidHand hand, ObjectsMove items)
      {
        tempo += Time.deltaTime;
        float percento = tempo > tempoMax ? 1 : Mathf.Pow(tempo / tempoMax, 1f / 3f);

        for (int i = 0; i < items.Count; i++)
          items.GetChild(i).position = Vector3.Lerp(items.GetStartOfChild(i), items.GetEndOfChild(i), percento);

        return percento != 1;
      }

      private static bool Gesture(RigidHand hand, float grabStrengthCorrente, float grabStrengthPassato)
      {
        Controller c = new Controller();
        Frame framePassato = c.Frame(4);
        Frame frameCorrente = c.Frame();
        Hand manoPassata = framePassato.Hand(hand.LeapID()), manoCorrente = frameCorrente.Hand(hand.LeapID());

        return manoCorrente != null && manoPassata != null && manoCorrente.GrabStrength == grabStrengthCorrente && manoPassata.GrabStrength == grabStrengthPassato && SceneSettings.FindObjectsOfType<ParticleSystem>() != null;
      }

      #endregion
    }

    public class ObjectMove
    {
      private Transform obj;
      private Vector3 start, end;

      public Transform Obj { get { return obj; } set { obj = value; } }
      public Vector3 Start { get { return start; } set { start = value; } }
      public Vector3 End { get { return end; } set { end = value; } }

      public ObjectMove(Transform obj, Vector3 start, Vector3 end)
      {
        this.obj = obj;
        this.start = start;
        this.end = end;
      }
    }

    public class ObjectsMove
    {
      private List<ObjectMove> lista;

      public int Count { get { return lista.Count; } }

      public ObjectsMove() { lista = new List<ObjectMove>(); }

      public Transform GetChild(int index) { return lista[index].Obj; }

      public Vector3 GetStartOfChild(int index) { return lista[index].Start; }

      public Vector3 GetEndOfChild(int index) { return lista[index].End; }

      public void SetEndOfChild(int index, Vector3 newEnd) { lista[index].End = newEnd; }

      public void Add(ObjectMove item) { lista.Add(item); }

      public void RemoveAll() { lista.RemoveRange(0, lista.Count); }
    }
  }
}