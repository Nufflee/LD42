using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
  public float Health { get; private set; } = 100;
  public Action onDeath;
  public bool isBeingAttacked;

  private Slider healthBar;

  private void Start()
  {
    healthBar = transform.Find("Canvas/HealthBar").GetComponent<Slider>();
  }

  public void Damage(float damage)
  {
    Health -= damage;

    healthBar.value = Health;
    print(healthBar.value);

    if (Health <= 0)
    {
      onDeath?.Invoke();
      Destroy(gameObject);
    }
  }
}