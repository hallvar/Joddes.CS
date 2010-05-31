namespace System.Reflection
{
    public class PropertyInfo
    {
        string name;

        public PropertyInfo (string name, Type type)
        {
            this.name = name;
            this.PropertyType = type;
        }

        public Type PropertyType {
            get; private set;
        }

        public void SetValue (object obj, object val, object[] index)
        {
            ((Jsm.Html5.Object)obj)[this.name] = val;
        }
    }
}