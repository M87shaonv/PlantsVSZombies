using UnityEngine;

public class FailUI : MonoBehaviour
{
  private Animator anim;
  void Awake()
  {
    anim = GetComponent<Animator>();
  }
  void Start()
  {
    Hide();
  }
  void Hide()
  {
    anim.enabled = false;
  }
  public void Show()
  {
    anim.enabled = true;
  }
}