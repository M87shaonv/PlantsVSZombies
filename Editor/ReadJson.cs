using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json; // 需要导入Newtonsoft.Json库来进行JSON解析
using UnityEditor;

using UnityEngine;

[InitializeOnLoad]
public class ReadJson
{
	static ReadJson()
	{
		return;//读取json文本的代码,仅读取1次
		string jsonPath = "Assets/Resources/CrazyDave.json";
		string jsonContent = File.ReadAllText(jsonPath);

		// 使用Newtonsoft.Json进行反序列化
		List<LevelSpeakData> jsonDataList = JsonConvert.DeserializeObject<List<LevelSpeakData>>(jsonContent);
		// 创建ScriptableObject实例并填充数据
		JsonDataList asset = (JsonDataList)ScriptableObject.CreateInstance<JsonDataList>();
		asset.JsonData = jsonDataList;

		// 保存到Resources文件夹，以便运行时访问。记得先在Assets文件夹下创建Resources文件夹
		string assetPath = "Assets/Resources/JsonDataList.asset";
		AssetDatabase.CreateAsset(asset, assetPath);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();//刷新资源
	}

}
