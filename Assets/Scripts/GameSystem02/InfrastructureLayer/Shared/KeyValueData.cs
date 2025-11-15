using System.Collections.Generic;

namespace GameSystems.InfrastructureLayer.DialogueDirectingSystem
{
    public class KeyValueData<keyType, ValueType>
    {
        private Dictionary<keyType, ValueType> Datas;

        public KeyValueData()
        {
            this.Datas = new();
        }

        public void RegisterValue(keyType keyType, ValueType valueType)
        {
            if (this.Datas.ContainsKey(keyType)) return;

            this.Datas.Add(keyType, valueType);
        }

        public void RemoveValue(keyType keyType)
        {
            if (!this.Datas.ContainsKey(keyType)) return;

            this.Datas.Remove(keyType);
        }

        public bool TryGetValue(keyType keyType, out ValueType valueType)
        {
            if (this.Datas.TryGetValue(keyType, out valueType))
                return true;
            else
                return false;
        }

        public IEnumerable<ValueType> GetAllValues => this.Datas.Values;

        public IEnumerable<KeyValuePair<keyType, ValueType>> GetAllPairs => this.Datas;
    }
}