using UnityEngine;

public class BubbleLauncher : MonoBehaviour
{
    [SerializeField] private GameObject         _bubblePrefab;
    [SerializeField] private Transform          _muzzle;
    [SerializeField] private float              _bubbleSpeed = 10;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBubble();
        }
    }

    private void ShootBubble()
    {
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float angle = Mathf.Atan2(mousePos.y - transform.position.y,mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.rotation = targetRotation;

        Vector2 forward = transform.TransformDirection(Vector2.left);
        Vector2 toOther = Vector3.Normalize(mousePos3D - transform.position);

        Bubble firedBubble = Instantiate(_bubblePrefab.GetComponent<Bubble>(), _muzzle.position, Quaternion.identity);
        firedBubble.OnInstantiate(toOther, _bubbleSpeed);
    }
}