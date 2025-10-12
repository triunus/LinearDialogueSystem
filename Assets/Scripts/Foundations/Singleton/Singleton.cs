using System;

namespace Foundations.Patterns.Singleton
{
    /// <summary>
    /// ���׸� ����� ��ӿ� �̱��� Ŭ����.
    /// </summary>
    public abstract class Singleton<T> where T : class
    {
        /// <summary>
        /// Lazy<T>�� ����Ͽ� thread-safe�ϸ�, ó�� Instance�� ������ ���� �����ȴ�.
        /// </summary>
        private static readonly Lazy<T> lazyInstance = new(() => (T)Activator.CreateInstance(typeof(T), nonPublic: true)!);

        /// <summary>
        /// �ܺο��� ���� ������ �̱� �ν��Ͻ�.
        /// </summary>
        public static T Instance => lazyInstance.Value;

        /// <summary>
        /// �ܺ� ���� ���ܿ� �⺻ ������.
        /// �ݵ�� private �Ǵ� protected�� �����ؾ� ��.
        /// </summary>
        protected Singleton() { }
    }
}