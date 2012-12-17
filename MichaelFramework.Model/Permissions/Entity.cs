using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace MichaelFramework.Model.Permissions
{
    public enum eEntityLevel
    {
        Global=0,
        Class,
        Entity,
        Property
    }

    public class Entity
    {
        public Entity()
        {

        }

        public Entity(eEntityLevel entityLevel, string classId,
            string entityId, string propertyId)
        {
            EntityLevel = entityLevel;
            ClassId = classId;
            EntityId = entityId;
            PropertyId = propertyId;
        }

        [Property("a_entitylevel")]
        public eEntityLevel EntityLevel { get; set; }

        [Property("a_classId")]
        public string ClassId { get; set; }

        [Property("a_entityId")]
        public string EntityId { get; set; }

        [Property("a_propertyId")]
        public string PropertyId { get; set; }
    }
}
