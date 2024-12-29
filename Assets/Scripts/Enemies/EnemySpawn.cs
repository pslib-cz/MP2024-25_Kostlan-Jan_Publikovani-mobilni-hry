using UnityEngine;

/// <summary>
/// Když se hráče dotkne boxu, spawne nepřítele.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    private bool firstTime = true;
    void OnTriggerEnter2D(Collider2D collider)
    { 
        if (collider.gameObject.CompareTag("Player"))
        {
            if (firstTime)
            {
                enemy.SetActive(true);
                firstTime = false;
            }
        }
    }
}