namespace QuickMember
{
    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Field, AllowMultiple = false)]
    public class OrdinalAttribute : System.Attribute
    {
        public OrdinalAttribute(ushort ordinal)
        {
            Ordinal = ordinal;
        }

        public ushort Ordinal { get; private set; }
    }
}
