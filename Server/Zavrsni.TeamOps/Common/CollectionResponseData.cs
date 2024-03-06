namespace Zavrsni.TeamOps.Common
{
    public class CollectionResponseData<T>
    {
        public IList<T> Items { get; set; } = new List<T>();
        public int Count { get; set; }
    }
}
