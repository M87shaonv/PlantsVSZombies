using System;
using System.Collections.Generic;
using UnityEngine;
public class JsonDataList : ScriptableObject
{
  public List<LevelSpeakData> JsonData = new List<LevelSpeakData>();
}
[System.Serializable]
public class LevelSpeakData
{
  public int LevelID;
  public int Showed;
  public String[] text;
}