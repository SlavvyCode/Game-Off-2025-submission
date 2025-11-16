using System;
using System.Collections;
using Project.Scripts.Sound;
using Project.Scripts.UI;
using Project.Scripts.Util;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TopDownController : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody2D _rb = null;
    public float speed = 12.5f;
    // public float turnRate = 200.0f;
    public bool movementEnabled = true;
    
    private Cooldown screamCooldown = new Cooldown(5f);
    
    [SerializeField] private SoundData screamSound;

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
        // UIManager.instance.RegisterPlayer(this);
        string id = PlayerPrefs.GetString("currentCheckpointID", "");

        if (!string.IsNullOrEmpty(id))
        {
            float x = PlayerPrefs.GetFloat(id + "_x");
            float y = PlayerPrefs.GetFloat(id + "_y");

            transform.position = new Vector2(x, y);
        }

        var _controls = PlayerInput.actions;

        _controls["Movement"].canceled += OnMoveControls;
        _controls["ResetDebug"].started += DebugReset;
        _controls["Movement"].performed += OnMoveControls;
        _controls["Sneak"].started += ToggleSneak;
        _controls["Sneak"].canceled += ToggleSneak;
        _controls["Pause"].started += ctx => UIManager.instance.TogglePause();
        _controls["Scream"].started += Scream;
        // _controls.Player.Cursor.performed += ctx => mousePosition =
        //     mainCam.ScreenToWorldPoint(new Vector3(ctx.ReadValue<Vector2>().x, ctx.ReadValue<Vector2>().y, 0));
        //
    }

    private void DebugReset(InputAction.CallbackContext obj)
    {
        SaveUtil.ClearUserData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Scream(InputAction.CallbackContext obj)
    {
        if (!screamCooldown.IsReady())
            return;
        screamCooldown.Use();
        AudioManager.Instance.PlaySound(screamSound, transform.position);

        StartCoroutine(DisableMovementForSeconds(screamSound.clip.length));

    }

    private IEnumerator DisableMovementForSeconds(float f)
    {
        movementEnabled = false;
        yield return new WaitForSeconds(f);
        movementEnabled = true;
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
        if (!movementEnabled)
            return;
        if (movementDirection == Vector2.zero)
            return; // No movement input, exit early
        targetMovePosition = (Vector2)transform.position + movementDirection.normalized * speed * Time.deltaTime;
        _rb.MovePosition(targetMovePosition);
    }


    void FixedUpdate()
    {
        Move();
    }

}
