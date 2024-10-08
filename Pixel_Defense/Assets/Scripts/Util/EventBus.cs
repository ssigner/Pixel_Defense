using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EventBus
{
    static Dictionary<Type, List<Delegate>> eventDictionary = new Dictionary<Type, List<Delegate>>();

    public static void Subscribe<T>(System.Action<T> onTowerSummon )
    {
        var evType = typeof(T);
        if (eventDictionary.ContainsKey(evType)==false)
        {
            eventDictionary[evType] = new List<Delegate>();
        }

        eventDictionary[evType].Add(onTowerSummon);
    }

    public static void Publish<T>( T ev)
    {

        var actionList = eventDictionary.GetValueOrDefault(typeof(T));
        if (actionList == null)
        {
            return;
        }

        foreach (var onHandle  in actionList)
        {
           var handle=  onHandle as System.Action<T>;
            if (handle == null)
                continue;
            handle.Invoke(ev);
        }

    }

    internal static void Unsubscribe<T>(Action<T> onTowerSummon)
    {
        var evType = typeof(T);
        if (eventDictionary.ContainsKey(evType) == false)
        {
            return;
        }
        eventDictionary[evType].Remove(onTowerSummon);
    }
}