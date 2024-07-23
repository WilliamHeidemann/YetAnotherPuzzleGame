using UnityEngine;
using Random = UnityEngine.Random;

namespace Components
{
    public class Spinner : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(Vector3.back, Time.deltaTime * 10f);
        }
    }
}
