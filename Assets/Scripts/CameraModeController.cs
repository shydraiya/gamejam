using TMPro;

using Unity.Mathematics;

using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraModeController : MonoBehaviour
{
    public Transform player;
    public PlayerMoveController playerMove;

    public bool IsFPS => isFPS;

    [Header("Toggle Key")]
    public KeyCode toggleKey = KeyCode.Space;

    [Header("Top View")]
    public float topHeight = 10f;
    public float topOrthoSize = 10f;

    [Header("FPS View")]
    public Vector3 fpsOffset = new Vector3(0f, 1.6f, 0f);
    public float fpsFov = 60f;
    public float mouseSensitivity = 2f;

    [Header("Sniping")]
    public GameObject crosshairUI;          // 중앙 조준선(Image)
    public float shootRange = 100f;
    public float damage = 50f;
    public LayerMask hitMask = ~0;          // 기본: 전부 맞음
    public KeyCode fireKey = KeyCode.Mouse0; // 좌클릭


    Camera cam;
    bool isFPS = false;
    float pitch = 0f;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        ApplyTopView();
        if (playerMove) playerMove.SetMode(false, transform); // basis는 아무거나 상관 없음(탑뷰에서 안 씀)
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            isFPS = !isFPS;
            if (isFPS) ApplyFPS();
            else ApplyTopView();

            if (playerMove)
                playerMove.SetMode(isFPS, transform); // FPS 이동 기준은 "카메라(transform)"
        }

        if (isFPS)
        {
            FPSLook();
            transform.position = player.position + fpsOffset;

            if (Input.GetKeyDown(fireKey))
                FireRay();
        }
    }

    void LateUpdate()
    {
        if (!player) return;

        if (!isFPS)
        {
            // 탑뷰: 플레이어 따라가기
            transform.position = new Vector3(player.position.x, topHeight, player.position.z);
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else
        {
            // FPS: 플레이어 머리 위치로 붙기 + pitch 유지
            transform.position = player.position + fpsOffset;

            // // yaw는 플레이어가 갖고 있으므로, 카메라는 플레이어 yaw를 따라가야 함
            // // (카메라를 자식으로 안 두는 대신, 여기서 yaw를 동기화)
            // float yaw = player.eulerAngles.y;
            // transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }

    void ApplyTopView()
    {
        cam.orthographic = true;
        cam.orthographicSize = topOrthoSize;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pitch = 0f;

        if (crosshairUI) crosshairUI.SetActive(false);
    }

    void ApplyFPS()
    {
        cam.orthographic = false;
        cam.fieldOfView = fpsFov;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pitch = 0f;
        if (crosshairUI) crosshairUI.SetActive(true);
    }

    void FPSLook()
    {
        float mx = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float my = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // yaw를 누적 방식으로 관리하는 게 안전합니다(플레이어 회전값을 직접 더하는 것도 OK)
        player.Rotate(Vector3.up * mx, Space.World);

        pitch -= my;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        // 핵심: yaw(플레이어) * pitch(카메라)
        Quaternion yawRot = Quaternion.Euler(0f, player.eulerAngles.y, 0f);
        Quaternion pitchRot = Quaternion.Euler(pitch, 0f, 0f);
        transform.rotation = yawRot * pitchRot;
    }

    void FireRay()
    {
        // 카메라 정중앙 기준
        Vector3 origin = transform.position;
        Vector3 dir = transform.forward;

        // 디버그용 레이 시각화(씬 뷰에서 보임)
        Debug.DrawRay(origin, dir * shootRange, Color.yellow, 0.5f);

        if (Physics.Raycast(origin, dir, out RaycastHit hit, shootRange, hitMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"HIT: {hit.collider.name} at {hit.point}");

            // 1) 가장 단순한 데미지 처리: Health 컴포넌트가 있으면 깎기
            var health = hit.collider.GetComponentInParent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // 2) “강한 적”만 약점(WeakSpot) 맞춰야 데미지 주는 식으로 확장 가능
            // 예: hit.collider.CompareTag("WeakSpot") 체크 등
        }
    }

}
