using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class TokenEstimator
{
    public static int Estimate(string text) => string.IsNullOrEmpty(text) ? 0 : Math.Max(1, text.Length / 4);
}