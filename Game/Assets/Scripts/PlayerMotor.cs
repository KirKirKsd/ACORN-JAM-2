using UnityEngine;

public class PlayerMotor : MonoBehaviour {

    // input
    private Controls actions;
    private Controls.PlayerActions playerActions;

    [Header("Movement")]
    [SerializeField] private float speed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feet;

    // components
    private Rigidbody2D rb;

    private void Awake() {
        // ����� ��������
        actions = new Controls();
        playerActions = actions.Player;

        // ����������� ������� ��� �� ��� ����������� ��� ������� �� ������
        playerActions.Jump.performed += _ => Jump();
    }

    private void OnEnable() {
        // ��� ��������� ��������� ���������� � ����� ����������
        playerActions.Enable();
    }

    private void Start() {
       // ������� ����������
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        Movement();
    }

    // �������� ���������
    private void Movement() {
        var input = playerActions.Movement.ReadValue<float>(); // ������� �������� ���������� ��� X
        var velocity = new Vector2(input * speed, rb.velocity.y); // ����� :Vector2 (x = �������� ���������� X * ��������, y = ���� ������ �� ��� Y)
        rb.velocity = velocity; // ����� ������ ��������
    }

    // ������ ���������
    private void Jump() {
        if (!IsGrounded()) return; // ���� �������� ��������� �� �� ����� �� ������� �� �����������

        var velocity = new Vector2(rb.velocity.x, jumpForce); // ����� :Vector2 (x = ���� ������ �� ��� X, y = ���� ������)
        rb.velocity = velocity; // ����� ������ ��������
    }

    // �������� ��������� �� �������� �� �����
    private bool IsGrounded() {
        return Physics2D.OverlapCircle(feet.position, 0.05f, groundLayer); // ��������� � ���������� :bool ��������� �� � ������� ��� �����
    }

    private void OnDisable() {
        // ��� ���������� ��������� ����������� � ����� ����������
        playerActions.Disable();
    }

}