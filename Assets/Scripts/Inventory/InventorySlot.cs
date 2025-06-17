using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler
{
    public Item Item;

    public IInventory InventoryReference;
    public int Index;

    private InventorySlot _targetSlot;
    private Vector3 _originalPosition;
    private Image _image;
    [SerializeField] private Sprite _emptyImage;
    private Outline _outline;
    private Color _prevColor;
    private bool _isOutlineUsed;

    [SerializeField] private GameObject _dragClone;
    [SerializeField] private Canvas _parentCanvas;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
        _parentCanvas = transform.parent.GetComponentInParent<Canvas>();

        _dragClone = _parentCanvas.transform.Find("DragClone").gameObject;
    }

    public void SetOutline(Color color)
    {
        _isOutlineUsed = _outline.enabled;
        if (_isOutlineUsed) _prevColor = _outline.effectColor;

        _outline.effectColor = color;
        _outline.enabled = true;
    }

    public void DisableOutline()
    {
        if (_isOutlineUsed) _outline.effectColor = _prevColor;
        _outline.enabled = _isOutlineUsed;
        _isOutlineUsed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Item == null) return;


        _originalPosition = transform.position;

        _image.enabled = false;

        if (_dragClone.activeSelf == false)
        {
            _dragClone.SetActive(true);

            _dragClone.GetComponent<RectTransform>().sizeDelta = transform.parent.GetComponent<RectTransform>().sizeDelta;
            _dragClone.GetComponent<Image>().sprite = _image.sprite;
        }

        _dragClone.transform.position = eventData.position;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (Item == null) return;
        _image.enabled = true;
        DisableOutline();
        _dragClone.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_dragClone != null)
            _dragClone.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Item == null) return;
        _dragClone.SetActive(false);

        _targetSlot = null;
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var slot = result.gameObject.GetComponent<InventorySlot>();
            if (slot != null && slot != this)
            {
                _targetSlot = slot;
                break;
            }
        }

        if (_targetSlot != null)
        {
            IInventory sourceInventory = InventoryReference;
            int sourceIndex = Index;
            IInventory targetInventory = _targetSlot.InventoryReference;
            int targetIndex = _targetSlot.Index;

            if (sourceInventory == targetInventory)
            {
                sourceInventory.SwapItems(sourceIndex, targetIndex);
            }
            else
            {
                Item sourceItem = sourceInventory.Items[sourceIndex];
                Item displacedItem = targetInventory.SetItem(sourceItem, targetIndex);
                sourceInventory.SetItem(displacedItem, sourceIndex);
            }
        }

        transform.position = _originalPosition;
    }


    public void UpdateSlotImage()
    {
        _image.sprite = Item != null ? Item.ImageSprite : _emptyImage;
    }
}