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
    
    public UI_Inventory _inventory;
    private int _slotIndex; //슬롯의 번호
    private ItemDataSo _itemData; //아이템의 Data를 슬롯이 관리하게함
    private Image _itemImage;              
    private GameObject _dragSlot;
    private DragSlot _dragSlotComponent;
    private TextMeshProUGUI _quantityText; //슬롯 
    private bool _isLock; //슬롯 잠금
    private E_SlotType _slotType; //슬롯의 타입

    public int SlotIndex { get => _slotIndex; } //해당 슬롯의 번호를 전달
    public E_SlotType SlotType { get => _slotType; }
    public ItemDataSo ItemData { get => _itemData; set { _itemData = value; } }
    public Sprite ItemImage { get => _itemImage.sprite;}
    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindImage(typeof(E_Images));
        BindText(typeof(E_QuantityText));
        _itemImage = GetImage((int)E_Images.ItemImage);
        _quantityText = GetText((int)E_QuantityText.ItemQuantity);
        _dragSlot = GameObject.Find("DragCanvas").FindChild("DragSlot"); //Find말고 어떤식으로 DragSlot을 찾을지 생각하기
        _dragSlotComponent = _dragSlot.GetComponent<DragSlot>();
        _inventory = this.GetComponentInParent<UI_Inventory>();
        SlotClear();
        return true;
    }
    public void SetInfo(ItemDataSo itemData, int index, int slotType = -1, bool isLock = true) //인벤토리가 활성화 되면 Data를 전달받아 Slot 셋팅을 다시함
    {
        _slotIndex = index;
        _slotType = (E_SlotType)slotType;
        ItemData = itemData;
        _isLock = isLock;
        Swap();
        SetAlpha(_itemImage.sprite != null);
    }
    public void SlotClear() //슬롯을 클리어 해주는 함수입니다.
    {
        _itemImage.sprite = null;
        _quantityText.text = string.Empty;
        SetAlpha(_itemImage.sprite != null);
        ItemData = new ItemDataSo();
    }
    private void SlotSetting() 
    {
        _itemImage.sprite = ItemData.ItemImage;
        _quantityText.text = (ItemData.CurrentAmont != 0) ? ItemData.CurrentAmont.ToString() : string.Empty; //아이템 Data의 수량에 맞게 Text변경
    }
    private void SetAlpha(bool isAlphaColor) //인벤토리의 아이템이 없으면 이미지의 Color값을 변경함
    {
        Color color = _itemImage.color;
        color.a = isAlphaColor ? 1 : 0;
        _itemImage.color = color;
    }
    public void Swap() //아이템의 이미지와 Sprite를 변경함
    {
        SetAlpha(ItemData.KeyNumber != 0);
        if (ItemData.KeyNumber == 0)
        {
            SlotClear();
            return;
        }
        SlotSetting();
    }
    public void AddItem<T>(T item) where T : ItemDataSo // 아이템을 Slot에 추가하기 위한 함수
    {
        ItemData = item; // 제네릭 타입의 아이템을 저장
        SlotSetting();
        SetAlpha(_itemImage.sprite != null);
    }
    public void UseItem() //우클릭시 아이템을 하나씩 사라지게 만들었음 
    {
        if (ItemData.BaseType == E_BaseType.UseItem)
        {
            ItemData.CurrentAmont -= 1; // 아이템의 수량을 하나 줄입니다.
            UseItem item = ItemData as UseItem;
            if (item.UseType == E_UseItemType.Hunger)
            {
                Debug.Log("배고픔 증가");
            }
            else if (item.UseType == E_UseItemType.Hp)
            {
                Debug.Log("체력 증가");
            }
            SlotSetting();
            if (ItemData.CurrentAmont <= 0) // 수량이 0 이하라면 아이템을 인벤토리에서 제거합니다.
                SlotClear();
        }
    }
    public int ItemMinus(int value) 
    {
        int currentValue = 0;
        ItemData.CurrentAmont -= value;
        SlotSetting();
        if (ItemData.CurrentAmont <= 0) // 뺀 값이 마이너스로 가면 슬롯 클리어 및 마이너스 값 전달
        {
            currentValue = ItemData.CurrentAmont;
            SlotClear();
        }
        return currentValue;
    }
    public int MaxStackCheck(ItemDataSo useItem) //아이템의 수량이 최대 수량보다 높게 합쳐지지 않게하기 위한 함수
    {
        if (useItem == null) return 0;
        var item = useItem.BaseType == E_BaseType.UseItem ? useItem as UseItem : useItem as EtcItem;
        ItemData.CurrentAmont += item == null ? 0 : item.CurrentAmont;
        if (item != null && ItemData.CurrentAmont > item.MaxStack)
        {
            int returnValue = ItemData.CurrentAmont - item.MaxStack;
            ItemData.CurrentAmont = item.MaxStack;
            _quantityText.text = ItemData.CurrentAmont.ToString();
            return returnValue;
        }
        else
        {
            _quantityText.text = ItemData.CurrentAmont.ToString();
            return 0;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_inventory != null) _inventory.SelectSlot(this);
        if (_itemImage.sprite != null && _dragSlot != null && _isLock)
        {
            _dragSlot.SetActive(true);
            _dragSlot.transform.position = transform.position;
            _dragSlotComponent.SetSlot(_itemImage, _quantityText, this);
        }
        else if (!_isLock && this.GetComponentInParent<CraftingInventory>() != null)
        {
            CraftingInventory crafting = this.GetComponentInParent<CraftingInventory>();
            if (crafting != null)
            {
                crafting.NewItemSlot.SlotClear();
                crafting.NewItemSlot.SetInfo(ItemData,0,-1,false);
                crafting.CraftingSlot._itemData = ItemData;
                crafting.CraftingSlot.SlotDataSet();
            }
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_dragSlotComponent.slot != null && _dragSlotComponent.slot.ItemData.KeyNumber != 0 && _isLock)
        {
            _dragSlot.transform.position = eventData.position;
        }
    }
    public void OnDrop(PointerEventData eventData) //아이템의 스왑이 이뤄지는 공간 
    {
        Inventory inventory = this.GetComponentInParent<Inventory>();
        if (_dragSlotComponent.slot != null && _dragSlotComponent.slot.ItemData.KeyNumber != 0 && inventory != null && this != _dragSlotComponent.slot && _isLock)
        {
            if (this._slotType == E_SlotType.None) 
            {
                if (_dragSlotComponent.slot.ItemData is ArmorItem && _dragSlotComponent.slot.SlotType == E_SlotType.BackPack)
                {
                    var itemType = _dragSlotComponent.slot.ItemData as ArmorItem;
                    if (itemType.armorType == E_ArmorItemType.BackPack)
                    {
                        int count = _inventory.GetBackPackSlot();
                        if (count == 0)
                            inventory.SlotSwap(this, _dragSlotComponent.slot);
                    }
                }
                else
                {
                    inventory.SlotSwap(this, _dragSlotComponent.slot);
                }
            }
            else
            {
                if ((E_SlotType)_dragSlotComponent.slot.ItemData.BaseType == this.SlotType)
                    inventory.SlotSwap(this, _dragSlotComponent.slot);
                else if (_dragSlotComponent.slot.ItemData is ArmorItem)
                {
                    var itemType = _dragSlotComponent.slot.ItemData as ArmorItem;
                    if (this.SlotType == E_SlotType.BackPack)
                    {
                        int count = _inventory.GetBackPackSlot();
                        if (itemType.PlusValue >= count)
                            inventory.SlotSwap(this, _dragSlotComponent.slot);
                    }
                    else if ((E_SlotType)itemType.armorType == this.SlotType)
                        inventory.SlotSwap(this, _dragSlotComponent.slot);
                }
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
