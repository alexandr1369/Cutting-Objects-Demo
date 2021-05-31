using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Animator scissorsAnimator;
    [SerializeField]
    private Vector2 yPositionBounds;
    [SerializeField]
    private DynamicJoystick joystick;

    public bool IsSlicing { get; private set; }

    private Rigidbody rb;
    private float movementSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementSpeed = 5f;
        IsSlicing = false;
    }
    private void Update()
    {
        // check for game over
        var sequence = DOTween.Sequence();
        if (Input.GetMouseButton(0) && !GameManager.instance.isGameFinished)
        {
            // start slicing
            if (!IsSlicing || !scissorsAnimator.GetBool("Cutting"))
            {
                IsSlicing = true;

                Transform scissorsTransform = scissorsAnimator.transform.parent;
                Vector3 newRotation = Vector3.zero;
                Vector3 newPosition = new Vector3(0, 0, -6f);
                float duration = .25f;
                sequence
                    .Append(scissorsTransform.DOLocalMove(newPosition, duration))
                    .Join(scissorsTransform.DOLocalRotate(newRotation, duration))
                    .OnStart(() => scissorsAnimator.SetBool("Cutting", true));
            }
        }
        else
        {
            // stop slicing
            if (IsSlicing)
            {
                IsSlicing = false;

                Transform scissorsTransform = scissorsAnimator.transform.parent;
                Vector3 newRotation = new Vector3(0, 0, 90f);
                Vector3 newPosition = new Vector3(5f, .5f, -.25f);
                float duration = .25f;
                sequence
                    .Append(scissorsTransform.DOLocalMove(newPosition, duration))
                    .Join(scissorsTransform.DOLocalRotate(newRotation, duration))
                    .OnComplete(() => scissorsAnimator.SetBool("Cutting", false));

                transform.DOMoveY(1.75f, duration);
            }
        }

        Vector3 currentPosition = transform.position;
        float yBoundedPosition = Mathf.Clamp(currentPosition.y, yPositionBounds.x, yPositionBounds.y);
        transform.position = new Vector3(currentPosition.x, yBoundedPosition, currentPosition.z);
    }
    private void FixedUpdate()
    {
        // move player forward
        rb.velocity = Vector3.forward * movementSpeed * GameManager.instance.AccelerationMultiplier;

        // move player on OY with touch
        if (IsSlicing)
        {
            float inputMovementSpeed = movementSpeed * 2;
            Vector3 direction = Vector3.up * joystick.Vertical;
            rb.velocity += direction * inputMovementSpeed;
        }
    }
}
