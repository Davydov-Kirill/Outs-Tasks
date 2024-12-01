namespace Reflection.ConsoleExample
{
    [Serializable]
    public class F
    {
        private int i1, i2, i3, i4, i5;

        public int I1 { get => i1; set => i1 = value; }
        public int I2 { get => i2; set => i2 = value; }
        public int I3 { get => i3; set => i3 = value; }
        public int I4 { get => i4; set => i4 = value; }
        public int I5 { get => i5; set => i5 = value; }

        public static F Get()
        {
            return new F() { i1 = 1, i2 = 2, i3 = 3, i4 = 4, i5 = 5 };
        }

        public override string ToString() => $"i1: {i1}, i2: {i2}, i3: {i3}, i4: {i4}, i5: {i5}";
    }
}