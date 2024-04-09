using UnityEngine;
using System.Collections.Generic;

public class PlantManger : MonoBehaviour
{
  public static PlantManger Instance { get; private set; }
  public List<GameObject> plantType = new List<GameObject>();//植物类型列表
  void Awake()
  {
    Instance = this;
  }
}