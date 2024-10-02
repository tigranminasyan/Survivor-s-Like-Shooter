using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtilities
{
    public static class Utilities
    {
        public static bool IsNotZero(float value, float epsilon = 0.0001f)
        {
            return Mathf.Abs(value) > epsilon;
        }
    }
}
