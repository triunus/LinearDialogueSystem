using UnityEngine;

namespace GameSystems.GameFlows.MainStageScene
{
    public interface IMainStageScene_ExitGameFlow
    {
        public void OperateExitGame();
    }

    public class MainStageScene_ExitGameFlow : MonoBehaviour, IGameFlow, IMainStageScene_ExitGameFlow
    {
        private void Awake()
        {
            var LocalRepository = Repository.MainStageSceneRepository.Instance;

            // 등록
            LocalRepository.GameFlow_LazyReferenceRepository.RegisterReference<MainStageScene_ExitGameFlow>(this);
        }

        public void OperateExitGame()
        {
            Debug.Log($"게임 나가기");
        }
    }
}