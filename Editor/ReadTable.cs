using System;
using System.IO;
using System.Reflection;
using OfficeOpenXml;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class Startup
{
	static Startup()
	{
		return;//读取表格的代码,仅读取1次
		string path = Application.dataPath + "/Editor/Level.xlsx";//表格存储位置
		string assetName = "Level";//导出数据后存储的资源名

		FileInfo fileInfo = new(path);
		LevelData levelData = (LevelData)ScriptableObject.CreateInstance(typeof(LevelData));//创建一个LevelData的实例 
		using (ExcelPackage excelPackage = new(fileInfo))
		{
			//读取表格内的具体表单
			ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["normal"];//获取第一个工作表
			for (int i = worksheet.Dimension.Start.Row + 2; i <= worksheet.Dimension.End.Row; i++)//遍历表格的每一行
			{
				LevelItem levelItem = new();
				Type type = typeof(LevelItem);//获取LevelItem的类型
				for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)//遍历表格的每一列
				{
					//使用反射方式对levelItem进行赋值
					FieldInfo variable = type.GetField(worksheet.GetValue(2, j).ToString());
					string tableValue = worksheet.GetValue(i, j).ToString();
					variable.SetValue(levelItem, Convert.ChangeType(tableValue, variable.FieldType));
				}
				levelData.levelDataList.Add(levelItem);//将levelItem添加到levelData的列表中
			}

		}
		AssetDatabase.CreateAsset(levelData, "Assets/Resources/" + assetName + ".asset");//将levelData存储到资源中
		AssetDatabase.SaveAssets();//保存资源
		AssetDatabase.Refresh();//刷新资源
	}
}