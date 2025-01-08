public class Element(string group, int pos, string name, int num, string sign, double mol, IList<int> el)
    {
        public string Group { get; set; } = group;
        public int Position { get; set; } = pos;
        public string Name { get; set; } = name;
        public int Number { get; set; } = num;
        public string Sign { get; set; } = sign;
        public double Molar { get; set; } = mol;
        public IList<int> Electrons { get; set; } = el;

        public override string ToString()
        {
            return $"{Sign} - {Name}";
        }
    }