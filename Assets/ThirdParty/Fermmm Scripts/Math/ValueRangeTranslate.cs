using UnityEngine;

public static class ValueRangeTranslate
{
    ///  <summary>
    ///  Use this function if you have 2 ranges of values and want to know a equivalent from the range 1 to the range 2
    /// 	Example:
    /// 	range1: 40 to 80
    /// 	range2: 10 to 20
    /// 	if valuetotranslate = 80
    /// 	result = 20
    /// 	This is the formula of the function: (((valueToTranslate - range1Min) * (range2Max - range2Min)) / (range1Max - range1Min)) + range2Min;
    ///  </summary>
    ///  <param name='valueToTranslate'>
    ///  A value from range 1 to be translated to range 2
    ///  </param>
    ///  <param name='range1Min'>
    ///  Range 1 minimum.
    ///  </param>
    ///  <param name='range1Max'>
    ///  Range 1 max.
    ///  </param>
    ///  <param name='range2Min'>
    ///  Range 2 minimum.
    ///  </param>
    ///  <param name='range2Max'>
    ///  Range 2 max.
    ///  </param>
    /// <param name="trowErrors">Trows an error when range1Min and range1Max are the same number, instead of only returning NaN wich can be a hard to track issue.</param>
    public static float TranslateFloat(float valueToTranslate, float range1Min, float range1Max, float range2Min, float range2Max, bool trowErrors = true)
	{
        float res = (((valueToTranslate - range1Min) * (range2Max - range2Min)) / (range1Max - range1Min)) + range2Min;
        if(trowErrors && float.IsNaN(res))
            Debug.LogError("ValueRangeTranslate error. Range 1 min cannot be the same than Range 1 max");
		return res;
	}

    ///  <summary>
    ///  Use this function if you have 2 ranges of values and want to know a equivalent from the range 1 to the range 2
    /// 	Example:
    /// 	range1: 40 to 80
    /// 	range2: 10 to 20
    /// 	if valuetotranslate = 80
    /// 	result = 20
    /// 	This is the formula of the function: (((valueToTranslate - range1Min) * (range2Max - range2Min)) / (range1Max - range1Min)) + range2Min;
    ///  </summary>
    ///  <param name='valueToTranslate'>
    ///  A value from range 1 to be translated to range 2
    ///  </param>
    ///  <param name='range1Min'>
    ///  Range 1 minimum.
    ///  </param>
    ///  <param name='range1Max'>
    ///  Range 1 max.
    ///  </param>
    ///  <param name='range2Min'>
    ///  Range 2 minimum.
    ///  </param>
    ///  <param name='range2Max'>
    ///  Range 2 max.
    ///  </param>
    /// <param name="trowErrors">Trows an error when range1Min and range1Max are the same number, instead of only returning NaN wich can be a hard to track issue.</param>
    public static Vector2 TranslateVector2(Vector2 valueToTranslate, Vector2 range1Min, Vector2 range1Max, Vector2 range2Min, Vector2 range2Max, bool trowErrors = true)
    {
        return new Vector2(
                TranslateFloat(valueToTranslate.x, range1Min.x, range1Max.x, range2Min.x, range2Max.x, trowErrors),
                TranslateFloat(valueToTranslate.y, range1Min.y, range1Max.y, range2Min.y, range2Max.y, trowErrors)
            );
    }
}