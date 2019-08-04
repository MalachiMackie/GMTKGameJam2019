using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public List<GameObject> LevelTextTriggers = new List<GameObject>();

    public Text DisplayText;

    public Player player;
    private Text DashUI;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TextTrigger")
        {
            DisplayText.text = other.gameObject.GetComponent<Text>().text;

            if (other.gameObject.name == "Tutorial_2_Trigger")
            {
                player.EnableDash();
                //DashUI = player.GetComponentInChildren<Text>();
                //DashUI.transform.position = new Vector3(75, 50, 0);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "TextTrigger")
        {
            DisplayText.text = "";
        }
    }
}
