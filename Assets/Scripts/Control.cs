using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField] private Transform hook;
    [SerializeField] private float hookSpeed = 5;

    void Start()
    {
        
    }

    void Update()
    {
        float hookVerticalMove = Input.GetAxisRaw("Vertical");

        hook.Translate(Vector2.up * hookVerticalMove * (hookSpeed * Time.deltaTime));
    }
}