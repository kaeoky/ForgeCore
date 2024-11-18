using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ForgeCore.Utility.UI
{
    public static class UIUtility
    {
        public static IEnumerator FadeInOutImage(float fadeIn, float lifetime, float fadeOut, Image image)
        {
            yield return FadeImageToFullAlpha(fadeIn, image);
            yield return new WaitForSeconds(lifetime);
            yield return FadeImageToZeroAlpha(fadeOut, image);
        }
        
        public static IEnumerator FadeInOutText(float fadeIn, float lifetime, float fadeOut, TextMeshProUGUI text)
        {
            yield return FadeTextToFullAlpha(fadeIn, text);
            yield return new WaitForSeconds(lifetime);
            yield return FadeTextToZeroAlpha(fadeOut, text);
        }
        
        public static IEnumerator FadeImageToFullAlpha(float t, Image i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
            while (i.color.a < 1.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                yield return null;
            }
        }
        
        public static IEnumerator FadeImageToZeroAlpha(float t, Image i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
            while (i.color.a > 0.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
                yield return null;
            }
        }
        
        public static IEnumerator FadeTextToFullAlpha(float t, TextMeshProUGUI i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
            while (i.color.a < 1.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
                yield return null;
            }
        }
 
        public static IEnumerator FadeTextToZeroAlpha(float t, TextMeshProUGUI i)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
            while (i.color.a > 0.0f)
            {
                i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
                yield return null;
            }
        }
    }
}