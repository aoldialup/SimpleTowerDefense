
namespace SimpleTowerDefense
{
    internal class DefenseInfo
    {
        public string Name { get; }

        public int Cost { get; }

        public DefenseInfo(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }
    }
}
