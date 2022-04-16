namespace Lab2
{
    public class TableItem
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public TableItem(string id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
