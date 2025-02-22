using UnityEngine;

public class Playerlight : MonoBehaviour
{
    private CircleCollider2D cld;
    void Start()
    {
        cld = GetComponent<CircleCollider2D>();
    }
    public void StartShining()
    {
        cld.radius = 5f;
    }

    public void StopShining()
    {
        cld.radius = 1f;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            collision.gameObject.GetComponent<EnemyAI>().Blind();
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.gameObject.transform.position - transform.position) * 10);
        }
    }
}
