using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

[AttributeUsage(AttributeTargets.Field)]
class Inject : Attribute
{
    public string key;
    public Inject()
    {
        key = null;
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
        DIContainer.Current.Inject(o);

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

    public static string GenKey<T>()
    {
        return typeof(T).Name;
    }

    public static string GenKey(Type type)
    {
        return type.Name;
    }
    private string GenKey(Type fieldType, string key)
    {
        return GenKey(fieldType) + "__" + key;
    }


    public static string GenKey<T>(string name)
    {
        return GenKey(typeof(T)) + "__" + name;
    }

    public void Regist<T>(T obj)
    {
        objContianer.Add(GenKey<T>(), obj);
    }
    public void Regist<T>(T obj,string name)
    {
        objContianer.Add(GenKey<T>(name), obj);
    }


  

    public object Get(Type type, string key)
    {
        key=GenKey(type, key);
        if (objContianer.ContainsKey(key) == false && this != Global)
        {
            return Global.Get(type, key);
        }

        return objContianer[key];
    }


    public object Get(Type type)
    {
        var  key = GenKey(type);
        if (objContianer.ContainsKey(key) == false && this !=Global)
        {
            return Global.Get(type);
        }

        return objContianer[key];
    }


    public T Get<T>(string name) where T : class
    {
        return objContianer[GenKey<T>(name)] as T;
    }

    public void Inject(object o)
    {
        Type type = o.GetType();

        foreach(var field in type.GetFields( System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
        {
            var injectObj = field.GetAttribute<Inject>();
            var fieldType = field.FieldType;
            
            if(injectObj == null) continue;

            object value;

            if (string.IsNullOrEmpty(injectObj.key))
            {
                value = Get(fieldType);
            }
            else
            {
                value = Get(fieldType, injectObj.key);
            }

            field.SetValue(o, value);
        }


    }


}