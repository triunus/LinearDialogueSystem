using System.Collections.Generic;

using GameSystems.InfrastructureLayer.DialogueDirectingSystem;
using GameSystems.OutputLayer.DialogueDirectingSystem;

namespace GameSystems.DomainLayer.DialogueDirectingSystem
{
    public interface IBackGroundAggregate
    {
        public void UpdateBackGroundAdapter(RuntimeBackGroundDB runtimeBackGroundDB);

        public bool TryGetBackGroundAdapter(string key, out IBackGroundAdapter value);
        public IEnumerable<IBackGroundAdapter> GetAllBackGroundAdapter();
    }

    public class BackGroundAggregate : IBackGroundAggregate
    {
        private KeyValueData<string, IBackGroundAdapter> BackGroundAdapter;

        public BackGroundAggregate()
        {
            this.BackGroundAdapter = new();
        }

        public void UpdateBackGroundAdapter(RuntimeBackGroundDB runtimeBackGroundDB)
        {
            foreach (var backGroundPair in runtimeBackGroundDB.GetAllBackGroundPair())
            {
                var adapter = backGroundPair.Value.GetComponent<IBackGroundAdapter>();
                if (adapter == null) continue;

                this.BackGroundAdapter.RegisterValue(backGroundPair.Key, adapter);
            }
        }

        public bool TryGetBackGroundAdapter(string key, out IBackGroundAdapter value) => this.BackGroundAdapter.TryGetValue(key, out value);
        public IEnumerable<IBackGroundAdapter> GetAllBackGroundAdapter() => this.BackGroundAdapter.GetAllValues;
    }
}