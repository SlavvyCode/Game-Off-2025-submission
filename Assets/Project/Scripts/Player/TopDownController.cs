using Project.Scripts.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class TopDownController : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D _rb = null;
    public float speed = 12.5f;
    // public float turnRate = 200.0f;

    // private Camera mainCam;

    Vector2 targetMovePosition;


    
    public PlayerInput PlayerInput { get; private set; }    
    private InputAction _attackAction;
    private InputAction _jumpAction;
    private Vector2 movementDirection = Vector2.zero;
    Vector2 mousePosition;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        PlayerInput = GetComponent<PlayerInput>();
        // mainCam = Camera.main;
    }

    private void Start()
    {
        UIManager.instance.RegisterPlayer(this);

        var _controls = PlayerInput.actions;

        _controls["Movement"].canceled += OnMoveControls;
        _controls["Movement"].performed += OnMoveControls;
        _controls["Sneak"].started += ToggleSneak;
        _controls["Sneak"].canceled += ToggleSneak;

        _controls["Jump"].started += ctx => OnJump();
        // _controls.Player.Cursor.performed += ctx => mousePosition =
            // mainCam.ScreenToWorldPoint(new Vector3(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y, 0));
        
    }

    bool sneaking = false;
    float sneakMultiplier = .5f;
    private void ToggleSneak(InputAction.CallbackContext obj)
    {
        sneaking = !sneaking;
        if (sneaking)
            speed *= sneakMultiplier;
        else
            speed /= sneakMultiplier;
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
