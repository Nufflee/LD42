using System;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
  public float Health { get; protected set; } = 100;
  public Action onDeath;
  public bool isBeingAttacked;
  public Army army;

  protected Slider healthBar;
  protected Entity currentlyAttacking;
  protected float damage = 15.0f;

  private bool isDead;

  public virtual void Attack(Entity entity)
  {
  }

  public void Damage(float damage)
  {
    Health -= damage;

    healthBar.value = Health;
    print(healthBar.value);

    if (Health <= 0)
    {
      onDeath?.Invoke();
      isDead = true;
      army.Remove(this);
      Destroy(gameObject);
    }
  }
}