using UnityEngine;
using UnityEngine.EventSystems;

public class Itemsadder : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject[] items;

    public int _selectedItemIndex { get; set; }

    private GameObject _targetCeil, m_item;

    public void setSelectedItem(int index)
    {
        _selectedItemIndex = index;
    }

    void Start()
    {
        EventBus.OnCanPlaceItemResponce += SpawnItem;
    }

    private void SpawnItem(bool rq)
    {
        if (!rq)
            return;

        GameObject m_item = Instantiate(items[_selectedItemIndex - 1], _targetCeil.transform);
        m_item.transform.SetAsFirstSibling();
        EventBus.OnItemSpawned?.Invoke((int)_targetCeil.transform.localPosition.x, (int)_targetCeil.transform.localPosition.y, (item)_selectedItemIndex);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_selectedItemIndex == 0)
            return;

        if (eventData.button == PointerEventData.InputButton.Right || _selectedItemIndex == -1)
        {
            if (!eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).gameObject.name.Contains("item"))
                return;

            EventBus.OnItemDelete?.Invoke((int)eventData.pointerCurrentRaycast.gameObject.transform.localPosition.x,
             (int)eventData.pointerCurrentRaycast.gameObject.transform.localPosition.y);

            Destroy(eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).gameObject);
            return;
        }

        _targetCeil = eventData.pointerCurrentRaycast.gameObject;
        EventBus.OnCanPlaceItemCheck?.Invoke((int)_targetCeil.transform.localPosition.x, (int)_targetCeil.transform.localPosition.y);
    }
}
