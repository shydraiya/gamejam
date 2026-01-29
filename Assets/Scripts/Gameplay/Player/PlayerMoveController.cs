using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveController : MonoBehaviour
{
    public PlayerStats stats;

    [Header("References")]
    public Transform moveBasis; // FPS일 때 이동 기준(대개 Main Camera)

    [Header("Dash")]
    public KeyCode dashKey = KeyCode.LeftShift;
    public float dashMultiplier = 5.0f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 0.8f;

    Rigidbody rb;
    bool isFPS = false;

    // Dash state (여기에 둬야 프레임 넘어 유지됨)
    float dashEndTime = -1f;
    float nextDashTime = 0f;
    Vector3 lastMoveDir = Vector3.forward;
    Vector3 dashDir = Vector3.forward;
    bool dashPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.freezeRotation = true;
    }

    // CameraModeController에서 호출
    public void SetMode(bool fps, Transform fpsMoveBasis)
    {
        isFPS = fps;
        moveBasis = fpsMoveBasis;
    }

    void Update()
    {
        // 입력은 Update에서 받는 게 안전(고정 프레임 누락 방지)
        if (Input.GetKeyDown(dashKey))
            dashPressed = true;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        const float dead = 0.01f;
        if (Mathf.Abs(h) < dead) h = 0f;
        if (Mathf.Abs(v) < dead) v = 0f;

        // 기본 이동 방향 계산 (Top/FPS 공통)
        Vector3 moveDir = Vector3.zero;

        if (!isFPS)
        {
            moveDir = new Vector3(h, 0f, v);
        }
        else
        {
            if (!moveBasis) return;

            Vector3 forward = moveBasis.forward; forward.y = 0f;
            Vector3 right = moveBasis.right; right.y = 0f;

            if (forward.sqrMagnitude < 0.0001f || right.sqrMagnitude < 0.0001f)
                return;

            forward.Normalize();
            right.Normalize();

            moveDir = forward * v + right * h;
        }

        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        // 마지막 이동 방향 갱신(입력이 있을 때만)
        if (moveDir.sqrMagnitude > 0.0001f)
            lastMoveDir = moveDir.normalized;

        bool isDashing = Time.time < dashEndTime;

        // 대쉬 시작 (고정 방향: 시작 시점의 방향으로 끝까지)
        if (dashPressed)
        {
            dashPressed = false;

            if (!isDashing && Time.time >= nextDashTime)
            {
                dashDir = (moveDir.sqrMagnitude > 0.0001f) ? moveDir.normalized : lastMoveDir;
                dashEndTime = Time.time + dashDuration;
                nextDashTime = Time.time + dashCooldown;
                isDashing = true;

                // 무적 시작
                if (stats != null) stats.SetInvulnerable(true);
            }
        }

        // 대쉬 종료 시 무적 해제
        if (!isDashing && stats != null && stats.Invulnerable)
        {
            stats.SetInvulnerable(false);
        }

        // 입력도 없고 대쉬도 아니면 완전 정지
        if (!isDashing && moveDir.sqrMagnitude < 0.0001f)
        {
            StopRigidbody(rb);
            return;
        }

        float baseSpeed = stats != null ? stats.MoveSpeed : 6f;
        float speed = baseSpeed * (isFPS ? 0.6f : 1f);

        Vector3 finalDir = isDashing ? dashDir : moveDir;
        float finalSpeed = isDashing ? speed * dashMultiplier : speed;

        rb.MovePosition(rb.position + finalDir * finalSpeed * Time.fixedDeltaTime);
    }

    void OnDisable()
    {
        // 비활성화될 때 무적이 남지 않게 안전장치
        if (stats != null) stats.SetInvulnerable(false);
    }

    static void StopRigidbody(Rigidbody rb)
    {
#if UNITY_6000_0_OR_NEWER
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
#else
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
#endif
    }
}
