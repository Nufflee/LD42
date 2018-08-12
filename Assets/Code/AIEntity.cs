using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AIEntity : Entity
{
  public NavMeshAgent agent;

  private void Start()
  {
    healthBar = transform.Find("Canvas/HealthBar").GetComponent<Slider>();
    agent = GetComponent<NavMeshAgent>();
  }

  public override void Attack(Entity entity)
  {
    float damage = Random.Range(this.damage - 10, this.damage + 10);

    if (currentlyAttacking != null)
    {
      if (Vector3.Distance(currentlyAttacking.transform.position, transform.position) > 3.5f)
      {
        agent.SetDestination(currentlyAttacking.transform.position);
      }
      else
      {
        currentlyAttacking.Damage(damage);
      }

      return;
    }

    if (Vector3.Distance(entity.transform.position, transform.position) > 3.5f)
    {
      agent.SetDestination(entity.transform.position);
    }

    entity.Damage(damage);
    entity.isBeingAttacked = true;
    entity.onDeath += () => { currentlyAttacking = null; };

    currentlyAttacking = entity;
  }
}