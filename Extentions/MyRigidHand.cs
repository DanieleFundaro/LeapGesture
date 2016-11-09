using System.Collections.Generic;
using UnityEngine;

namespace Leap
{
  namespace Unity
  {
    public static class MyRigidHand
    {
      #region Variabili private (valori iniziali e variabili di lavoro)

      private static float minGrab = 0.5f, minPinch = 0.9f, tempo = 0, tempoMax = 0.5f;
      private static string tag = null;
      private static bool esplodi = false;
      private static ObjectsMove om = null;
      private static Color coloreRaggioSelezione = Color.red;
      private static Material selMat = Resources.Load<Material>("SelectedObject");

      #endregion

      #region Start and stop grab

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di presa [0, 1].</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void StartGrab(this RigidHand hand, Collider obj, Transform parent, float min, params string[] tagUntouchable)
      {
        Hand h = hand.GetLeapHand();

        if (h != null && h.GrabStrength >= min && !TagTrovato(obj.transform, tagUntouchable) && (parent != null || parent != hand.GetComponentInParent<IHandModel>()))
          obj.transform.SetParent(hand.palm);
        else
          StopGrab(hand, obj, parent);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di presa [0, 1].</param>
      public static void StartGrab(this RigidHand hand, Collider obj, Transform parent, float min)
      {
        StartGrab(hand, obj, parent, min, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di presa [0, 1].</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void StartGrab(this RigidHand hand, Collider obj, float min, params string[] tagUntouchable)
      {
        StartGrab(hand, obj, null, min, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto.</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void StartGrab(this RigidHand hand, Collider obj, Transform parent, params string[] tagUntouchable)
      {
        StartGrab(hand, obj, parent, minGrab, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di presa [0, 1].</param>
      public static void StartGrab(this RigidHand hand, Collider obj, float min)
      {
        StartGrab(hand, obj, null, min, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto.</param>
      public static void StartGrab(this RigidHand hand, Collider obj, Transform parent)
      {
        StartGrab(hand, obj, parent, minGrab, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void StartGrab(this RigidHand hand, Collider obj, params string[] tagUntouchable)
      {
        StartGrab(hand, obj, null, minGrab, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da afferrare.</param>
      public static void StartGrab(this RigidHand hand, Collider obj)
      {
        StartGrab(hand, obj, null, minGrab, tag);
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

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj. Questo metodo serve per poter spostare e ruotare un assemblato di oggetti.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Genitore che si vuole afferrare per spostarlo e ruotarlo.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di presa [0, 1].</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void StartGrab(this RigidHand hand, Transform obj, float min, params string[] tagUntouchable)
      {
        Hand h = hand.GetLeapHand();

        if (h != null && h.GrabStrength >= min && !TagTrovato(obj, tagUntouchable))
          obj.SetParent(hand.palm);
        else
          StopGrab(hand, obj);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj. Questo metodo serve per poter spostare e ruotare un assemblato di oggetti.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Genitore che si vuole afferrare per spostarlo e ruotarlo.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di presa [0, 1].</param>
      public static void StartGrab(this RigidHand hand, Transform obj, float min)
      {
        StartGrab(hand, obj, min, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj. Questo metodo serve per poter spostare e ruotare un assemblato di oggetti.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Genitore che si vuole afferrare per spostarlo e ruotarlo.</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void StartGrab(this RigidHand hand, Transform obj, string tagUntouchable)
      {
        StartGrab(hand, obj, minGrab, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di presa e afferra l'oggetto obj. Questo metodo serve per poter spostare e ruotare un assemblato di oggetti.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Genitore che si vuole afferrare per spostarlo e ruotarlo.</param>
      public static void StartGrab(this RigidHand hand, Transform obj)
      {
        StartGrab(hand, obj, minGrab, tag);
      }

      /// <summary>
      /// Rilascio definitivo dell'oggetto obj afferrato.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Genitore afferrato.</param>
      public static void StopGrab(this RigidHand hand, Transform obj)
      {
        obj.parent = null;
      }

      #endregion

      #region Pinch

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj, in base alla posizione delle dita.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da pizzicare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto obj.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di pizzico [0, 1].</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void Pinch(this RigidHand hand, Collider obj, Transform fingerBone, Transform parent, float min, params string[] tagUntouchable)
      {
        if (hand.GetLeapHand().PinchStrength >= min && !TagTrovato(obj.transform, tagUntouchable))
          obj.transform.SetParent(fingerBone);
        else
          StopPinch(hand, obj, parent);

        
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj, in base alla posizione delle dita.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da pizzicare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto obj.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di pizzico [0, 1].</param>
      public static void Pinch(this RigidHand hand, Collider obj, Transform fingerBone, Transform parent, float min)
      {
        Pinch(hand, obj, fingerBone, parent, min, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj, in base alla posizione delle dita.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da pizzicare.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto obj.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void Pinch(this RigidHand hand, Collider obj, Transform fingerBone, Transform parent, params string[] tagUntouchable)
      {
        Pinch(hand, obj, fingerBone, parent, minPinch, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj, in base alla posizione delle dita.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da pizzicare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di pizzico [0, 1].</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void Pinch(this RigidHand hand, Collider obj, Transform fingerBone, float min, params string[] tagUntouchable)
      {
        Pinch(hand, obj, fingerBone, null, min, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj, in base alla posizione delle dita.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da pizzicare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="parent">Eventuale genitore a cui appartiene l'oggetto obj.</param>
      public static void Pinch(this RigidHand hand, Collider obj, Transform fingerBone, Transform parent)
      {
        Pinch(hand, obj, fingerBone, parent, minPinch, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj, in base alla posizione delle dita.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da pizzicare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di pizzico [0, 1].</param>
      public static void Pinch(this RigidHand hand, Collider obj, Transform fingerBone, float min)
      {
        Pinch(hand, obj, fingerBone, null, min, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj, in base alla posizione delle dita.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da pizzicare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void Pinch(this RigidHand hand, Collider obj, Transform fingerBone, params string[] tagUntouchable)
      {
        Pinch(hand, obj, fingerBone, null, minPinch, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj, in base alla posizione delle dita.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da pizzicare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      public static void Pinch(this RigidHand hand, Collider obj, Transform fingerBone)
      {
        Pinch(hand, obj, fingerBone, null, minPinch, tag);
      }

      /// <summary>
      /// Rilascio definitivo dell'oggetto obj pizzicato.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto pizzicato.</param>
      /// <param name="parent">Eventuale genitore da assegnare all'oggetto.</param>
      public static void StopPinch(this RigidHand hand, Collider obj, Transform parent)
      {
        obj.transform.SetParent(parent);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj, in base alla posizione delle dita pizzicanti, in direzione radiale rispetto al genitore parent, senza scendere mai sotto la posizione iniziale.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da pizzicare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="parent">Genitore dell'oggetto obj.</param>
      /// <param name="localInitialPositionObj">Posizione iniziale dell'oggetto obj, rispetto al genitore.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di pizzico [0, 1].</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void Pinch(this RigidHand hand, Transform obj, Transform fingerBone, Transform parent, Vector3 localInitialPositionObj, float min, params string[] tagUntouchable)
      {
        if (hand.GetLeapHand().PinchStrength >= min && !TagTrovato(obj, tagUntouchable))
        {
          Vector3 dir = obj.position - fingerBone.position, dirMano = parent.position - fingerBone.position, nuovaPosizione = Vector3.Project(dirMano, dir);

          if (nuovaPosizione.IsLongerThan(localInitialPositionObj, dir))
            obj.position = Vector3.MoveTowards(obj.position, fingerBone.position + nuovaPosizione, nuovaPosizione.magnitude);
        }
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj in direzione radiale rispetto al genitore parent, senza scendere mai sotto la posizione iniziale.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da spostare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="parent">Genitore dell'oggetto obj.</param>
      /// <param name="localInitialPositionObj">Posizione iniziale dell'oggetto obj, rispetto al genitore.</param>
      /// <param name="min">Valore minimo per cui si può considerare valido il gesto di pizzico [0, 1].</param>
      public static void Pinch(this RigidHand hand, Transform obj, Transform fingerBone, Transform parent, Vector3 localInitialPositionObj, float min)
      {
        Pinch(hand, obj, parent, fingerBone, localInitialPositionObj, min, null);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj in direzione radiale rispetto al genitore parent, senza scendere mai sotto la posizione iniziale.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da spostare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="parent">Genitore dell'oggetto obj.</param>
      /// <param name="localInitialPositionObj">Posizione iniziale dell'oggetto obj, rispetto al genitore.</param>
      /// <param name="tagUntouchable">Tag appartenenti agli oggetti da ignorare (null se tutti possono essere presi).</param>
      public static void Pinch(this RigidHand hand, Transform obj, Transform fingerBone, Transform parent, Vector3 localInitialPositionObj, params string[] tagUntouchable)
      {
        Pinch(hand, obj, parent, fingerBone, localInitialPositionObj, minPinch, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di pizzico e sposta l'oggetto obj in direzione radiale rispetto al genitore parent, senza scendere mai sotto la posizione iniziale.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="obj">Oggetto da spostare.</param>
      /// <param name="fingerBone">Dito che effettua il pinch dell'oggetto obj.</param>
      /// <param name="parent">Genitore dell'oggetto obj.</param>
      /// <param name="localInitialPositionObj">Posizione iniziale dell'oggetto obj, rispetto al genitore.</param>
      public static void Pinch(this RigidHand hand, Transform obj, Transform fingerBone, Transform parent, Vector3 localInitialPositionObj)
      {
        Pinch(hand, obj, parent, fingerBone, localInitialPositionObj, minPinch, null);
      }

      #endregion

      #region Explosion effect

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// <param name="hand"></param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="colorRay">Colore del raggio.</param>
      /// <param name="tagUntouchable">Lista di tag appartenente agli oggetti da ignorare.</param>
      public static void Explosion(this RigidHand hand, Ray raySelection, Color colorRay, params string[] tagUntouchable)
      {
        if (!esplodi)
        {
          RaycastHit colpito = new RaycastHit();

          // Disegna la linea per usarla come puntatore, così da facilitare la selezione degli oggetti
          Utility.DrawLine(raySelection.origin, raySelection.origin + raySelection.direction, colorRay, 0.05f);

          // Controllo se è stato puntato un oggetto
          if (Physics.Raycast(raySelection, out colpito))
            if (colpito.collider != null)
            {
              Transform padre = colpito.collider.transform.parent;

              // Deve essere un assemblato di oggetti e non devo considerare, ovviamente, l'altra mano o gli oggetti da ignorare
              if (padre != null && !TagTrovato(padre, tagUntouchable))
              {
                RigidHand rh = (RigidHand)padre.GetComponentInParent<IHandModel>();

                if (rh == null)
                  om = hand.SelectObjects(padre);
              }
            }

          // Controllo del gesto tipico per l'esplosione
          esplodi = om != null && hand.ExplosionGesture();
        }

        // Parte grafica dell'esplosione. Sposto tutti gli oggetti, già selezionati, di una distanza calcolata in precedenza
        if (esplodi)
          esplodi = hand.PlayExplosion(om);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, selezionati dal raggio raySelection.
      /// <param name="hand"></param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="colorRay">Colore del raggio.</param>
      public static void Explosion(this RigidHand hand, Ray raySelection, Color colorRay)
      {
        Explosion(hand, raySelection, colorRay, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// <param name="hand"></param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="tagUntouchable">Lista di tag appartenente agli oggetti da ignorare.</param>
      public static void Explosion(this RigidHand hand, Ray raySelection, params string[] tagUntouchable)
      {
        Explosion(hand, raySelection, coloreRaggioSelezione, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, selezionati dal raggio raySelection.
      /// <param name="hand"></param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      public static void Explosion(this RigidHand hand, Ray raySelection)
      {
        Explosion(hand, raySelection, coloreRaggioSelezione, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, selezionati dal raggio con origine sul palmo della mano e con direzione il vettore normale del palmo.
      /// <param name="hand"></param>
      public static void Explosion(this RigidHand hand)
      {
        Vector3 dir = hand.GetPalmNormal();
        dir.Normalize();

        // Calcolo il raggio e distanzio il punto di inizio dal palmo della mano di 0.1, così non colpisco le dita della stessa mano
        Ray raggio = new Ray(Vector3.MoveTowards(hand.GetPalmPosition(), dir, 0.07f), dir);

        Explosion(hand, raggio, coloreRaggioSelezione, tag);
      }

      /// <summary>
      /// Rende gli oggetti, figli di parent, selezionati, quindi pronti per essere esplosi, entro il tempo temp.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="parent">Oggetto padre che si vuole far esplodere.</param>
      /// <param name="temp">Tempo massimo a disposizione per poter effettuare l'esplosione, superato il quale bisogna rendere, di nuovo, gli oggetti selezinati.</param>
      /// <returns></returns>
      public static ObjectsMove SelectObjects(this RigidHand hand, Transform parent, float temp = 3)
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
          {
            figlio.gameObject.AddComponent<ParticleSystem>();
            ParticleSystem psF = figlio.GetComponent<ParticleSystem>();
            psF.startColor = selMat.color;
            psF.gravityModifier = -0.05f;
          }

          om.Add(new ObjectMove(figlio, startC, startC + v));
        }

        tempo = 0;

        // Effettuo la distruzione delle particelle che sono state aggiunge agli ogetti (se ce ne sono).
        // Se supero il tempo, dovrò ripetere l'operazione
        ParticleSystem[] ps1 = Object.FindObjectsOfType<ParticleSystem>();

        foreach (ParticleSystem p in ps1)
          Object.Destroy(p, temp);

        return om;
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

      #endregion

      #region Metodi privati
      
      private static bool TagTrovato(Transform obj, string[] listaTag)
      {
        if (listaTag != null)
          for (int i = 0; i < listaTag.Length; i++)
            if (obj.tag == listaTag[i])
              return true;

        return false;
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