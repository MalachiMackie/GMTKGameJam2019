using UnityEngine;

public class Camera : MonoBehaviour
{

    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<Player>().gameObject;
        transform.position = Player.transform.position + new Vector3(0, 2, -3);
        transform.SetParent(Player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
