using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Solider : Entity
{
  public NavMeshAgent agent;
  private float lastAttack;

  private void Start()
  {
    healthBar = transform.Find("Canvas/HealthBar").GetComponent<Slider>();
    agent = GetComponent<NavMeshAgent>();

    damage = 25.0f;

    onDeath += () =>
    {
      SoliderController.soliders.Remove(this);

      if (currentlyAttacking)
      {
        currentlyAttacking.isBeingAttacked = false;
      }
    };
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0) && Time.time - lastAttack > 0.5f)
    {
      Attack();
      lastAttack = Time.time;
    }

    agent.speed = Territory.isPlayerConquering ? 1.0f : 4.5f;
  }

  private void Attack()
  {
    float damage = Random.Range(this.damage - 10, this.damage + 10);

    if (currentlyAttacking != null)
    {
      currentlyAttacking.Damage(damage);

      return;
    }

    List<Collider> colliders = Physics.OverlapSphere(transform.position, 3.0f, LayerMask.GetMask("Enemy")).Where((e) => e.GetComponent<Enemy>().isBeingAttacked == false).ToList();

    if (colliders.Count > 0)
    {
      colliders.Sort((a, b) => a.transform.position.magnitude > b.transform.position.magnitude ? 1 : a.transform.position.magnitude == b.transform.position.magnitude ? 0 : -1);

      Enemy enemy = colliders[0].GetComponent<Enemy>();

      enemy.Damage(damage);
      enemy.isBeingAttacked = true;
      enemy.onDeath += () => { currentlyAttacking = null; };

      currentlyAttacking = enemy;
    }
  }
}