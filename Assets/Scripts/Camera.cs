using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent(Player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
