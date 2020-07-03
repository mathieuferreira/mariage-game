namespace Bar
{
    public class BarConsumable
    {
        public enum Kind
        {
            Beer,
            Cake,
            Talk
        }

        private readonly Kind kind;

        public BarConsumable(Kind kind)
        {
            this.kind = kind;
        }

        public Kind GetKind()
        {
            return kind;
        }
    }
}
