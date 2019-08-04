using Assets.Scripts;
using UnityEngine;

public class Key : MonoBehaviour
{
    public float FloatPeriod;

    public float FloatDistance = 0.1f;

    private float _floatCounter = 0;
    private Vector3 _startPosition;

    public GameObject ActivatableGO;

    private Activatable _activatable;

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _activatable = ActivatableGO.GetComponent<Activatable>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        _floatCounter += 50 / FloatPeriod / 2;

        var position = transform.position;
        position.y = _startPosition.y + FloatDistance * Mathf.Sin(_floatCounter);
        transform.position = position;
    }

    public void Trigger()
    {
        //TODO: Do Special Effects
        _activatable.Activate();
        Destroy(gameObject);
    }
}
