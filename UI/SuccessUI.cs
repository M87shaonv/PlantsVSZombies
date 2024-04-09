using UnityEngine;

public class SuccessUI : MonoBehaviour
{
  private Animator anim;
  void Awake()
  {
    anim = GetComponent<Animator>();
  }
  void Start()
  {
    anim.enabled = false; // 关闭动画
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
