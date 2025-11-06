using GameSystems.DialogueDirectingService.PlainServices;
using GameSystems.DialogueDirectingService.Datas;

using Foundations.PlugInHub;

namespace GameSystems.DialogueDirectingService.Views
{
    public interface IDialogueViewBinding
    {
        public void InitialBinding(string key, IMultiPlugInHub multiPlugInHub, IFadeInAndOutService fadeInAndOutService = null);
    }
}