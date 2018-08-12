using System.Linq;
using UnityEngine;

public class Minimap : MonoBehaviour
{
  private void LateUpdate()
  {
    if (SoliderController.soliders.Count == 0)
    {
      return;
    }
    
    Vector3 mean = SoliderController.soliders.Select((s) => s.transform.position).Aggregate((a, b) => a + b) / SoliderController.soliders.Count;

    transform.position = new Vector3(mean.x, transform.position.y, mean.z);
  }
}