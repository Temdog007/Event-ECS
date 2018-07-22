namespace Event_ECS_WPF.SystemObjects
{
    public struct ECSData
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0}, Value: {1}({2})", Name, Value, Type);
        }
    }
}
