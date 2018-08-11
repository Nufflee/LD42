using System.Collections.Generic;
using UnityEngine;

public class Army : MonoBehaviour
{
  public List<Entity> entities = new List<Entity>();

  public void Add(Entity entity)
  {
    entities.Add(entity);
  }

  public void Remove(Entity entity)
  {
    entities.Remove(entity);
  }

  public void Attack(Army army)
  {
    Entity[] entities = GetComponentsInChildren<Entity>();

    for (int i = 0; i < entities.Length; i++)
    {
      if (i >= army.entities.Count)
      {
        return;
      }

      Entity entity = entities[i];

      entity.Attack(army.entities[i]);
    }
  }
}