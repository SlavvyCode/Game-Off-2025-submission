using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownController : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D _rb = null;
    public float speed = 12.5f;
    // public float turnRate = 200.0f;

    private bool dashing = false;

    private Camera mainCam;



    Vector2 targetMovePosition;


    
    private PlayerInput _playerInput;
    private InputAction _attackAction;
    private InputAction _jumpAction;
    // private InputAction _clickAction;
    // private InputAction _lookAction;
    // private InputAction _pointAction;
    private Vector2 movementDirection = Vector2.zero;
    Vector2 mousePosition;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        mainCam = Camera.main;
    }

    private void Start()
    {
        var _controls = _playerInput.actions;

        _controls["Movement"].canceled += OnMoveControls;
        _controls["Movement"].performed += OnMoveControls;

        _controls["Jump"].started += ctx => OnJump();
        // _controls.Player.Cursor.performed += ctx => mousePosition =
            // mainCam.ScreenToWorldPoint(new Vector3(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y, 0));
        
    }

    private void OnJump()
    {
        //todo jump
        Debug.Log("Jump pressed");
    }

    void OnDestroy()
    {
    }

    //on movecontrols
    public void OnMoveControls(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
    }


    public void Move()
    {
        if (movementDirection == Vector2.zero)
            return; // No movement input, exit early
        targetMovePosition = (Vector2)transform.position + movementDirection.normalized * speed * Time.deltaTime;
        _rb.MovePosition(targetMovePosition);
    }


    void Update()
    {
        Move();
    }

}
