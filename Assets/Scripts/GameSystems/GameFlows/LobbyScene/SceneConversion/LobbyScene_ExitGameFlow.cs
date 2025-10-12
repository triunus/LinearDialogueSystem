using UnityEngine;

namespace GameSystems.GameFlows.LobbyScene
{
    public interface ILobbyScene_ExitGameFlow
    {
        public void OperateExitGame();
    }

    public class LobbyScene_ExitGameFlow : MonoBehaviour, IGameFlow, ILobbyScene_ExitGameFlow
    {
        private void Awake()
        {
            var LocalRepository = Repository.LobbySceneRepository.Instance;

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<LobbyScene_ExitGameFlow>(this);
        }

        public void OperateExitGame()
        {

        }
    }
}
