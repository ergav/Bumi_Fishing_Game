using UnityEngine;

public class Control : MonoBehaviour
{
    [SerializeField] private Transform      rod;
    [SerializeField] private Rigidbody2D    hook;
    [SerializeField] private float          hookSpeed = 5;
    [SerializeField] private float          boatSpeed = 2.5f;

    [SerializeField] private float          maximumHookHeight = 0;
    [SerializeField] private float          minimumHookHeight = -10;

    private Rigidbody2D                     rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        float boatHorizontalMove = Input.GetAxisRaw("Horizontal");
        float hookVerticalMove = Input.GetAxisRaw("Vertical");

        Vector2 boatMove = Vector2.right * boatHorizontalMove * (boatSpeed * Time.fixedDeltaTime);
        Vector2 hookMove = new Vector2(boatMove.x ,hookVerticalMove * (hookSpeed * Time.fixedDeltaTime));

        rb.linearVelocity = boatMove;
        hook.linearVelocity = hookMove;

        //transform.Translate(Vector2.right * boatHorizontalMove * (boatSpeed * Time.deltaTime));
        //hook.Translate(Vector2.up * hookVerticalMove * (hookSpeed * Time.deltaTime));

        //if (hook.localPosition.y < minimumHookHeight)
        //{
        //    hook.localPosition = new Vector3(hook.localPosition.x, minimumHookHeight, hook.localPosition.z);
        //}

        Vector2 desiredHookPos = new Vector2(rod.position.x, hook.transform.position.y);
        Vector2 smoothHookPos = Vector2.Lerp(hook.transform.position, desiredHookPos, 0.1f);
        hook.transform.position = smoothHookPos;

        if (hook.transform.localPosition.y > maximumHookHeight)
        {
            hook.transform.localPosition = new Vector3(hook.transform.localPosition.x, maximumHookHeight, hook.transform.localPosition.z);
        }
    }
}