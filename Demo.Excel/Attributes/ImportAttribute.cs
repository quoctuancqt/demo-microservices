namespace CExcel.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ImportAttribute : Attribute
    {
        public ImportAttribute(int order = 1)
        {
            Order = order;
        }

        public string Header { get; }
        public int Order { get; }
    }
}
