using System;
using TMP_RichText_StringBuilder.Runtime.Base;
using UnityEngine;

namespace TMP_RichText_StringBuilder.Runtime
{
    [Serializable]
    public class TextString : TextTool
    {
        [SerializeField] private string text;
        
        public override string ConvertToString()
        {
            return text;
        }
    }
}