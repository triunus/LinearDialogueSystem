using UnityEngine;

namespace Foundations.Patterns.Singleton
{
    /// <summary>
    /// 글로벌 전용 Singleton입니다.  ( 모든 Scene에서 공통으로 사용되는 것 )
    /// 인스턴스가 존재하지 않으면, 생성.
    /// 미리 배치된 인스턴스가 있으면 해당 객체를 인스턴스로 사용.
    /// 중복되는 인스턴스가 존재하면, 삭제.
    /// </summary>
    [DefaultExecutionOrder(-200)]
    public abstract class GlobalSingleton<T> : MonoBehaviour where T : GlobalSingleton<T>
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

                        // AddComponent<T>()를 수행하는 동시에, Awake()가 호출되고 이때 instance 멤버 정의와 DontDestroyOnLoad가 중복 호출이 되지만, 문제는 안됨.
                        // 더 정확하게 코드를 만드릭 위해서는 코드가 복잡해짐. 따라서 단순하고 명확한 이 형식을 유지.
                        instance = GO.AddComponent<T>();
                        DontDestroyOnLoad(GO);
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
                DontDestroyOnLoad(this.gameObject);
                Debug.Log($"[씬 전용 싱글톤<{typeof(T).Name}>] 최초 인스턴스로 등록되었습니다.");
            }
            else if (instance != this)
            {
                Debug.LogWarning($"[씬 전용 싱글톤<{typeof(T).Name}>] 중복 인스턴스 발견 → {name} 오브젝트를 파괴합니다.");
                Destroy(gameObject);
            }
        }
    }
}