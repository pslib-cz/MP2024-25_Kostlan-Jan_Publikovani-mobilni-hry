using System.Collections;
using UnityEngine;

/// <summary>
/// Basic bullet with rigidbody and autodestroy.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    //proměnný ke střelbe
    public float speed = 20f;
    [SerializeField]public float timeDestroy = 8;
    public Rigidbody2D rb;

    /// <summary>
    /// When summon.
    /// </summary>
    void Start()
    {
        //získání rigidbody
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.right * speed;
        StartCoroutine(ExampleCoroutine());
    }
    
    /// <summary>
    /// After x time destoy this object.
    /// </summary>
    IEnumerator ExampleCoroutine()
    {
        //čeká se 10 vteřin před tím, než se zníčí bullet.
        yield return new WaitForSeconds(timeDestroy);
        Destroy(gameObject);
    }
    
    /// <summary>
    /// When collison with someone diffrent, destoy yourself.
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Neudělá nic, pokud se dotkne objektu s tagem "Indestructible"
        if (collision.gameObject.tag == "Indestructible")
            return;
        // Zničí také střelu
        Destroy(gameObject);
    }
}
