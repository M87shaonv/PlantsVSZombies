using UnityEngine;
using System;

public class PrepareUI : MonoBehaviour
{
  private Animator anim;
  private Action OnComplete;
  void Start()
  {
    anim = GetComponent<Animator>();
    anim.enabled = false;
  }
  public void Show(Action OnComplete)
  {
    this.OnComplete = OnComplete;
    anim.enabled = true;
  }

  void OnShowComplete()
  {
    //如果不为null，则执行回调函数；为null时，则直接返回
    OnComplete?.Invoke();
  }
}
