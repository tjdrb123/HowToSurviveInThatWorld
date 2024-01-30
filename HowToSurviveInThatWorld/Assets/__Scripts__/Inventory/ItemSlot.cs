using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ItemSlot : UI_Base, IPointerDownHandler, IPointerUpHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    enum E_Images
    {
        ItemImage,
    }
    enum E_QuantityText
    {
        ItemQuantity,
    }
    
    private int _slotIndex; //슬롯의 번호
    private ItemData _itemData; //아이템의 Data를 슬롯이 관리하게함
    private Image _itemImage;              
    private GameObject _dragSlot;
    private DragSlot _dragSlotComponent;
    private TextMeshProUGUI _quantityText; //슬롯 
    private E_ItemType _slotType; //슬롯의 타입

    public int SlotIndex { get => _slotIndex; } //해당 슬롯의 번호를 전달
    public E_ItemType SlotType { get => _slotType; }
    public ItemData ItemData { get => _itemData; set { _itemData = value; } }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindImage(typeof(E_Images));
        BindText(typeof(E_QuantityText));
        _itemImage = GetImage((int)E_Images.ItemImage);
        _quantityText = GetText((int)E_QuantityText.ItemQuantity);
        _dragSlot = GameObject.Find("DragCanvas").FindChild("DragSlot"); //Find말고 어떤식으로 DragSlot을 찾을지 생각하기
        _dragSlotComponent = _dragSlot.GetComponent<DragSlot>();
        return true;
    }

    public void SetInfo(ItemData itemData, int index, int slotType = -1) //인벤토리가 활성화 되면 Data를 전달받아 Slot 셋팅을 다시함
    {
        _slotIndex = index;
        _slotType = (E_ItemType)slotType;
        ItemData = itemData;
        Swap();
        SetAlpha(_itemImage.sprite != null);
    }
    private void SlotClear() //슬롯을 클리어 해주는 함수입니다.
    {
        _itemImage.sprite = null;
        _quantityText.text = string.Empty;
    }
    private void SlotSetting() 
    {
        _itemImage.sprite = Resources.Load<Sprite>(ItemData.name);
        _quantityText.text = (ItemData.keyNumber != 0) ? ItemData.stack.ToString() : string.Empty; //아이템 Data의 수량에 맞게 Text변경
    }
    private void SetAlpha(bool isAlphaColor) //인벤토리의 아이템이 없으면 이미지의 Color값을 변경함
    {
        Color color = _itemImage.color;
        color.a = isAlphaColor ? 1 : 0;
        _itemImage.color = color;
    }
    public void Swap() //아이템의 이미지와 Sprite를 변경함 1. Data로 이미지를 불러올 거기 때문에 매개변수를 뺼 준비하기
    {
        SetAlpha(ItemData.keyNumber != 0);
        if (ItemData.keyNumber == 0)
        {
            SlotClear();
            return;
        }
        SlotSetting();
    }
    public void AddItem(ItemData item) //아이템을 Slot의 추가하기 위한 함수
    {
        ItemData = item;
        SlotSetting();
        SetAlpha(_itemImage.sprite != null);
    }
    public void UseItem(ItemData item) //우클릭시 아이템을 하나씩 사라지게 만들었음 
    {
        ItemData.stack -= 1; // 아이템의 수량을 하나 줄입니다.
        SlotSetting();
        if (ItemData.stack <= 0) // 수량이 0 이하라면 아이템을 인벤토리에서 제거합니다.
        {
            SlotClear();
            SetAlpha(ItemData.keyNumber != 0);
        }
    }
    public int MaxStackCheck(int stack) //아이템의 수량이 최대 수량보다 높게 합쳐지지 않게하기 위한 함수
    {
        ItemData.stack += stack;
        if (ItemData.stack > ItemData.maxStack)
        {
            int returnValue = ItemData.stack - ItemData.maxStack;
            ItemData.stack = ItemData.maxStack;
            _quantityText.text = ItemData.stack.ToString();
            return returnValue;
        }
        else
        {
            _quantityText.text = ItemData.stack.ToString();
            return 0;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_itemImage.sprite != null && _dragSlot != null)
        {
            _dragSlot.SetActive(true);
            _dragSlot.transform.position = transform.position;
            _dragSlotComponent.SetSlot(_itemImage, _quantityText, this);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_dragSlotComponent.slot != null)
        {
            _dragSlot.transform.position = eventData.position;
        }
    }
    public void OnDrop(PointerEventData eventData) //아이템의 스왑이 이뤄지는 공간 
    {
        Inventory inventory = this.GetComponentInParent<Inventory>();
        if (_dragSlotComponent != null && _dragSlotComponent.slot != null && inventory != null && this != _dragSlotComponent.slot)
        {
            if (this._slotType == E_ItemType.None) 
            {
                inventory.SlotSwap(this, _dragSlotComponent.slot);
            }
            else if ((E_ItemType)_dragSlotComponent.slot.ItemData.itemBaseType == this._slotType)
            {
                inventory.SlotSwap(this, _dragSlotComponent.slot);
            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _dragSlot.SetActive(false);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        _dragSlot.SetActive(false);
    }
}
