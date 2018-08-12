using System.Linq;
using UnityEngine;

public class CameraController : MonoBehaviour
{
  private Vector3 offset;

  private void Start()
  {
    offset = transform.position - SoliderController.soliders.Select((s) => s.transform.position).Aggregate((a, b) => a + b) / SoliderController.soliders.Count;
  }

  private void Update()
  {
    if (SoliderController.soliders.Count == 0)
    {
      return;
    }

    Vector3 mean = SoliderController.soliders.Select((s) => s.transform.position).Aggregate((a, b) => a + b) / SoliderController.soliders.Count;

    transform.position = mean + offset;
  }
}