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

    [Header("Top->FPS Aim")]
    public LayerMask groundMask;          // 바닥 레이어(예: Ground)
    public float defaultFpsPitch = 0f;    // 전환 시 기본 pitch

    Camera cam;
    bool isFPS = false;
    float pitch = -5f;

    bool TryGetMouseWorldPoint(out Vector3 point)
    {
        point = default;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // 1) 바닥 콜라이더가 있으면 그걸 맞추는 게 가장 안정적
        if (Physics.Raycast(ray, out RaycastHit hit, 500f, groundMask, QueryTriggerInteraction.Ignore))
        {
            point = hit.point;
            return true;
        }

        // 2) 바닥 콜라이더가 없다면, 수평 Plane(y = player.y)로 계산
        if (player != null)
        {
            Plane plane = new Plane(Vector3.up, new Vector3(0f, player.position.y, 0f));
            if (plane.Raycast(ray, out float enter))
            {
                point = ray.GetPoint(enter);
                return true;
            }
        }

        return false;
    }

    void SwitchToFPSUsingMouseAim()
    {
        // 마우스가 가리키는 월드 포인트를 얻고
        if (TryGetMouseWorldPoint(out Vector3 worldPoint))
        {
            Vector3 dir = worldPoint - player.position;
            dir.y = 0f; // 수평 회전만
            if (dir.sqrMagnitude > 0.0001f)
            {
                player.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            }
        }

        // pitch는 전환 시 기본값으로
        pitch = Mathf.Clamp(defaultFpsPitch, -80f, 80f);

        // 카메라 회전 동기화(즉시 자연스럽게)
        Quaternion yawRot = Quaternion.Euler(0f, player.eulerAngles.y, 0f);
        Quaternion pitchRot = Quaternion.Euler(pitch, 0f, 0f);
        transform.rotation = yawRot * pitchRot;
    }


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
            Debug.Log($"Camera mode changed. isFPS={isFPS}");

            if (isFPS)
            {
                // 전환 직전: TopView의 마우스 위치 기반으로 시점 맞추기
                SwitchToFPSUsingMouseAim();
                ApplyFPS();
            }
            else
            {
                ApplyTopView();
            }

            if (playerMove)
                playerMove.SetMode(isFPS, transform);
        }

        if (isFPS)
        {
            FPSLook();
            transform.position = player.position + fpsOffset;

            // if (Input.GetKeyDown(fireKey)){
            //     FireRay();
            //     Debug.Log("sniping!");
            // }
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
        Debug.Log("fire 2");
        // 디버그용 레이 시각화(씬 뷰에서 보임)
        Debug.DrawRay(origin, dir * shootRange, Color.yellow, 0.5f);

        if (Physics.Raycast(origin, dir, out RaycastHit hit, shootRange, hitMask, QueryTriggerInteraction.Ignore))
        {
            Debug.Log($"HIT: {hit.collider.name} at {hit.point}");

            Enemy enemy = hit.collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, isFPS);
            }

            // 2) “강한 적”만 약점(WeakSpot) 맞춰야 데미지 주는 식으로 확장 가능
            // 예: hit.collider.CompareTag("WeakSpot") 체크 등
        }
    }

}
