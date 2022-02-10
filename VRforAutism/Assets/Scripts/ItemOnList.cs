public class ItemOnList
{
    private readonly Item _item;
    private readonly int _nItem;
    private int _nTaken;

    public bool IsAllTaken { get; set; }

    public Item Item => _item;
    public string Info => $"{_item.ItemName}  {_nTaken}/{_nItem}";

    public ItemOnList(Item item, bool isAllTaken = false, int nItem = 1)
    {
        _item = item;
        IsAllTaken = isAllTaken;
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
                IsAllTaken = true;
            }
        }
    }

    public void removeOne()
    {
        if (_nTaken != 0)
        {
            _nTaken--;
            if (IsAllTaken)
            {
                IsAllTaken = false;
            }
        }
    }
}