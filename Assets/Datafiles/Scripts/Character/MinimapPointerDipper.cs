using UnityEngine;

public class MinimapPointerDipper : MonoBehaviour
{
    public MeshRenderer _pointer;
    public Light _light;

    public bool _enabled = true;

    public bool _meshDipper = true;
    public bool _lightDipper;

    public float _dipperDuration = 1f;

    private void Start()
    {
        if(gameObject.TryGetComponent<MeshRenderer>(out MeshRenderer _mesh))
        {
            if(_mesh!=null)
            {
                _meshDipper = true;
            }
        }

        if(gameObject.TryGetComponent<Light>(out Light _light))
        {
            if (_light != null)
            {
                _lightDipper = true;
            }
        }

        if(_meshDipper)
        {
            if(_pointer == null)
            {
                _pointer = GetComponent<MeshRenderer>();
            }
            InvokeRepeating("DipperEffect", 0, _dipperDuration);  
        }else if(_lightDipper)
        {
            _light = GetComponent<Light>();
            InvokeRepeating("DipperEffect", 0, _dipperDuration);
        }
    }

    private void DipperEffect()
    {
        if (_lightDipper)
        {
            _enabled = !_enabled;
            _light.enabled = _enabled;
        }
        else if (_meshDipper)
        {
            _enabled = !_enabled;
            _pointer.enabled = _enabled;
        }
    }
}
