using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class playerFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    private Vector3 playerVector;
    public GameObject myUI;
    public int speed;
    public GameObject transition;
    void Start()
    {
        transition.transform.DOMove(new Vector3(93.6f, 2.2f, 0), 2f);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        playerVector = player.position;
        playerVector.z = -10;
        transform.position = Vector3.Lerp(transform.position, playerVector, speed * Time.deltaTime);
    }
}
