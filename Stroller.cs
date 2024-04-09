using UnityEditor;
using UnityEngine;

public class Stroller : MonoBehaviour
{
  public float Speed = 5;
  public bool isCrash = false;
  void OnTriggerEnter2D(Collider2D other)
  {

    if (other.CompareTag("Zombie"))
    {
      isCrash = true;
      AudioManger.Instance.PlayClip(Config.Stroller);
      other.GetComponent<Zombie>().Dead();
    }
  }
  void Update()
  {
    if (isCrash)
    {
      Move();
      Destroy(this.gameObject, 5f);
    }
  }
  void Move()
  {
    transform.Translate(Vector2.right * Speed * Time.deltaTime);
  }
}