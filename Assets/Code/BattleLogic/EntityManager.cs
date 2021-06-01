using System.Collections.Generic;
using MiniRPG.Common;

namespace MiniRPG.BattleLogic
{
    public interface IEntityManager
    {
        IEnumerable<Entity> Entities { get; }
        void AddEntity(Entity entity);
        void RemoveEntity(int entityId);
        Entity GetEntity(int entityId);
    }

    public class EntityManager : IEntityManager
    {
        public IEnumerable<Entity> Entities => _entities.Values;
        private IDictionary<int, Entity> _entities;
        private ILogger _logger;

        public EntityManager(ILogger logger)
        {
            _entities = new Dictionary<int, Entity>();
            _logger = logger;
        }


        public void AddEntity(Entity entity)
        {
            if(_entities.ContainsKey(entity.id))
            {
                _logger.LogError($"Cannot add entity. An entity with the id : {entity.id} already exists.");
                return;
            }

            _entities[entity.id] = entity;
        }

        public Entity GetEntity(int entityId)
        {
            if(_entities.TryGetValue(entityId, out var entity))
            {
                return entity;
            }

            return null;
        }

        public void RemoveEntity(int entityId)
        {
            if(_entities.ContainsKey(entityId))
            {
                _entities.Remove(entityId);
            }
        }
    }
}