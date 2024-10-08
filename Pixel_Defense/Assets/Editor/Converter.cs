using ExcelDataReader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class Converter
{
    public static void ReadExcel()
    {
        string excelPath = "Assets/xlsx/data.xlsx";

        if (File.Exists(excelPath) == false)
        {
            throw new System.Exception($"{excelPath}에 엑셀파일이 없습니다.");
        }

        DataSet dataset;

        using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                dataset = reader.AsDataSet();
            }
        }

        string assetPath = "Assets/Prefabs/data.asset";
        GameData gamedata;

        gamedata = AssetDatabase.LoadAssetAtPath<GameData>(assetPath);

        bool needCreate = gamedata == null;
        if (needCreate)
        {
            gamedata = ScriptableObject.CreateInstance<GameData>();

            AssetDatabase.CreateAsset(gamedata, assetPath);
        }

        Type type = typeof(GameData);


        FieldInfo[] fields = type.GetFields();
        foreach (FieldInfo field in fields)
        {
            if (field.FieldType.GetGenericTypeDefinition() != typeof(List<>))
                continue;

            Type fieldType = field.FieldType.GenericTypeArguments[0];
            MethodInfo method = typeof(Converter).GetMethod("MakeListFromDataSet", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static).MakeGenericMethod(fieldType);

            var result = method.Invoke(null, new object[] { dataset, field.Name });

            field.SetValue(gamedata, result);
        }

        if (needCreate == false)
        {
            EditorUtility.SetDirty(gamedata);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


    }
    private static List<T> MakeListFromDataSet<T>(DataSet dataset, string tableName) where T : class, new()
    {
        List<T> result = new List<T>();
        var table = findTable(dataset, tableName);

        if (table == null)
        {
            throw new Exception($"{tableName} : 해당 이름의 테이블이 존재하지 않습니다. ");
        }

        var rowCnt = table.Rows.Count;
        var columnCnt = table.Columns.Count;

        var fieldNameList = new List<string>();
        for (int i = 0; i < columnCnt; i++)
        {
            fieldNameList.Add(table.Rows[0][i].ToString());
        }

        for (int i = 1; i < rowCnt; i++)
        {
            var typeObj = new T();
            var type = typeObj.GetType();


            var fieldInfos = type.GetFields();

            for (int j = 0; j < fieldInfos.Length; j++)
            {
                var fieldInfo = fieldInfos[j];
                int fieldIdx = findFieldIdx(fieldNameList, fieldInfo.Name);
                if (fieldIdx != -1)
                {
                    var value = table.Rows[i][fieldIdx];

                    if (value is DBNull) {
                        continue;
                    }

                    var fieldType = fieldInfo.FieldType;


                    if (fieldType.GetInterface("IFillFromStr") != null)
                    {
                        var fieldObj = System.Activator.CreateInstance(fieldType) as IFillFromStr;
                        fieldObj.fillFromStr(value.ToString());
                        value = fieldObj;

                    }
                    else if (fieldType.IsEnum)
                    {
                        value = Enum.Parse(fieldType, value as string);
                    }
                    else if (fieldType.IsPrimitive || fieldType == typeof(string))
                    {
                        value = Convert.ChangeType(value, fieldType);
                    }


                    fieldInfo.SetValue(typeObj, value);
                }


            }

            result.Add(typeObj);
        }

        return result;

    }


    private static DataTable findTable(DataSet dataset, string tableName)
    {

        for (int i = 0; i < dataset.Tables.Count; i++)
        {
            if (dataset.Tables[i].TableName == tableName)
            {
                return dataset.Tables[i];
            }
        }

        return null;
    }

    private static int findFieldIdx(List<string> fieldNameList, string fieldName)
    {
        for (int i = 0; i < fieldNameList.Count; i++)
        {
            if (fieldNameList[i] == fieldName) return i;
        }
        return -1;
    }
}
