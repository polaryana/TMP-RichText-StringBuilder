using System;
using TMP_RichText_StringBuilder.Runtime.Base;

namespace TMP_RichText_StringBuilder.Runtime
{
    [Serializable]
    public class NewLine : TextTool
    {
        public override string ConvertToString()
        {
            return "\\n";
        }
    }
}