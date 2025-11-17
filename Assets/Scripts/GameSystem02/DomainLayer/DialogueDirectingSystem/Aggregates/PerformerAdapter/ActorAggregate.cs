using System.Collections.Generic;

using GameSystems.InfrastructureLayer.DialogueDirectingSystem;
using GameSystems.OutputLayer.DialogueDirectingSystem;

namespace GameSystems.DomainLayer.DialogueDirectingSystem
{
    public  interface IActorAggregate
    {
        public void UpdateActorAdapter(IRuntimeActorDB runtimeActorDB);

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

        public void UpdateActorAdapter(IRuntimeActorDB runtimeActorDB)
        {
            foreach(var actorPair in runtimeActorDB.GetAllActorPair())
            {
                if (!actorPair.Value.TryGetComponent<IActorPerformerAdapter>(out var actorPerformerAdapter)) continue;

                this.ActorPerformerAdapters.RegisterValue(actorPair.Key, actorPerformerAdapter);
            }
        }

        public bool TryGetActorAdapter(string key, out IActorPerformerAdapter value) => this.ActorPerformerAdapters.TryGetValue(key, out value);
        public IEnumerable<IActorPerformerAdapter> GetAllActorAdapter() => this.ActorPerformerAdapters.GetAllValues;
    }
}