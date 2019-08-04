using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    private Text DashText;

    // Start is called before the first frame update
    void Start()
    {
        var texts = GetComponentsInChildren<Text>();

        foreach (var text in texts)
        {
            if (text.gameObject.name == "DashUI")
            {
                DashText = text;
            }
        }

        DashText.fontSize = 30;
        DashText.transform.position = new Vector3(75, 50, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableDash()
    {
        DashText.enabled = true;
    }

    public void DisableDash()
    {
        DashText.enabled = false;
    }

    public void StartDash()
    {
        DashText.color = Color.grey;
    }

    public void DashLoaded()
    {
        
        StartCoroutine(DashFlash());
    }

    private IEnumerator DashFlash()
    {
        DashText.color = Color.white;
        DashText.fontSize += 10;
        yield return new WaitForSeconds(0.1f);
        DashText.fontSize -= 10;
    }
}
