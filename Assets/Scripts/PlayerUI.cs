using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public List<GameObject> LevelTextTriggers = new List<GameObject>();

    public Text DisplayText;

    // Start is called before the first frame update
    void Start()
    {

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
