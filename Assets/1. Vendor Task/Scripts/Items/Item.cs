namespace VendorTask.Items
{
    public readonly struct Item
    {
        public readonly int Id;
        public readonly int Cost;

        private readonly string _name;

        public Item(int id, int cost, string name)
        {
            Id = id;
            Cost = cost;
            _name = name;
        }

        public override bool Equals(object other)
        {
            if (other is Item item)
                return item.Id == Id;

            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ Cost;
                hashCode = (hashCode * 397) ^ (_name != null ? _name.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return _name;
        }
    }
}