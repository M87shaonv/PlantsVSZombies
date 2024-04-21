using UnityEngine;
using System.Collections.Generic;
/// <summary>
/// 在使用bufferpoolmanger时使用
/// </summary>
public enum PlantTypes
{
  Sunflower,//0
  PeaShooterOne,//1
  PeaShooter,//2
  WallNut,//3
  TorchWood,//4
  Squash,//5
  SnowPeaShooter,//6
  MachineGunPeaShooter,//7
  CabbagePitcher,//8
  OldWallLamp,//9
  Carambola,//10
  MagicCat,//11
  CerryBomb,//12
  CornCannon,//13
}
public class PlantManger : MonoBehaviour
{
  public static PlantManger Instance { get; private set; }
  public List<GameObject> plantType = new List<GameObject>();//植物类型列表
  void Awake()
  {
    Instance = this;
  }
}