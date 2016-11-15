using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Leap
{
  namespace Unity
  {
    public static class MyRigidHand
    {
      #region Variabili private (valori iniziali e variabili di lavoro)

      private static float minGrab = 0.5f, minPinch = 0.9f, tempo = 0, tempoMax = 0.5f;
      private static string tag = null, testoExplosion = "Explosion ready.", testoImplosion = "Implosion ready.";
      private static bool esplodi = false, selezioneOggetti = false, implodi = false;
      private static ObjectsMove objM = null;
      private static Color coloreRaggioSelezione = Color.red;
      private static Material selezione = Resources.Load<Material>("SelectedObject");

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
          Vector3 dir = obj.position - parent.position, dirMano = fingerBone.position - parent.position, nuovaPosizione = Vector3.Project(dirMano, dir);

          if (nuovaPosizione.IsLongerThan(localInitialPositionObj, dir))
            obj.position = Vector3.MoveTowards(obj.position, parent.position + nuovaPosizione, nuovaPosizione.magnitude);
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
        Pinch(hand, obj, fingerBone, parent, localInitialPositionObj, min, null);
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
        Pinch(hand, obj, fingerBone, parent, localInitialPositionObj, minPinch, tagUntouchable);
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
        Pinch(hand, obj, fingerBone, parent, localInitialPositionObj, minPinch, null);
      }

      #endregion

      #region Explosion and implosion effect

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// <param name="hand"></param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="colorRay">Colore del raggio.</param>
      /// <param name="tagUntouchable">Lista di tag appartenente agli oggetti da ignorare.</param>
      public static void Explosion(this RigidHand hand, string message, Ray raySelection, Color colorRay, params string[] tagUntouchable)
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
                  objM = hand.SelectObjects(padre, message);
              }
            }

          // Controllo del gesto tipico per l'esplosione
          esplodi = objM != null && hand.ExplosionGesture(message);
        }

        // Parte grafica dell'esplosione. Sposto tutti gli oggetti, già selezionati, di una distanza calcolata in precedenza
        if (esplodi)
          esplodi = hand.Play(objM);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// <param name="hand"></param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="colorRay">Colore del raggio.</param>
      public static void Explosion(this RigidHand hand, string message, Ray raySelection, Color colorRay)
      {
        Explosion(hand, message, raySelection, colorRay, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// <param name="hand"></param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      public static void Explosion(this RigidHand hand, string message, Ray raySelection)
      {
        Explosion(hand, message, raySelection, coloreRaggioSelezione, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, selezionati dal raggio raySelection.
      /// <param name="hand"></param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="colorRay">Colore del raggio.</param>
      public static void Explosion(this RigidHand hand, Ray raySelection, Color colorRay)
      {
        Explosion(hand, testoExplosion, raySelection, colorRay, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// <param name="hand"></param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="tagUntouchable">Lista di tag appartenente agli oggetti da ignorare.</param>
      public static void Explosion(this RigidHand hand, Ray raySelection, params string[] tagUntouchable)
      {
        Explosion(hand, testoExplosion, raySelection, coloreRaggioSelezione, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, selezionati dal raggio con origine sul palmo della mano e con direzione il vettore normale del palmo.
      /// <param name="hand"></param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      public static void Explosion(this RigidHand hand, string message)
      {
        Vector3 dir = hand.GetPalmNormal();
        dir.Normalize();

        // Calcolo il raggio e distanzio il punto di inizio dal palmo della mano di 0.1, così non colpisco le dita della stessa mano
        Ray raggio = new Ray(Vector3.MoveTowards(hand.GetPalmPosition(), dir, 0.07f), dir);

        Explosion(hand, message, raggio, coloreRaggioSelezione, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, selezionati dal raggio raySelection.
      /// <param name="hand"></param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      public static void Explosion(this RigidHand hand, Ray raySelection)
      {
        Explosion(hand, testoExplosion, raySelection, coloreRaggioSelezione, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di esplosione e fa esplodere gli oggetti, selezionati dal raggio con origine sul palmo della mano e con direzione il vettore normale del palmo.
      /// <param name="hand"></param>
      public static void Explosion(this RigidHand hand)
      {
        Explosion(hand, testoExplosion);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di implosione e fa implodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="initialPosition">Posizioni iniziali degli oggetti selezionati.</param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="colorRay">>Colore del raggio.</param>
      /// <param name="tagUntouchable">Lista di tag appartenente agli oggetti da ignorare.</param>
      public static void Implosion(this RigidHand hand, Dictionary<Transform, Vector3> initialPosition, string message, Ray raySelection, Color colorRay, params string[] tagUntouchable)
      {
        if (!implodi)
        {
          RaycastHit colpito = new RaycastHit();

          // Disegna la linea per usarla come puntatore, così da facilitare la selezione degli oggetti
          Utility.DrawLine(raySelection.origin, raySelection.origin + raySelection.direction, colorRay, 0.05f);

          // Controllo se è stato puntato un oggetto
          if (Physics.Raycast(raySelection, out colpito))
          {
            if (colpito.collider != null)
            {
              Transform padre = colpito.collider.transform.parent;

              // Deve essere un assemblato di oggetti e non devo considerare, ovviamente, l'altra mano o gli oggetti da ignorare
              if (padre != null && !TagTrovato(padre, tagUntouchable))
              {
                RigidHand rh = (RigidHand)padre.GetComponentInParent<IHandModel>();

                if (rh == null)
                {
                  objM = hand.SelectObjects(padre, message);

                  for (int i = 0; i < objM.Count; i++)
                  {
                    Transform t = objM.GetChild(i);
                    Vector3 pos = initialPosition[t];
                    objM.SetEndOfChild(i, pos);
                  }
                }
              }
            }
          }
          
          // Controllo del gesto tipico per l'esplosione
          implodi = objM != null && hand.ImplosionGesture(message);
        }

        // Parte grafica dell'esplosione. Sposto tutti gli oggetti, già selezionati, di una distanza calcolata in precedenza
        if (implodi)
          implodi = hand.Play(objM);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di implosione e fa implodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="initialPosition">Posizioni iniziali degli oggetti selezionati.</param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="colorRay">>Colore del raggio.</param>
      public static void Implosion(this RigidHand hand, Dictionary<Transform, Vector3> initialPosition, string message, Ray raySelection, Color colorRay)
      {
        Implosion(hand, initialPosition, message, raySelection, colorRay, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di implosione e fa implodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="initialPosition">Posizioni iniziali degli oggetti selezionati.</param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="tagUntouchable">Lista di tag appartenente agli oggetti da ignorare.</param>
      public static void Implosion(this RigidHand hand, Dictionary<Transform, Vector3> initialPosition, string message, Ray raySelection, params string[] tagUntouchable)
      {
        Implosion(hand, initialPosition, message, raySelection, coloreRaggioSelezione, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di implosione e fa implodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="initialPosition">Posizioni iniziali degli oggetti selezionati.</param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      public static void Implosion(this RigidHand hand, Dictionary<Transform, Vector3> initialPosition, string message, Ray raySelection)
      {
        Implosion(hand, initialPosition, message, raySelection, coloreRaggioSelezione, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di implosione e fa implodere gli oggetti, selezionati dal raggio con origine sul palmo della mano e con direzione il vettore normale del palmo.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="initialPosition">Posizioni iniziali degli oggetti selezionati.</param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      public static void Implosion(this RigidHand hand, Dictionary<Transform, Vector3> initialPosition, string message)
      {
        Vector3 dir = hand.GetPalmNormal();
        dir.Normalize();

        // Calcolo il raggio e distanzio il punto di inizio dal palmo della mano di 0.1, così non colpisco le dita della stessa mano
        Ray raggio = new Ray(Vector3.MoveTowards(hand.GetPalmPosition(), dir, 0.07f), dir);

        Implosion(hand, initialPosition, message, raggio, coloreRaggioSelezione, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di implosione e fa implodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="initialPosition">Posizioni iniziali degli oggetti selezionati.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="colorRay">>Colore del raggio.</param>
      /// <param name="tagUntouchable">Lista di tag appartenente agli oggetti da ignorare.</param>
      public static void Implosion(this RigidHand hand, Dictionary<Transform, Vector3> initialPosition, Ray raySelection, Color colorRay, params string[] tagUntouchable)
      {
        Implosion(hand, initialPosition, testoImplosion, raySelection, colorRay, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di implosione e fa implodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="initialPosition">Posizioni iniziali degli oggetti selezionati.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="tagUntouchable">Lista di tag appartenente agli oggetti da ignorare.</param>
      public static void Implosion(this RigidHand hand, Dictionary<Transform, Vector3> initialPosition, Ray raySelection, params string[] tagUntouchable)
      {
        Implosion(hand, initialPosition, testoImplosion, raySelection, coloreRaggioSelezione, tagUntouchable);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di implosione e fa implodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="initialPosition">Posizioni iniziali degli oggetti selezionati.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      /// <param name="colorRay">>Colore del raggio.</param>
      public static void Implosion(this RigidHand hand, Dictionary<Transform, Vector3> initialPosition, Ray raySelection, Color colorRay)
      {
        Implosion(hand, initialPosition, testoImplosion, raySelection, colorRay, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di implosione e fa implodere gli oggetti, escludendo quelli provvisti di tagUntouchagle da ignorare, selezionati dal raggio raySelection.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="initialPosition">Posizioni iniziali degli oggetti selezionati.</param>
      /// <param name="raySelection">Raggio di selezione che punta sugli oggetti da far esplodere.</param>
      public static void Implosion(this RigidHand hand, Dictionary<Transform, Vector3> initialPosition, Ray raySelection)
      {
        Implosion(hand, initialPosition, testoImplosion, raySelection, coloreRaggioSelezione, tag);
      }

      /// <summary>
      /// Controlla se è stato effettuato il gesto di implosione e fa implodere gli oggetti, selezionati dal raggio con origine sul palmo della mano e con direzione il vettore normale del palmo.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="initialPosition">Posizioni iniziali degli oggetti selezionati.</param>
      public static void Implosion(this RigidHand hand, Dictionary<Transform, Vector3> initialPosition)
      {
        Implosion(hand, initialPosition, testoImplosion);
      }

      /// <summary>
      /// Rende gli oggetti, figli di parent, selezionati, quindi pronti per essere esplosi o implosi, entro il tempo temp.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="parent">Oggetto padre che si vuole far esplodere.</param>
      /// <param name="message">Testo di notifica da far visualizzare a schermo, quando tutto è pronto.</param>
      /// <param name="temp">Tempo massimo a disposizione per poter effettuare l'esplosione, superato il quale bisogna rendere, di nuovo, gli oggetti selezinati.</param>
      /// <returns></returns>
      public static ObjectsMove SelectObjects(this RigidHand hand, Transform parent, string message, float temp = 3)
      {
        if (!selezioneOggetti)
        {
          ObjectsMove om = new ObjectsMove();
          GameObject canvas = null;
          string nomeCanvas = "Canvas explosion";
          int count = parent.childCount;
          bool notificaEffettuata = false;

          // Per tutti i figli calcolo il punto iniziale e finale della traiettoria di esplosione
          for (int i = 0; i < count; i++)
          {
            Transform figlio = parent.GetChild(i);
            Vector3 startC = figlio.localPosition;
            MeshRenderer mr = figlio.GetComponent<MeshRenderer>();

            om.Add(new ObjectMove(figlio, startC, startC * 2, mr.material));
            mr.material = selezione;
          }
          
          selezioneOggetti = true;
          tempo = 0;

          // Rendo l'oggetto colpito "esplodibile", se già non è stato fatto in precedenza. Quindi notifico l'utente con un messaggio scritto.
          Canvas[] can = Object.FindObjectsOfType<Canvas>();

          foreach (Canvas c in can)
            if (c.name == nomeCanvas)
            {
              canvas = c.gameObject;
              notificaEffettuata = true;
              break;
            }

          // Creazione della notifica (Canvas e Text)
          if (!notificaEffettuata)
          {
            #region Creazione della notifica

            canvas = new GameObject();
            canvas.transform.SetParent(Camera.main.transform);
            GameObject text = new GameObject();
            canvas.name = nomeCanvas;
            canvas.AddComponent<RectTransform>();
            canvas.AddComponent<Canvas>();
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();

            RectTransform rtc = canvas.GetComponent<RectTransform>();
            rtc.localPosition = new Vector3(0, 0, 0.3f);
            rtc.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            rtc.sizeDelta = new Vector2(20, 20);

            Canvas c = canvas.GetComponent<Canvas>();
            c.renderMode = RenderMode.WorldSpace;
            c.worldCamera = Camera.main;

            CanvasScaler cs = canvas.GetComponent<CanvasScaler>();
            cs.dynamicPixelsPerUnit = 100;
            cs.referencePixelsPerUnit = 100;

            GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
            gr.ignoreReversedGraphics = false;
            gr.blockingObjects = GraphicRaycaster.BlockingObjects.None;

            text.transform.SetParent(canvas.transform);
            text.name = "Text explosion";
            text.AddComponent<RectTransform>();
            text.AddComponent<Text>();

            RectTransform rtt = text.GetComponent<RectTransform>();
            rtt.pivot = new Vector2(0, 0);
            rtt.localPosition = new Vector3(2.5f, -8.5f, 0);
            rtt.localScale = new Vector3(1, 1, 1);
            rtt.sizeDelta = new Vector2(10, 2);

            Text t = text.GetComponent<Text>();
            t.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            t.fontSize = 1;
            t.color = Color.red;
            t.raycastTarget = false;
            t.text = message;

            #endregion
          }

          // Effettuo la distruzione della notificha (se c'è). Se supero il tempo, dovrò ripetere l'operazione
          Object.Destroy(canvas, temp);

          return om;
        }

        return objM;
      }

      /// <summary>
      /// Rende gli oggetti, figli di parent, selezionati, quindi pronti per essere esplosi, entro il tempo temp.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="parent">Oggetto padre che si vuole far esplodere.</param>
      /// <param name="temp">Tempo massimo a disposizione per poter effettuare l'esplosione, superato il quale bisogna rendere, di nuovo, gli oggetti selezinati.</param>
      /// <returns></returns>
      public static ObjectsMove SelectObjectsExplosion(this RigidHand hand, Transform parent, float temp = 3)
      {
        return SelectObjects(hand, parent, testoExplosion, temp);
      }

      /// <summary>
      /// Rende gli oggetti, figli di parent, selezionati, quindi pronti per essere implosi, entro il tempo temp.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="parent">Oggetto padre che si vuole far esplodere.</param>
      /// <param name="temp">Tempo massimo a disposizione per poter effettuare l'esplosione, superato il quale bisogna rendere, di nuovo, gli oggetti selezinati.</param>
      /// <returns></returns>
      public static ObjectsMove SelectObjectsImplosion(this RigidHand hand, Transform parent, float temp = 3)
      {
        return SelectObjects(hand, parent, testoExplosion, temp);
      }

      /// <summary>
      /// Gesto di esplosione. Restituisce true se è stato effettuato correttamente il tipico gesto di esplosione/apertura di un assemblato di oggetti, false altrimenti.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      public static bool ExplosionGesture(this RigidHand hand, string message)
      {
        return Gesture(hand, 0, 1, message);
      }

      /// <summary>
      /// Gesto di esplosione. Restituisce true se è stato effettuato correttamente il tipico gesto di esplosione/apertura di un assemblato di oggetti, false altrimenti.
      /// </summary>
      /// <param name="hand"></param>
      public static bool ExplosionGesture(this RigidHand hand)
      {
        return ExplosionGesture(hand, testoExplosion);
      }

      /// <summary>
      /// Gesto di implosione. Restituisce true se è stato effettuato correttamente il tipico gesto di implosione/chiusura di un assemblato di oggetti, false altrimenti.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="message">Messaggio di notifica per l'utente.</param>
      /// <returns></returns>
      public static bool ImplosionGesture(this RigidHand hand, string message)
      {
        return Gesture(hand, 1, 0, message);
      }

      /// <summary>
      /// Gesto di implosione. Restituisce true se è stato effettuato correttamente il tipico gesto di implosione/chiusura di un assemblato di oggetti, false altrimenti.
      /// </summary>
      /// <param name="hand"></param>
      /// <returns></returns>
      public static bool ImplosionGesture(this RigidHand hand)
      {
        return ImplosionGesture(hand, testoImplosion);
      }

      /// <summary>
      /// Grafica dell'esplosione (implosione). Restituisce il valore true finchè il movimento degli oggetti è attivo, restituisce il valore false una volta arrivati a destinazione.
      /// </summary>
      /// <param name="hand"></param>
      /// <param name="items">Collezione degli oggetti che si vuole spostare.</param>
      /// <returns></returns>
      public static bool Play(this RigidHand hand, ObjectsMove items)
      {
        tempo += Time.deltaTime;
        float percento = tempo > tempoMax ? 1 : Mathf.Pow(tempo / tempoMax, 1f / 3f);

        for (int i = 0; i < items.Count; i++)
          items.GetChild(i).localPosition = Vector3.Lerp(items.GetStartOfChild(i), items.GetEndOfChild(i), percento);

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

      private static bool Gesture(RigidHand hand, float grabStrengthCorrente, float grabStrengthPassato, string testoGesto)
      {
        Controller c = new Controller();
        Frame framePassato = c.Frame(4);
        Frame frameCorrente = c.Frame();
        Hand manoPassata = framePassato.Hand(hand.LeapID()), manoCorrente = frameCorrente.Hand(hand.LeapID());

        RipristinaMaterial(testoGesto);

        return manoCorrente != null && manoPassata != null && manoCorrente.GrabStrength == grabStrengthCorrente && manoPassata.GrabStrength == grabStrengthPassato && selezioneOggetti;
      }
      
      private static void RipristinaMaterial(string message)
      {
        Text testo = Object.FindObjectOfType<Text>();
        selezioneOggetti = testo != null && testo.text == message;

        if (!selezioneOggetti)
          for (int i = 0; i < objM.Count; i++)
          {
            MeshRenderer mr = objM.GetChild(i).GetComponent<MeshRenderer>();

            if (mr != null)
              mr.material = objM.GetMaterialOfChild(i);
          }
      }

      #endregion
    }

    public class ObjectMove
    {
      private Transform obj;
      private Vector3 start, end;
      private Material material;

      public Transform Obj { get { return obj; } set { obj = value; } }
      public Vector3 Start { get { return start; } set { start = value; } }
      public Vector3 End { get { return end; } set { end = value; } }
      public Material Material { get { return material; } set { material = value; } }

      public ObjectMove(Transform obj, Vector3 start, Vector3 end, Material material)
      {
        this.obj = obj;
        this.start = start;
        this.end = end;
        this.material = material;
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

      public Material GetMaterialOfChild(int index) { return lista[index].Material; }

      public void SetEndOfChild(int index, Vector3 newEnd) { lista[index].End = newEnd; }

      public void Add(ObjectMove item) { lista.Add(item); }

      public void RemoveAll() { lista.RemoveRange(0, lista.Count); }
    }
  }
}