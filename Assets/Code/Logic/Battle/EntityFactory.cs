using System;
using MiniRPG.Common;

namespace MiniRPG.Logic.Battle
{
    public interface IEntityFactory
    {
        Entity CreateEntity(Type entityType, string name, int entityId = -1);
    }

    public static class EntityFactoryExtensions
    {
        public static T CreateEntity<T>(this IEntityFactory entityFactory, string name, int entityId = -1) where T : Entity
        {
            return entityFactory.CreateEntity(typeof(T), name, entityId) as T;
        }
    }

    public class EntityFactory : IEntityFactory
    {
        private const int MIN_ENTITY_ID = 1000;
        private int _nextEntityId;
        private BattleSimulation _battleSimulation;
        private ILogger _logger;

        public EntityFactory(BattleSimulation battleSimulation, ILogger logger)
        {
            _nextEntityId = MIN_ENTITY_ID;
            _battleSimulation = battleSimulation;
            _logger = logger;
        }

        public Entity CreateEntity(Type entityType, string name, int entityId = -1)
        {
            if(!typeof(Entity).IsAssignableFrom(entityType))
            {
                _logger.LogError($"Cannot create entity of type : {entityType}. All entity types must inherit from {nameof(Entity)}");
                return null;
            }

            //assign new entity id if not given as argument
            if(entityId == -1)
            {
                entityId = _nextEntityId;
                _nextEntityId++;
            }else
            {
                //for avoiding id conflict
                _nextEntityId = Math.Max(_nextEntityId, entityId) + 1;
            }

            var entity = Activator.CreateInstance(entityType, name, entityId, _battleSimulation, _logger) as Entity;

            _logger.LogDebug($"Enitty of type  {entityType} and id {entityId} created.");

            return entity;
        }
    }
}