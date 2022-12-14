namespace EzComs.Common.CustomExceptions
{
    [Serializable]
    public class ConversionException<Source, Target> : Exception
    {
        public ConversionException() : base($"There was a problem converting the {typeof(Source).FullName} to a {typeof(Target).FullName}.") { }
    }
}
