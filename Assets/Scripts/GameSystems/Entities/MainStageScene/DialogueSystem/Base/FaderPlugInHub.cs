using System.Collections;
using System.Linq;

using UnityEngine;


namespace GameSystems.Entities.MainStageScene
{
    public class FaderPlugInHub : PlugInHub<IFadeInAndOut>
    {
        public bool TryFadeIn(string key, string actionDuration, out IEnumerator enumerator)
        {
            enumerator = null;

            if (this.PlugIns.ContainsKey(key))
            {
                if (!this.TryParseDuration(actionDuration, out var duration)) return false;

                enumerator = this.PlugIns[key].FadeIn(duration);
                return true;
            }
            else return false;
        }
        public bool TryFadeOut(string key, string actionDuration, out IEnumerator enumerator)
        {
            enumerator = null;

            if (this.PlugIns.ContainsKey(key))
            {
                if (!this.TryParseDuration(actionDuration, out var duration)) return false;

                enumerator = this.PlugIns[key].FadeOut(duration);
                return true;
            }
            else return false;
        }
        private bool TryParseDuration(string actionDuration, out float duration)
        {
            // string을 float으로 파싱.
            string onlyNumbers = new string(actionDuration.Where(c => char.IsDigit(c) || c == '.').ToArray());
            float convertedDuration = float.Parse(onlyNumbers);

            // 너무 낮거나 높은 경우 값 제한.
            if (convertedDuration <= 0) convertedDuration = Time.deltaTime;
            if (10 <= convertedDuration) convertedDuration = 10f;

            duration = convertedDuration;
            return true;
        }
    }
}