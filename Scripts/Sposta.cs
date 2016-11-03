using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]

public class Sposta : MonoBehaviour {

  private Vector3 posObj;
  private Vector3 offset;

  void Awake()
  {
    this.GetComponent<MeshCollider>().convex = true;
  }

  void OnMouseDown()
  {
    posObj = Camera.main.WorldToScreenPoint(gameObject.transform.position);
    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, posObj.z));
  }

  void OnMouseDrag()
  {
    Vector3 posCorrente = new Vector3(Input.mousePosition.x, Input.mousePosition.y, posObj.z);
    transform.position = Camera.main.ScreenToWorldPoint(posCorrente) + offset;
  }
}
