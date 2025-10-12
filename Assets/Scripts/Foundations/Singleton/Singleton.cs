using System;

namespace Foundations.Patterns.Singleton
{
    /// <summary>
    /// 제네릭 기반의 상속용 싱글톤 클래스.
    /// </summary>
    public abstract class Singleton<T> where T : class
    {
        /// <summary>
        /// Lazy<T>를 사용하여 thread-safe하며, 처음 Instance에 접근할 때만 생성된다.
        /// </summary>
        private static readonly Lazy<T> lazyInstance = new(() => (T)Activator.CreateInstance(typeof(T), nonPublic: true)!);

        /// <summary>
        /// 외부에서 접근 가능한 싱글 인스턴스.
        /// </summary>
        public static T Instance => lazyInstance.Value;

        /// <summary>
        /// 외부 생성 차단용 기본 생성자.
        /// 반드시 private 또는 protected로 선언해야 함.
        /// </summary>
        protected Singleton() { }
    }
}