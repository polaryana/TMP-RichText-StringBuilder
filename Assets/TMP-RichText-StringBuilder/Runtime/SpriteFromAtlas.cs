using System;
using TMP_RichText_StringBuilder.Runtime.Base;
using UnityEngine;

namespace TMP_RichText_StringBuilder.Runtime
{
    [Serializable]
    public class SpriteFromAtlas : TextTool
    {
        [SerializeField] private int spriteIndexFromAtlas;
        
        public override string ConvertToString()
        {
            return $"<sprite={spriteIndexFromAtlas}>";
        }
    }
}