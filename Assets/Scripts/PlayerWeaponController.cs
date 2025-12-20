using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Refs")]
    public Camera mainCam;
    public CameraModeController camMode; // IsFPS 제공
    public Transform player;             // 보통 자기 자신
    public float groundY = 0f;           // 탑뷰 바닥 높이

    [Header("Weapons")]
    public MonoBehaviour topViewWeapon;  // IWeapon 구현체를 드래그(Shotgun 등)
    public MonoBehaviour fpsWeapon;      // IWeapon 구현체를 드래그(스나이퍼 등)

    [Header("Input")]
    public KeyCode fireKey = KeyCode.Mouse0;

    public Vector3 AimDirection { get; private set; } = Vector3.forward;

    IWeapon topW;
    IWeapon fpsW;

    void Awake()
    {
        if (!player) player = transform;
        if (!mainCam) mainCam = Camera.main;

        topW = topViewWeapon as IWeapon;
        fpsW = fpsWeapon as IWeapon;

        if (topW != null) topW.SetOwner(this);
        if (fpsW != null) fpsW.SetOwner(this);
    }

    void Update()
    {
        if (!mainCam || !camMode) return;

        bool isFPS = camMode.IsFPS;

        // 무기 활성 상태 관리(선택)
        if (topW != null) topW.SetActive(!isFPS);
        if (fpsW != null) fpsW.SetActive(isFPS);

        // 조준 방향 갱신
        AimDirection = isFPS ? GetFPSAimDirection() : GetTopViewMouseAimDirection();

        // 발사
        if (Input.GetKey(fireKey))
        {
            var w = isFPS ? fpsW : topW;
            if (w != null) w.Fire();
        }
    }

    public Vector3 GetFPSAimDirection()
    {
        // FPS에서는 카메라 정면(피치 포함)
        Vector3 dir = mainCam.transform.forward;
        if (dir.sqrMagnitude < 0.0001f) dir = player.forward;
        return dir.normalized;
    }

    public Vector3 GetTopViewMouseAimDirection()
    {
        // 탑뷰에서는 마우스가 가리키는 바닥 점을 기준으로 방향 계산
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0f, groundY, 0f));

        if (!ground.Raycast(ray, out float enter))
            return player.forward.normalized;

        Vector3 hitPoint = ray.GetPoint(enter);
        Vector3 dir = hitPoint - player.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.0001f)
            return player.forward.normalized;

        return dir.normalized;
    }
}
