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

    public int slotIndex; //슬롯의 번호

    [SerializeField] private ItemData _itemData; //아이템의 Data를 슬롯이 관리하게함
    [SerializeField] private Image _itemImage;              
    [SerializeField] private GameObject _dragSlot;
    [SerializeField] private DragSlot _dragSlotComponent;
    private TextMeshProUGUI _quantityText; //슬롯 
    [SerializeField] private E_ItemType _slotType; //슬롯의 타입
    public ItemData ItemData { get => _itemData; set { _itemData = value; } }
    public Image SpriteRenderer { get { return _itemImage; }}

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindImage(typeof(E_Images));
        BindText(typeof(E_QuantityText));
        
        _itemImage = GetImage((int)E_Images.ItemImage);
        _quantityText = GetText((int)E_QuantityText.ItemQuantity);

        if (_itemImage.sprite == null) //타입이 장착 타입이거나, 이미지가 없으면 문자 X
            _quantityText.text = string.Empty;

        AddItem(ItemData);
        return true;
    }

    public void SetInfo(ItemData itemData, int index, int slotType = -1) //인벤토리에서 데이터가 변경되면 다시 Setinfo로 해주기
    {
        Initialize();
        slotIndex = index;
        _dragSlot = GameObject.Find("DragCanvas").FindChild("DragSlot");    //Find말고 어떤식으로 DragSlot을 찾을지 생각하기
        _dragSlotComponent = _dragSlot.GetComponent<DragSlot>();
        _slotType = (E_ItemType)slotType;
        if (itemData.name == null)
        {
            _quantityText.text = string.Empty;
            _itemImage.sprite = null;
        }
        SetAlpha(_itemImage.sprite != null);
    }
    private void SlotClear()
    {
        _itemImage.sprite = null;
        _quantityText.text = string.Empty;
    }
    private void SetAlpha(bool isAlphaColor) //인벤토리의 아이템이 없으면 이미지의 Color값을 변경함
    {
        Color color = _itemImage.color;
        color.a = isAlphaColor ? 1 : 0;
        _itemImage.color = color;
    }
    public void Swap(Sprite sprite) //아이템의 이미지와 Sprite를 변경함 1. Data로 이미지를 불러올 거기 때문에 매개변수를 뺼 준비하기
    {
        SetAlpha(ItemData.keyNumber != 0);
        if (ItemData.keyNumber == 0)
        {
            SlotClear();
            return;
        }
        _itemImage.sprite = sprite;
        _quantityText.text = ItemData.stack.ToString();
    }
    public void AddItem(ItemData item) 
    {
        ItemData = item; //아이템 Data에 추가
        _quantityText.text = item.stack.ToString(); //아이템 Data의 수량에 맞게 Text변경
        var image = Resources.Load<Sprite>(item.name);  //리소스 매니저로 코드 변경해야함
        _itemImage.sprite = image;
        SetAlpha(_itemImage.sprite != null);
    }
    public int MaxStackCheck(int stack)
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
    public void OnPointerDown(PointerEventData eventData) //마우스 클릭시
    {
        if (_itemImage.sprite != null && _dragSlot != null)
        {
            _dragSlot.SetActive(true);
            _dragSlot.transform.position = transform.position;
            _dragSlotComponent.SetSlot(_itemImage, _quantityText, this);
        }
        //slot에 마우스 클릭시 Drag 슬롯을 활성화하고 현재의 마우스 포지션으로 위치 조정
    }
    public void OnDrag(PointerEventData eventData)
    {
        _dragSlot.transform.position = eventData.position;
    }
    public void OnDrop(PointerEventData eventData)
    {
        //현재의 slot의 번호와 놔두고 싶은 slot의 번호를 가져와 인벤토리의 DataSwitching 함수를 작동시켜 데이터 변경후
        //인벤토리에서는 Setinfo를 다시 작동 ex) itemslot[현재 slotindex].Setinfo(아이템 데이터[현재 slotindex]);
        Inventory inventory = this.GetComponentInParent<Inventory>();
        if (_dragSlotComponent != null && _dragSlotComponent.slot.SpriteRenderer.sprite != null && inventory != null)
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
