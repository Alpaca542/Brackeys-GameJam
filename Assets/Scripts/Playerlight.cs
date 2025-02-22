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
        cld.gameObject.SetActive(true);
    }

    public void StopShining()
    {
        cld.gameObject.SetActive(true);
    }
}
