using System.Collections;
using System.Linq;

using UnityEngine;

namespace GameSystems.Entities.MainStageScene
{
    public class FaderPlugInHub : PlugInHub<IFadeInAndOut>
    {
        public bool TryFadeIn(string key, string faderContent, out IEnumerator enumerator)
        {
            enumerator = null;

            if (this.PlugIns.ContainsKey(key))
            {
                if (!this.TryParseDuration(faderContent, out var duration)) return false;

                enumerator = this.PlugIns[key].FadeIn(duration);
                return true;
            }
            else return false;
        }
        public bool TryFadeOut(string key, string faderContent, out IEnumerator enumerator)
        {
            enumerator = null;

            if (this.PlugIns.ContainsKey(key))
            {
                if (!this.TryParseDuration(faderContent, out var duration)) return false;

                enumerator = this.PlugIns[key].FadeOut(duration);
                return true;
            }
            else return false;
        }
        private bool TryParseDuration(string faderContent, out float duration)
        {
            string[] parsedData = faderContent.Split('_');

            // 오류 값일 경우, 1f 값을 할당.
            if (parsedData.Length > 1)
            {
                duration = 1f;
                return false;
            }

            // string을 float으로 파싱.
            duration = float.Parse(parsedData[0]);

            // 너무 낮거나 높은 경우 값 제한.
            if (duration <= 0) duration = Time.deltaTime;
            if (10 <= duration) duration = 10f;
            
            return true;
        }
    }
}