using System;
using System.Collections.Generic;
using UnityEngine;
public static class EntityGroup
{
    public static List<EntityPack> AllPacks = new List<EntityPack>();

    public static void RegisterEntity(EntitySubject entity)
    {
        EntityPack existingPack = FindNearbyPack(entity);
        if (existingPack != null)
        {
            existingPack.AddFollower(entity);
        }
        else
        {
            AllPacks.Add(new EntityPack(entity));
        }
    }

    public static void UnregisterEntity(EntitySubject entity)
    {
        EntityPack pack = FindEntityPack(entity);
        pack?.RemoveEntity(entity);
    }

    public static EntityPack FindEntityPack(EntitySubject entity)
    {
        return AllPacks.Find(pack => pack.Leader == entity || pack.GetFollowers().Contains(entity));
    }

    public static void RemovePack(EntityPack pack)
    {
        AllPacks.Remove(pack);
    }

    private static EntityPack FindNearbyPack(EntitySubject entity)
    {
        return AllPacks.Find(pack => Vector3.Distance(entity.transform.position, pack.Leader.transform.position) < 15f);
    }

    /// <summary>
    /// Retourne une liste de toutes les entités actuellement enregistrées.
    /// </summary>
    public static List<EntitySubject> GetAllEntities()
    {
        List<EntitySubject> allEntities = new List<EntitySubject>();

        foreach (EntityPack pack in AllPacks)
        {
            allEntities.Add(pack.Leader);
            allEntities.AddRange(pack.GetFollowers());
        }

        return allEntities;
    }
}

public class EntityPack
{
    public Action<EntitySubject> OnLeaderChanged;
    public EntitySubject Leader { get; private set; }
    private List<EntitySubject> followers;
    public float TotalFoodPoints { get; private set; }

    public EntityPack(EntitySubject leader)
    {
        Leader = leader;
        Leader.SetLeader(true);
        followers = new List<EntitySubject>();
    }

    public void AddFollower(EntitySubject entity)
    {
        if (!followers.Contains(entity))
        {
            followers.Add(entity);
        }
    }

    public void RemoveEntity(EntitySubject entity)
    {
        if (entity == Leader)
        {
            if (Leader) Leader.SetLeader(false);
            PromoteNewLeader();
        }
        else
        {
            followers.Remove(entity);
        }
    }

    private void PromoteNewLeader()
    {
        if (followers.Count > 0)
        {
            Leader = followers[0];
            Leader.SetLeader(true);
            followers.RemoveAt(0);
            OnLeaderChanged?.Invoke(Leader);
        }
        else
        {
            EntityGroup.RemovePack(this);
        }
    }

    public List<EntitySubject> GetFollowers()
    {
        return followers;
    }

    public void AddFoodPoints(float points)
    {
        TotalFoodPoints += points;
        Debug.Log($"Troupe {Leader.name} a accumulé {TotalFoodPoints} points de nourriture.");
    }

    public int CalculateGroupQuota()
    {
        return 25 + 10 * followers.Count;
    }
}
