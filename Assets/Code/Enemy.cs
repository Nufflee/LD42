using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DigitalRuby.Tween;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : AIEntity
{
  private float lastStop;
  private Vector3 castlePosition = Vector3.zero;
  private bool isLookingAround;
  private float lastAttack;

  private void Update()
  {
    agent.speed = Territory.isEnemyConquering ? 1.0f : 4.5f;

    if (Territory.isEnemyConquering)
    {
      agent.isStopped = true;
    }
    else
    {
      if (agent.isStopped)
      {
        SetDestination();
      }

      agent.isStopped = false;
    }

    if (Time.time - lastAttack > 0.7f)
    {
      if (currentlyAttacking)
      {
        Attack(currentlyAttacking);
      }
      else
      {
        List<Collider> colliders = Physics.OverlapSphere(transform.position, 20.0f, LayerMask.GetMask("Player")).ToList();

        colliders.Sort((a, b) => a.transform.position.magnitude > b.transform.position.magnitude ? 1 : a.transform.position.magnitude == b.transform.position.magnitude ? 0 : -1);

        foreach (Collider collider in colliders)
        {
          if (Vector3.Distance(transform.position, collider.transform.position) > 2.5f)
          {
            RaycastHit hit;

            if (Physics.Linecast(transform.position, collider.transform.position, out hit, LayerMask.GetMask("Obstacle")))
            {
              return;
            }

            agent.SetDestination(collider.transform.position);
          }
          else
          {
            army.Attack(collider.GetComponent<Entity>().army);
          }
        }
      }

      lastAttack = Time.time;
    }

    if (currentlyAttacking)
    {
      return;
    }

    if (agent.isStopped && !isLookingAround)
    {
      return;
    }

    float castleDistance = Vector3.Distance(transform.position, castlePosition);

    if (castleDistance < 3.0f)
    {
      agent.isStopped = true;

      return;
    }

    if (castleDistance < 15.0f)
    {
      agent.SetDestination(castlePosition);

      return;
    }

    if (Time.time - lastStop > 1.35f && isLookingAround)
    {
      agent.isStopped = false;
      isLookingAround = false;

      SetDestination();
    }

    if (EnemyController.shouldStop && (Time.time - lastStop > 7.5f || lastStop == 0) && !isLookingAround)
    {
      agent.isStopped = true;
      isLookingAround = true;

      lastStop = Time.time;

      StartCoroutine(LookAround());

      return;
    }

    if (agent.remainingDistance > 1.5f)
    {
      return;
    }

    SetDestination();
  }

  private void SetDestination()
  {
    Vector3 direction = new Vector3(Random.Range(1.5f, 2.5f), 0, Random.Range(1.5f, 2.5f)) + (transform.position - castlePosition + new Vector3(Random.Range(0.0f, 1.0f) * 10, 0, Random.Range(0.0f, 1.0f) * 10)).normalized * Random.Range(10.0f, 18.0f);

    agent.SetDestination(transform.position - direction);
  }

  private IEnumerator LookAround()
  {
    gameObject.Tween($"LookAround{EnemyController.GetId(this)}", transform.eulerAngles, transform.eulerAngles + new Vector3(0, 30, 0), 0.35f, TweenScaleFunctions.Linear, tween => { transform.eulerAngles = tween.CurrentValue; });

    yield return new WaitForSeconds(0.5f);

    gameObject.Tween($"LookAround{EnemyController.GetId(this)}-2", transform.eulerAngles, transform.eulerAngles + new Vector3(0, -60, 0), 0.7f, TweenScaleFunctions.Linear, tween => { transform.eulerAngles = tween.CurrentValue; });

    yield return new WaitForSeconds(0.85f);

    gameObject.Tween($"LookAround{EnemyController.GetId(this)}-3", transform.eulerAngles, transform.eulerAngles + new Vector3(0, 30, 0), 0.35f, TweenScaleFunctions.Linear, tween => { transform.eulerAngles = tween.CurrentValue; });
  }

  private void OnDestroy()
  {
    EnemyController.enemies.Remove(this);
  }
}