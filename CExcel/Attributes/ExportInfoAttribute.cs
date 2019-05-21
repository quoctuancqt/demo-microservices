namespace CExcel.Attributes
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ExportInfoAttribute : Attribute
    {
        public ExportInfoAttribute(int order = 1, string header = "")
        {
            Order = order;
            Header = header;
        }

        public int Order { get; }
        public string Header { get; set; }
    }
}
