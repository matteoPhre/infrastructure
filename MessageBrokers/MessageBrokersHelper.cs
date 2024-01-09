using Infrastructure.Core.Events;
using System;

namespace Infrastructure.MessageBrokers
{
    public static class MessageBrokersHelper
    {
        public static string GetTypeName(Type type)
        {
            var name = type.FullName.ToLower().Replace("+", ".");

            if (type is IEvent)
            {
                name += "_event";
            }

            return name;
        }

        public static string GetTypeName<T>()
        {
            return GetTypeName(typeof(T));
        }

        public static string GetQueueName(string configQueueName, string typeName)
        {
            return string.IsNullOrEmpty(configQueueName) ? (AppDomain.CurrentDomain.FriendlyName).Trim().Trim('_') + "_" + typeName : configQueueName;
        }
    }
}
