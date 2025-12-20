using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMoveController : MonoBehaviour
{
    public PlayerStats stats;

    [Header("References")]
    public Transform moveBasis; // FPS일 때 이동 기준(대개 Main Camera)

    Rigidbody rb;
    bool isFPS = false;

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

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal"); // A/D
        float v = Input.GetAxisRaw("Vertical");   // W/S

        if (!isFPS)
        {
            float topMoveSpeed = stats.MoveSpeed;
            // 탑뷰: 월드 기준 이동
            Vector3 input = new Vector3(h, 0f, v);
            if (input.sqrMagnitude > 1f) input.Normalize();
            rb.MovePosition(rb.position + input * topMoveSpeed * Time.fixedDeltaTime);
            return;
        }

        // FPS: 카메라 방향 기준 이동 (키보드는 회전/시점에 영향 없음)
        if (!moveBasis) return;
        float fpsMoveSpeed = stats.MoveSpeed * 0.6f;
        Vector3 forward = moveBasis.forward; forward.y = 0f; forward.Normalize();
        Vector3 right = moveBasis.right; right.y = 0f; right.Normalize();

        Vector3 move = forward * v + right * h;
        if (move.sqrMagnitude > 1f) move.Normalize();

        rb.MovePosition(rb.position + move * fpsMoveSpeed * Time.fixedDeltaTime);
    }
}
