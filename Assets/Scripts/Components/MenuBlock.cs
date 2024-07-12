using UnityEngine;
using UnityUtils;
using UtilityToolkit.Editor;
using UtilityToolkit.Runtime;

namespace Components
{
    public class MenuBlock : MonoBehaviour
    {
        public Vector3 translation;

        [Button]
        public void MoveAllByTranslation()
        {
            var blocks = FindObjectsByType<MenuBlock>(FindObjectsSortMode.None);
            blocks.ForEach(b => b.transform.position += translation);
        }
        
        
        [Button]
        public void UpX()
        {
            transform.position += Vector3.right;
        }

        [Button]
        public void DownX()
        {
            transform.position += Vector3.left;
        }

        [Button]
        public void UpZ()
        {
            transform.position += Vector3.forward;
        }

        [Button]
        public void DownZ()
        {
            transform.position += Vector3.back;
        }

        [Button]
        public void UpY()
        {
            transform.position += Vector3.up;
        }

        [Button]
        public void DownY()
        {
            transform.position += Vector3.down;
        }
    }
}