using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]

public class Ruota : MonoBehaviour
{
  public int speed = 10;
  private bool rotate;

  void Awake()
  {
    this.GetComponent<MeshCollider>().convex = true;
  }

  void Start()
  {
    rotate = false;
  }

  void Update()
  {
    if (Input.GetMouseButtonUp(1))
    {
      rotate = false;
    }

    if (rotate)
    {
      float rotX = Input.GetAxis("Mouse X") * speed, rotY = Input.GetAxis("Mouse Y") * speed;
      transform.Rotate(Vector3.down, rotX, Space.World);
      transform.Rotate(Vector3.right, rotY, Space.World);
    }
  }

  void OnMouseOver()
  {
    if (Input.GetMouseButtonDown(1))
    {
      rotate = true;
    }
  }
}
