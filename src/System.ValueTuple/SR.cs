using System.Collections.Generic;
using System.Reflection;
using System.Threading;

partial class SR
{
    private static Dictionary<string, string> ResourceNameDictionary;

    public const string AggregateException_ToString = "{0}{1}---> (Inner Exception #{2}) {3}{4}{5}";

    public const string NotSupported_CannotWriteToBufferedStreamIfReadBufferCannotBeFlushed = "Cannot write to a BufferedStream while the read buffer is not empty if the underlying stream is not seekable. Ensure that the stream underlying this BufferedStream can seek or avoid interleaving read and write operations on this BufferedStream.";

    public static string Format(string format, params object[] args)
    {
        return string.Format(format, args);
    }

    public static string GetResourceString(string name)
    {
        InitializeResourceNameDictionary();
        return ResourceNameDictionary.TryGetValue(name, out string value)
            ? value
            : null;
    }

    private static void InitializeResourceNameDictionary()
    {
        if (ResourceNameDictionary != null)
            return;

        var tmpDictionary = new Dictionary<string, string>();
        if (Interlocked.CompareExchange(ref ResourceNameDictionary, tmpDictionary, null) != null)
            return;

        var fields = typeof(SR).GetFields(BindingFlags.Public | BindingFlags.Static);
        for (int i = 0; i < fields.Length; i++)
        {
            FieldInfo currentField = fields[i];

            // IsLiteral determines if its value is written at 
            //   compile time and not changeable
            // IsInitOnly determine if the field can be set 
            //   in the body of the constructor
            // for C# a field which is readonly keyword would have both true 
            //   but a const field would have only IsLiteral equal to true
            if (currentField.IsLiteral && !currentField.IsInitOnly)
                ResourceNameDictionary.Add(currentField.Name, (string)currentField.GetValue(null));
        }
    }
}
