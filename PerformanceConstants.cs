/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/
#if PERFORMANCE
namespace Codefarts.VectorGrid
{
    /// <summary>
    /// Provides various keys as global constant values for use with the performance testing system.
    /// </summary>
    public class PerformanceConstants
    {
        public static string VectorGrid_OnRenderObject = "GeneralTools/VectorGrid/OnRenderObject";

        public static string[] GetPerformanceKeys()
        {
            return new[]
                {
                    VectorGrid_OnRenderObject                                          
                };
        }
    }
}
#endif