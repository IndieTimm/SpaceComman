using System.Collections.Generic;
using UnityEngine;

//TODO: REWORK
public enum GameSystemType
{
    UI = 0,
    PlayerHighPriority = 1,    // SLOTS
    PlayerMiddlePriority = 2,      
    Camera = 3,
    PlayerLowPriority = 4,      // DEFAULT
}

public class InputFocusWrapper
{
    private int id;
    private object sender;
    
    public InputFocusWrapper(int id, object sender)
    {
        this.id = id;
        this.sender = sender;
    }

    public override bool Equals(object obj)
    {
        return obj is InputFocusWrapper wrapper &&
               id == wrapper.id &&
               EqualityComparer<object>.Default.Equals(sender, wrapper.sender);
    }

    public override int GetHashCode()
    {
        int hashCode = 256141636;
        hashCode = hashCode * -1521134295 + id.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(sender);
        return hashCode;
    }
}

public static class GameInputFocus
{
    private class LockItem
    {
        public GameSystemType type;
        public List<object> senders = new List<object>();
    }
    private static List<LockItem> lockItems = new List<LockItem>();

    public static bool IsActive(GameSystemType type)
    {
        var item = lockItems.Find(x => (int)x.type < (int)type && x.senders.Count > 0);

        return item == null;
    }

    public static void Unfocus(object sender)
    {
        var item = lockItems.Find(x => x.senders.Contains(sender));

        if (item != null)
        {
            item.senders.Remove(sender);
        }
    }

    public static void Focus(GameSystemType type, object sender)
    {
        var item = lockItems.Find(x => x.type == type);

        if (item == null)
        {
            item = new LockItem()
            {
                type = type
            };

            lockItems.Add(item);
        }

        if (!item.senders.Contains(sender))
        {
            Debug.Log($"{sender} {type}");
            item.senders.Add(sender);
        }
    }
}