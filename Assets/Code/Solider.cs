using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Solider : MonoBehaviour
{
  public float Health { get; private set; } = 100;
  public Action onDeath;

  private Enemy currentlyAttacking;
  private float damage = 22.0f;
  private Slider healthBar;
  private NavMeshAgent agent;
  private float lastAttack;

  private void Start()
  {
    healthBar = transform.Find("Canvas/HealthBar").GetComponent<Slider>();
    agent = GetComponent<NavMeshAgent>();

    //InvokeRepeating(nameof(Attack), 0.0f, 1.0f);
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0) && Time.time - lastAttack > 0.7f)
    {
      Attack();
      lastAttack = Time.time;
    }
  }

  private void Attack()
  {
    float damage = Random.Range(this.damage - 10, this.damage + 10);

    if (currentlyAttacking != null)
    {
      currentlyAttacking.Damage(damage);

      return;
    }

    List<Collider> colliders = Physics.OverlapSphere(transform.position, 5.0f, LayerMask.GetMask("Enemy")).Where((e) => e.GetComponent<Enemy>().isBeingAttacked == false).ToList();

    if (colliders.Count > 0)
    {
      colliders.Sort((a, b) => a.transform.position.magnitude > b.transform.position.magnitude ? 1 : a.transform.position.magnitude == b.transform.position.magnitude ? 0 : -1);

      Enemy enemy = colliders[0].GetComponent<Enemy>();

      if (Vector3.Distance(enemy.transform.position, transform.position) > 3.5f)
      {
        agent.SetDestination(enemy.transform.position);
      }

      enemy.Damage(damage);
      enemy.isBeingAttacked = true;
      enemy.onDeath += () => { currentlyAttacking = null; };

      currentlyAttacking = enemy;
    }
  }

  public void Damage(float damage)
  {
    Health -= damage;

    healthBar.value = Health;

    if (Health <= 0)
    {
      onDeath?.Invoke();
      Destroy(gameObject);
    }
  }
}