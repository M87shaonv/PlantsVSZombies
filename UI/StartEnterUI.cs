using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartEnterUI : MonoBehaviour
{
  public Slider slider;
  public GameObject startbutton;
  public GameObject roll;
  AsyncOperation operation;
  private float currentProgress;
  void Start()
  {
    operation = SceneManager.LoadSceneAsync(1);
    operation.allowSceneActivation = false;
  }
  void Update()
  {
    currentProgress = operation.progress + 0.1f;
    slider.value = currentProgress;
    if (currentProgress >= 1)
    {
      startbutton.SetActive(true);
    }
    Quaternion rotation = Quaternion.AngleAxis(Time.deltaTime * Mathf.Rad2Deg, Vector3.up);
    roll.transform.rotation = transform.rotation * rotation;
  }

  public void OnStartButtonClick()
  {
    SceneManager.LoadScene(1);
  }
}
