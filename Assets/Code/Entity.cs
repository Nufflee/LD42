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

  private float lastDamage = -1;

  public virtual void Attack(Entity entity)
  {
  }

  private void Update()
  {
    if (Time.time - lastDamage > 2.5f)
    {
      isBeingAttacked = false;
    }
  }

  public void Damage(float damage)
  {
    Health -= damage;

    healthBar.value = Health;

    if (Health <= 0)
    {
      onDeath?.Invoke();
      army.Remove(this);
      Destroy(gameObject);
    }

    lastDamage = Time.time;
  }
}