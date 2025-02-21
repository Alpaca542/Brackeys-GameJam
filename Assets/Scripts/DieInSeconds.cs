using UnityEngine;

public class DieInSeconds : MonoBehaviour
{
    public float seconds;

    void Start()
    {
        Invoke(nameof(Die), seconds);
    }
    
    public void Die()
    {
        Destroy(gameObject);
    }
}
