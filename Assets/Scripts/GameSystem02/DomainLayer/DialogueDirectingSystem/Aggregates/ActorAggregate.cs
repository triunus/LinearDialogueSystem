using System.Collections.Generic;

using GameSystems.InfrastructureLayer.DialogueDirectingSystem;
using GameSystems.OutputLayer.DialogueDirectingSystem;

namespace GameSystems.DomainLayer.DialogueDirectingSystem
{
    public  interface IActorAggregate
    {
        public void UpdateActorAdapter(RuntimeActorDB runtimeActorDB);

        public bool TryGetActorAdapter(string key, out IActorPerformerAdapter value);
        public IEnumerable<IActorPerformerAdapter> GetAllActorAdapter();
    }

    public class ActorAggregate : IActorAggregate
    {
        private KeyValueData<string, IActorPerformerAdapter> ActorPerformerAdapters;

        public ActorAggregate()
        {
            this.ActorPerformerAdapters = new();
        }

        public void UpdateActorAdapter(RuntimeActorDB runtimeActorDB)
        {
            foreach(var actorPair in runtimeActorDB.GetAllActorPair())
            {
                var adapter = actorPair.Value.GetComponent<IActorPerformerAdapter>();
                if (adapter == null) continue;

                this.ActorPerformerAdapters.RegisterValue(actorPair.Key, adapter);
            }
        }

        public bool TryGetActorAdapter(string key, out IActorPerformerAdapter value) => this.ActorPerformerAdapters.TryGetValue(key, out value);
        public IEnumerable<IActorPerformerAdapter> GetAllActorAdapter() => this.ActorPerformerAdapters.GetAllValues;
    }
}