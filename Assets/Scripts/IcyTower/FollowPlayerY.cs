using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerY : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private Transform thisObject;
    private void Start()
    {
        thisObject = this.GetComponent<Transform>();
    }
    void Update()
    {
        thisObject.position = new Vector3(-4.05f, player.position.y, 0);
    }
}
