using UnityEngine;

namespace CGModifiers
{
    public class DisableOnStart : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
            Destroy(this);
        }
    }
}