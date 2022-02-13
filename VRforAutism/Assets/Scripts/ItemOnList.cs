public class ItemOnList
{
    private readonly int _nItem;
    private int _nTaken;

    public bool IsAllTaken { get; set; }

    public Item Item { get; }
    public int NTaken => _nTaken;
    public string Info => $"{Item.ItemName}  {_nTaken}/{_nItem}";

    public ItemOnList(Item item, int nTaken = 0, bool isAllTaken = false, int nItem = 1)
    {
        Item = item;
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