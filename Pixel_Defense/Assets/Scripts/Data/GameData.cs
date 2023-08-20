using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExcelDataReader;
using System.IO;
using System.Data;
using System;
using UnityEngine.Rendering;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class GameData :ScriptableObject
{
    public List<Item> items;
    public List<Monster> monsters;


#if UNITY_EDITOR

    [MenuItem("Test/Reflection")]
    public static void TestReflection()
    {
        var type = typeof(Item);
        var itemObj = new Item();
        type=itemObj.GetType();

        var fieldInfos = type.GetFields();

        itemObj.price = 3;

        var priceFieldInfo=type.GetField("price");

        priceFieldInfo.SetValue(itemObj,5);
        var v= priceFieldInfo.GetValue(itemObj);

        Debug.Log($"{v}  {itemObj.price}");

        //field
        //Property

    }

    [MenuItem("Test/ReadExcel")]
    public static void ReadExcel()
    {
        string excelPath = "Assets/xlsx/data.xlsx";

        if (File.Exists(excelPath)==false) {
            throw new System.Exception($"{excelPath}에 엑셀파일이 없습니다.");
        }

        DataSet dataset;

        using (var stream = File.Open(excelPath, FileMode.Open, FileAccess.Read,FileShare.ReadWrite))
        {           
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {             
                 dataset = reader.AsDataSet();
            }
        }


        var table= dataset.Tables[0];

        Debug.Log(table.TableName);

        var rowCnt = table.Rows.Count;
        var columnCnt = table.Columns.Count;
        Debug.Log($"row:{rowCnt} col :{columnCnt}");

        var v= table.Rows[0][0];
        Debug.Log($"value: {v}  type:{v.GetType()}" );

        for (int i = 0; i < rowCnt; i++) 
        {
            string temp = "";
            for(int j = 0; j < columnCnt; j++)
            {
                temp += table.Rows[i][j];
                temp += " ";
                temp += table.Rows[i][j].GetType().ToString();
                temp += " ";
            }
            Debug.Log(temp);
        }

        string assetPath = "Assets/Prefabs/data.asset";
        GameData gamedata;

        gamedata=AssetDatabase.LoadAssetAtPath<GameData>(assetPath);

        bool needCreate = gamedata == null;
        if (needCreate)
        {
            gamedata = ScriptableObject.CreateInstance<GameData>();

            AssetDatabase.CreateAsset(gamedata, assetPath);
        }
        //TODO: 한줄 씩 추가하는것 마저 자동화 시키기

        gamedata.items = MakeListFromDataSet<Item>(dataset, "Items");
        gamedata.monsters = MakeListFromDataSet<Monster>(dataset, "Monsters");

        //


        if (needCreate == false)
        {
            EditorUtility.SetDirty(gamedata);
        }

    
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


    }


    private static List<T> MakeListFromDataSet<T>(DataSet dataset, string tableName) where T:class,new()
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

            for(int j = 0; j < fieldInfos.Length; j++)
            {
                var fieldInfo = fieldInfos[j];
                int fieldIdx = findFieldIdx(fieldNameList, fieldInfo.Name);
                if (fieldIdx != -1) {
                    var value = table.Rows[i][fieldIdx];
                    if (fieldInfo.FieldType== typeof(int))
                    {
                        value = Convert.ToInt32(value);
                    }
                    else if(fieldInfo.FieldType == typeof(string))
                    {
                        value = Convert.ToString(value);
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

    private static int findFieldIdx(List<string> fieldNameList ,string fieldName)
    {
        for(int i = 0; i < fieldNameList.Count; i++)
        {
            if (fieldNameList[i] == fieldName) return i;
        }
        return -1;
    }

    private static List<Monster> MakeMonstersByDataSet(DataSet dataset, string tableName)
    {
        var table = findTable(dataset, tableName);

        var rowCnt = table.Rows.Count;
        var columnCnt = table.Columns.Count;

        var monsterList = new List<Monster>();

        var fieldNameList = new List<string>();

        for(int i = 0; i < columnCnt; i++)
        {
            fieldNameList.Add(table.Rows[0][i].ToString());
        }
        


        for (int i = 1; i < rowCnt; i++)
        {
            var monster = new Monster();


            int fieldIdx = findFieldIdx(fieldNameList, "code");

            if(fieldIdx != -1) monster.code = Convert.ToInt32(table.Rows[i][fieldIdx]);

            fieldIdx = findFieldIdx(fieldNameList, "name");
            if (fieldIdx != -1) monster.name = Convert.ToString(table.Rows[i][fieldIdx]);

            fieldIdx = findFieldIdx(fieldNameList, "atk");
            if (fieldIdx != -1) monster.atk = Convert.ToInt32(table.Rows[i][fieldIdx]);

            fieldIdx = findFieldIdx(fieldNameList, "hp");
            if (fieldIdx != -1) monster.hp = Convert.ToInt32(table.Rows[i][fieldIdx]);

            monsterList.Add(monster);
        }
        return monsterList;
    }

    private static List<Item> MakeItemsByDataSet(DataSet dataset,string tableName)
    {
        var table = findTable(dataset, tableName);

        var rowCnt = table.Rows.Count;
        var columnCnt = table.Columns.Count;

        var itemList = new List<Item>();

        for (int i = 1; i < rowCnt; i++)
        {
            var item = new Item();
            item.code = Convert.ToInt32(table.Rows[i][0]);
            item.name = Convert.ToString(table.Rows[i][1]);
            item.price = Convert.ToInt32(table.Rows[i][2]);
            itemList.Add(item);
        }
        return itemList;
    }


#endif

}
