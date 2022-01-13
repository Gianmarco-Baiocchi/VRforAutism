public class ItemOnList
{
    private Item _item;
    private bool _isAllTaken;
    private int _nItem;
    private int _nTaken;

    public bool IsAllTaken 
    {
        get => _isAllTaken;
        set => _isAllTaken = value;
    }
    public Item Item => _item;
    public string Info => $"{_item.ItemName}  {_nTaken}/{_nItem}";

    public ItemOnList(Item item, bool isAllTaken = false, int nItem = 1)
    {
        _item = item;
        _isAllTaken = isAllTaken;
        _nItem = nItem;
        _nTaken = 0;
    }
    
    public void takeOne()
    {
        if (_nTaken != _nItem)
        {
            _nTaken++;
            if (_nTaken == _nItem)
            {
                _isAllTaken = true;
            }
        }
    }

    public void removeOne()
    {
        if (_nTaken != 0)
        {
            _nTaken--;
            if (_isAllTaken)
            {
                _isAllTaken = false;
            }
        }
    }
}