using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class SoliderController : MonoBehaviour
{
  public static List<Solider> soliders;
  private new Camera camera;
  
  private void Start()
  {
    soliders = FindObjectsOfType<Solider>().ToList();
    camera = Camera.main;
  }

  private void Update()
  {
    if (camera == null)
    {
      return;
    }
    
    Ray ray = camera.ScreenPointToRay(Input.mousePosition);

    RaycastHit hit;

    if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Ground")))
    {
      foreach (Solider solider in soliders)
      {
        solider.GetComponent<NavMeshAgent>().SetDestination(hit.point);
      }
    }
  }
}