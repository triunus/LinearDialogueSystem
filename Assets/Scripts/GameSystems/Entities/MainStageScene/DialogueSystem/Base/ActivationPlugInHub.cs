namespace GameSystems.Entities.MainStageScene
{
    public class ActivationPlugInHub : PlugInHub<IActivation>
    {
        public bool TryDirectShow(string key)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;

            this.PlugIns[key].Show();
            return true;
        }

        public bool TryDirectHide(string key)
        {
            if (!this.PlugIns.ContainsKey(key)) return false;

            this.PlugIns[key].Hide();
            return true;
        }
    }
}