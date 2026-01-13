using System;
using TMP_RichText_StringBuilder.Runtime.Base;
using UnityEngine;

namespace TMP_RichText_StringBuilder.Runtime
{
    [Serializable]
    public class ColorChange : TextTool
    {
        [SerializeField] private Color color;
        
        public override string ConvertToString()
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>";
        }
    }
}