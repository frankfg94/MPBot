

namespace Bot_Test.Database.DbTools
{
    public class Tables
    {
        public string Name { get; set; }
        public string NameColumn { get; set; }

        public Tables(string name, string nameColumn)
        {
            Name = name;
            NameColumn = nameColumn;
        }
    }
}