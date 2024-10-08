using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms;

[AttributeUsage(AttributeTargets.Field)]
class Inject : Attribute
{
    public string key;
    public Inject()
    {
        key = "";
    }

    public Inject(string key)
    {
        this.key = key;
    }

}


public class InjectObj
{
    public bool isInjected=false;

    public void CheckInject(object o)
    {
        if (isInjected)
            return;
        isInjected = true;
        DIContainer.Inject(o);

    }
}

public class DIContainer 
{
    static DIContainer _current,_global;
    public static DIContainer Current 
    {
        get => _current;
        set => _current = value;
        
    }

    public static DIContainer Global
    {
        get {
            if(_global == null)
            {
                _global = new DIContainer();
            }
            return _global;
        }

    }

    Dictionary<string, object> objContianer = new Dictionary<string, object>();


    private static string GenKey(Type fieldType, string key="")
    {
        return fieldType.Name + "__" + key;
    }


    public void Regist<T>(T obj)
    {
        objContianer.Add(GenKey(typeof(T)), obj);
    }
    public void Regist<T>(T obj,string name)
    {
        objContianer.Add(GenKey(typeof(T),name), obj);
    }


  

    public object Get(Type type, string key)
    {
        key=GenKey(type, key);
        if (objContianer.ContainsKey(key)  )
        {
            return objContianer[key];
        }

        return null;
    }

    public bool Has(Type type,string key)
    {
        string dKey = GenKey(type, key);

        return objContianer.ContainsKey(dKey);
    }


    public object Get(Type type)
    {
        var  key = GenKey(type);
        if (objContianer.ContainsKey(key)  )
        {
            return objContianer[key];
        }

        return null;
    }


    public T Get<T>(string name) where T : class
    {
        return objContianer[GenKey(typeof(T),name)] as T;
    }


    public static object GetValue(Type fieldType, string key="")
    {
        object value = null;


        if (DIContainer.Current != null)
        {
            value = DIContainer.Current.Get(fieldType, key);
        }

        if (value == null)
        {
            value = Global.Get(fieldType, key);
        }

        return value;
    }

    public static void Inject(object o)
    {
        Type type = o.GetType();

        foreach(var field in type.GetFields( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
        {
            var injectObj = field.GetAttribute<Inject>();
            var fieldType = field.FieldType;
            
            if(injectObj == null) continue;

            object value= GetValue(fieldType, injectObj.key);

            if (value == null)
            {
                throw new Exception("등록되지 않은 값입니다. "+fieldType.Name + " "+ injectObj.key);
            }

            field.SetValue(o, value);
        }


    }


}