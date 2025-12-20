public interface IWeapon
{
    void SetActive(bool active);
    void Fire(); // 발사 트리거 (자동/수동 여부는 무기 내부에서 처리 가능)
}
