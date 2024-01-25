using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    private E_ItemType _slotType; //슬롯의 타입

    public ItemData ItemData { get => _itemData; set { _itemData = value; } }
    public Image SpriteRenderer { get { return _itemImage; }}
    public TextMeshProUGUI QuantityText { get { return _quantityText; } }

    public override bool Initialize()
    {
        if (!base.Initialize()) return false;

        BindImage(typeof(E_Images));
        BindText(typeof(E_QuantityText));
        
        _itemImage = GetImage((int)E_Images.ItemImage);
        _quantityText = GetText((int)E_QuantityText.ItemQuantity);

        if (_itemImage.sprite == null) //타입이 장착 타입이거나, 이미지가 없으면 문자 X
        {
            _quantityText.text = string.Empty;
        }

        return true;
    }

    public void SetInfo(ItemData itemData, int index, E_ItemType itemType = E_ItemType.None) //인벤토리에서 데이터가 변경되면 다시 Setinfo로 해주기
    {
        Initialize();
        slotIndex = index;
        _dragSlot = GameObject.Find("DragSlot");
        _dragSlotComponent = _dragSlot.GetComponent<DragSlot>();
        _slotType = itemType;
        if (itemData.Name == null)
        {
            _quantityText.text = string.Empty;
            _itemImage.sprite = null;
        }
        SetAlpha(_itemImage.sprite != null);
        //_quantityText.text = itemData.quantity; 아이템의 수량을 Data로 관리하기
        //_spriteRenderer.sprite = 리소스매니저를 통해 itemName;itemName을 이용해서 이미지 가져오기
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
    public void Swap(Sprite sprite, string text)
    {
        SetAlpha(sprite != null);
        if (sprite == null)
        {
            SlotClear();
            return;
        }
        _itemImage.sprite = sprite;
        _quantityText.text = text;
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
        if (_dragSlotComponent.slot != null && _dragSlotComponent.slot.SpriteRenderer.sprite != null)
        {
            if (this.slotIndex < 15 || this.slotIndex > 23)
            {
                Inventory.Instance.SlotSwap(this, _dragSlotComponent.slot);
            }
            else if (_slotType == Inventory.Instance.TypeCheck(_dragSlotComponent.slot))
            {
                Inventory.Instance.SlotSwap(this, _dragSlotComponent.slot);
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
