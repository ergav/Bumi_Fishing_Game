using UnityEngine;

public class FishController : MonoBehaviour
{
    [SerializeField] private float _speed = 2;

    private Vector3 _velocity;

    void Start()
    {
        _velocity = -transform.right * _speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movement = Time.fixedDeltaTime * _speed * _velocity.normalized;

        transform.position += movement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fish"))
        {
            Reverse(); // Reverse direction if colliding with something other than another fish
        }
    }

    void Reverse()
    {
        transform.forward *= -1; // Flip the fish's forward direction
        _velocity *= -1f; // Reverse the velocity
    }
}
