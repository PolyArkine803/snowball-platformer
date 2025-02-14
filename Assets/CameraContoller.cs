using System.Numerics;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    public Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
         transform.position = new UnityEngine.Vector3(player.position.x, player.position.y, -1);
    }
}
