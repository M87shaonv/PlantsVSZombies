using System.Collections.Generic;

public class ZombieEvent
{
  private static ZombieEvent _instance;//私有静态字段,用于存储该类的唯一实例
  public static ZombieEvent Instance//公开静态属性,用于获取该类的唯一实例
  {
    get
    {
      if (_instance == null)
      {
        _instance = new ZombieEvent();//创建该类的唯一实例
      }
      return _instance;
    }
  }

  //每一行的僵尸列表
  public Dictionary<int, List<Zombie>> zombieRows = new Dictionary<int, List<Zombie>>()
  {
            { 0, new List<Zombie>() { new Zombie() } },
            { 1, new List<Zombie>() { new Zombie() } },
            { 2, new List<Zombie>() { new Zombie() } },
            { 3, new List<Zombie>() { new Zombie() } },
            { 4, new List<Zombie>() { new Zombie() } }
  };

  public void OnZombieEntered(int rowIndex, Zombie zombie)// 僵尸进入事件
  {
    // 在该行添加生成的僵尸
    zombieRows[rowIndex].Add(zombie);
  }

  public void OnZombieExited(int rowIndex, Zombie zombie)// 僵尸离开事件
  {
    // 在该行移除离开的僵尸
    zombieRows[rowIndex].Remove(zombie);
  }
  public Dictionary<int, List<Plant>> plantRows = new Dictionary<int, List<Plant>>()
  {
            { 0, new List<Plant>() { new Plant() } },
            { 1, new List<Plant>() { new Plant() } },
            { 2, new List<Plant>() { new Plant() } },
            { 3, new List<Plant>() { new Plant() } },
            { 4, new List<Plant>() { new Plant() } }
  };

  public void OnPlantEntered(int rowIndex, Plant plant)// 植物进入事件
  {
    // 在该行添加生成的植物
    plantRows[rowIndex].Add(plant);
  }

  public void OnPlantExited(int rowIndex, Plant plant)// 植物离开事件
  {
    // 在该行移除离开的植物
    plantRows[rowIndex].Remove(plant);
  }
}