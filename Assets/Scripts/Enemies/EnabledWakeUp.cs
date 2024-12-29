using UnityEngine;

namespace Assets.Scripts.Enemies
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EnabledWakeUp : MonoBehaviour
    {

        public Skeleton skeleton;
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                skeleton.EnableWakeUp();
            }
        }
    }
}

