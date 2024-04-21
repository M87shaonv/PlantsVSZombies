using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferPoolManager
{
  private static BufferPoolManager instance;
  public static BufferPoolManager Instance
  {
    get
    {
      if (instance == null)
      {
        instance = new BufferPoolManager();
      }
      return instance;
    }
  }
  private GameObject poolObj;

  // 预制体,实例化后的具体游戏物体
  private Dictionary<GameObject, List<GameObject>> poolDataDic = new Dictionary<GameObject, List<GameObject>>();
  /// <summary>
  /// 从缓存池中获取第一个对象
  /// </summary>
  public GameObject GetObj(GameObject prefab)
  {
    GameObject obj = null;
    // 如果字典中存在预制体这个key并且数量大于0
    if (poolDataDic.ContainsKey(prefab) && poolDataDic[prefab].Count > 0)
    {
      // 返回list的第一个
      obj = poolDataDic[prefab][0];
      // 同时移除第一个
      poolDataDic[prefab].RemoveAt(0);
    }
    else
    {
      obj = GameObject.Instantiate(prefab);
    }
    if (obj == null) return null;
    obj.SetActive(true);
    obj.transform.SetParent(null);//设置父物体为空
    return obj;
  }

  public void PushObj(GameObject prefab, GameObject obj)
  {
    if (obj != null && obj.activeSelf)//验证对象是否处于有效状态
    {

      // 判断是否有根目录，没有则创建一个空游戏物体作为根
      if (poolObj == null) poolObj = new GameObject("poolObj");
      // 存在这个key
      if (poolDataDic.ContainsKey(prefab))
      {
        // 把物体放进去
        poolDataDic[prefab].Add(obj);
      }
      else
      {
        // 创建这个预制体的缓存池数据
        poolDataDic.Add(prefab, new List<GameObject>() { obj });
      }

      // 如果根目录下没有预制体命名的子物体
      if (poolObj.transform.Find(prefab.name) == null)
      {
        // 则创建，并且父节点是poolObj
        new GameObject(prefab.name).transform.SetParent(poolObj.transform);
      }
      // 隐藏
      obj.SetActive(false);
      // 设置父物体为根目录下的该预制体
      obj.transform.SetParent(poolObj.transform.Find(prefab.name));

    }
  }
  // 清除所有的数据
  public void Clear()
  {
    poolDataDic.Clear();
  }
  /// <summary>
  /// 等待一段时间后再放回缓存池
  /// </summary>
  /// <param name="gameobjectPerfab">预制体对象</param>
  /// <param name="gameobject">具体对象</param>
  /// <param name="time">等待时间</param>
  /// <returns></returns>
  public IEnumerator WaitAndPush(GameObject gameobjectPerfab, GameObject gameobject, float time)
  {
    yield return new WaitForSeconds(time);
    if (gameobject != null && gameobject.activeSelf)
    {
      PushObj(gameobjectPerfab, gameobject);
    }
  }
}