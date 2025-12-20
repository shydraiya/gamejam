using System.Collections;

using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private string colorProperty = "_BaseColor";
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.08f;

    private Material _mat;
    private Color _original;

    void Awake()
    {
        if (targetRenderer == null) targetRenderer = GetComponentInChildren<Renderer>();
        _mat = targetRenderer.material;
        _original = GetCurrentColor();
    }

    public void SetBaseColor(Color c)
    {
        _original = c;
        SetColor(c);
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(CoFlash());
    }

    IEnumerator CoFlash()
    {
        SetColor(flashColor);
        yield return new WaitForSeconds(flashDuration);
        SetColor(_original);
    }

    Color GetCurrentColor()
    {
        if (_mat.HasProperty(colorProperty)) return _mat.GetColor(colorProperty);
        if (_mat.HasProperty("_Color")) return _mat.GetColor("_Color");
        return Color.white;
    }

    void SetColor(Color c)
    {
        if (_mat.HasProperty(colorProperty)) _mat.SetColor(colorProperty, c);
        else if (_mat.HasProperty("_Color")) _mat.SetColor("_Color", c);
    }
}
