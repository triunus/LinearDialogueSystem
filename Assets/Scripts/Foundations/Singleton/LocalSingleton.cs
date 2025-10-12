using UnityEngine;

namespace Foundations.Patterns.Singleton
{
    /// <summary>
    /// 씬 전용 Singleton입니다.
    /// 인스턴스가 존재하지 않으면, 생성.
    /// 미리 배치된 인스턴스가 있으면 해당 객체를 인스턴스로 사용.
    /// 중복되는 인스턴스가 존재하면, 삭제.
    /// </summary>
    [DefaultExecutionOrder(-200)]
    public abstract class LocalSingleton<T> : MonoBehaviour where T : LocalSingleton<T>
    {
        private static T instance;

        /// <summary>
        /// 싱글 인스턴스 접근 프로퍼티
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // 씬에 존재하는 인스턴스를 찾거나,
                    instance = FindObjectOfType<T>();

                    // 없으면 T클래스가 컴포넌트될 객체 생성.
                    if (instance == null)
                    {
                        Debug.Log($"[씬 전용 싱글톤<{typeof(T).Name}>] 씬에서 찾지 못해 자동 생성했습니다.");
                        var GO = new GameObject(typeof(T).Name);

                        instance = GO.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        // 프리팹으로 배치된 경우를 위한 안전장치(선택적)
        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
                Debug.Log($"[씬 전용 싱글톤<{typeof(T).Name}>] 최초 인스턴스로 등록되었습니다.");
            }
            else if (instance != this)
            {
                Debug.LogWarning($"[씬 전용 싱글톤<{typeof(T).Name}>] 중복 인스턴스 발견 → {name} 오브젝트를 파괴합니다.");
                Destroy(gameObject);
            }
        }


        // Scene 종료 시, 해당 instance 초기화.
        private void OnDestroy()
        {
            if (instance == this)
            {
                Debug.Log($"[씬 전용 싱글톤<{typeof(T).Name}>] 씬 언로드로 인해 인스턴스가 해제되었습니다.");
                instance = null;
            }
        }

        // 유니티 Editor에서 static으로 사용한 값을 남기는 기능을 비활성화 시키기.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void SubsystemInit() => instance = null;
    }
}