using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SD_UI
{
    public class DamageEventDetails
    {
        public double DamageAmount { get; set; }
        public Color TextColor { get; set; }
        public float TextSize { get; set; }

        public DamageEventDetails(double damageAmount, Color textColor, float textSize)
        {
            DamageAmount = damageAmount;
            TextColor = textColor;
            TextSize = textSize;
        }
    }
}