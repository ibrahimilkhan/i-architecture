namespace Core.Persistence.Paging;

public abstract class BasePageableModel
{
    public int Size { get; set; }
    public int Index { get; set; }
    public int Count { get; set; }
    public int Pages { get; set; }
    public int HasPrevious { get; set; }
    public int HasNext { get; set; }
}