using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Player.transform.position + new Vector3(0, 2, -3);
        transform.SetParent(Player.transform);
        //transform.position = new Vector3(2.418f, 3.967f, -25.601f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
