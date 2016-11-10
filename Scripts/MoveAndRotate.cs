using UnityEngine;
using System.Collections;

public class MoveAndRotate : MonoBehaviour
{
  public int speedRotation = 10;
  public float speedTranslate = 1;

  // Update is called once per frame
  void Update()
  {
    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
    {
      if (Input.GetKey(KeyCode.DownArrow))
        transform.Rotate(Vector3.left * speedRotation, Space.World);
      else if (Input.GetKey(KeyCode.UpArrow))
        transform.Rotate(Vector3.right * speedRotation, Space.World);

      if (Input.GetKey(KeyCode.RightArrow))
        transform.Rotate(Vector3.down * speedRotation, Space.World);
      else if (Input.GetKey(KeyCode.LeftArrow))
        transform.Rotate(Vector3.up * speedRotation, Space.World);

      if (Input.GetKey(KeyCode.A))
        transform.Rotate(Vector3.back * speedRotation, Space.World);
      else if (Input.GetKey(KeyCode.D))
        transform.Rotate(Vector3.forward * speedRotation, Space.World);
    }
    else
    {
      if (Input.GetKey(KeyCode.DownArrow))
        transform.position += Vector3.down * speedTranslate;
      else if (Input.GetKey(KeyCode.UpArrow))
        transform.position += Vector3.up * speedTranslate;

      if (Input.GetKey(KeyCode.RightArrow))
        transform.position += Vector3.right * speedTranslate;
      else if (Input.GetKey(KeyCode.LeftArrow))
        transform.position += Vector3.left * speedTranslate;

      if (Input.GetKey(KeyCode.A))
        transform.position += Vector3.back * speedTranslate;
      else if (Input.GetKey(KeyCode.D))
        transform.position += Vector3.forward * speedTranslate;
    }
  }
}