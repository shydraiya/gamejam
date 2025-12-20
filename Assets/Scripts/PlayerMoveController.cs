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
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
    
        const float dead = 0.01f;
        if (Mathf.Abs(h) < dead) h = 0f;
        if (Mathf.Abs(v) < dead) v = 0f;
    
        // 입력 없으면 완전 정지(물리 누적/미세 입력 방지)
        if (h == 0f && v == 0f)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            return;
        }
    
        float speed = stats != null ? stats.MoveSpeed : 6f;
    
        if (!isFPS)
        {
            Vector3 input = new Vector3(h, 0f, v);
            if (input.sqrMagnitude > 1f) input.Normalize();
            rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
            return;
        }
    
        if (!moveBasis) return;
    
        float fpsMoveSpeed = speed * 0.6f;
    
        Vector3 forward = moveBasis.forward; forward.y = 0f;
        Vector3 right = moveBasis.right; right.y = 0f;
    
        if (forward.sqrMagnitude < 0.0001f || right.sqrMagnitude < 0.0001f)
            return;
    
        forward.Normalize();
        right.Normalize();
    
        Vector3 move = forward * v + right * h;
        if (move.sqrMagnitude > 1f) move.Normalize();
    
        rb.MovePosition(rb.position + move * fpsMoveSpeed * Time.fixedDeltaTime);
    }

}
