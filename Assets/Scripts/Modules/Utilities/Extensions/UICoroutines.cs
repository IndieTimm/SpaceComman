using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUtilities.CoroutineHelper
{
    public static class UICoroutines
    {
        public class UIValueContainer : MonoBehaviour
        {
            public float alpha;
        }

        private class Item
        {
            public Color from;
            public Color to;
            public Graphic graphic;
        }

        public static IEnumerator FadeOut(GameObject target, float duration, bool unscaledTime = true)
        {
            return Fade(target, duration, true, unscaledTime);
        }

        public static IEnumerator FadeIn(GameObject target, float duration, bool unscaledTime = true)
        {
            return Fade(target, duration, false, unscaledTime);
        }

        public static IEnumerator Fade(GameObject target, float duration, bool fadeOut, bool unscaledTime)
        {
            var color_dictionary = new List<Item>();

            foreach (var graphic in target.GetComponentsInChildren<Graphic>())
            {
                var container = graphic.gameObject.GetComponent<UIValueContainer>();

                if (container == null)
                {
                    container = graphic.gameObject.AddComponent<UIValueContainer>();
                    container.alpha = graphic.color.a;
                }

                var color = fadeOut
                    ? new Color(graphic.color.r, graphic.color.g, graphic.color.b, 0)
                    : new Color(graphic.color.r, graphic.color.g, graphic.color.b, container.alpha);

                color_dictionary.Add(new Item()
                {
                    graphic = graphic,
                    from = graphic.color,
                    to = color
                });
            }

            return CoroutineBuilder.LerpValue01(duration, unscaledTime, time =>
            {
                foreach (Item item in color_dictionary)
                {
                    item.graphic.color = Color.Lerp(item.from, item.to, time);
                }
            });
        }
    }
}