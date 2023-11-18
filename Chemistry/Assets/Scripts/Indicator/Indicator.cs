using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Indicator : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private IndicatorElementData _data;

    [SerializeField] private TextMeshProUGUI _elementNameText;

    [SerializeField] private GameObject _pipette;
    
    private bool _isDragging;
    private bool _isTryFilled;
    private Camera _mainCamera;
    private Vector2 offset;
    private Vector3 _initPipettePosition;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _initPipettePosition = transform.position;
        _elementNameText.text = _data.indicatorName.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        _isTryFilled = false;

        offset = _pipette.transform.position - _mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 
            _mainCamera.WorldToScreenPoint(transform.position).z));
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (_isDragging)
        {
            Vector3 currentMousePosition = _mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, _mainCamera.WorldToScreenPoint(transform.position).z));
            _pipette.transform.position = new Vector3(currentMousePosition.x + offset.x, currentMousePosition.y + offset.y, _pipette.transform.position.z);
            
            Ray ray = _mainCamera.ScreenPointToRay(eventData.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out Tube tubeComponent))
                {
                    //tubeComponent.TryFilledTube(_data, _pipette.transform, _initPipettePosition);
                    tubeComponent.TryFilledTubeWithIndicator(_data, _pipette.transform,_initPipettePosition);
                    _isDragging = false;
                    _isTryFilled = true;
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(_isTryFilled) return;
        _isDragging = false;
        _pipette.transform.position = _initPipettePosition;
    }
}
