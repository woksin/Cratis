// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Aksio.Cratis.Events.Store;

namespace Aksio.Cratis.Events.Observation
{
    /// <summary>
    /// Represents an implementation of <see cref="IObserverInvoker"/>.
    /// </summary>
    public class ObserverInvoker : IObserverInvoker
    {
        readonly Dictionary<EventType, MethodInfo> _methodsByEventTypeId;
        readonly IServiceProvider _serviceProvider;
        readonly IEventTypes _eventTypes;
        readonly Type _targetType;

        /// <inheritdoc/>
        public IEnumerable<EventType> EventTypes => _methodsByEventTypeId.Keys;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObserverInvoker"/> class.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> for creating instances of actual observer.</param>
        /// <param name="eventTypes"><see cref="IEventTypes"/> for mapping types.</param>
        /// <param name="targetType">Type of observer.</param>
        public ObserverInvoker(IServiceProvider serviceProvider, IEventTypes eventTypes, Type targetType)
        {
            _serviceProvider = serviceProvider;
            _eventTypes = eventTypes;
            _targetType = targetType;

            // TODO: Make a choice; can we have multiple methods handling the same event -
            // if so, make this either throw an exception if duplicates, or array of methods if allowed
            _methodsByEventTypeId = targetType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                                            .Where(_ => IsObservingMethod(_))
                                            .ToDictionary(_ => _eventTypes.GetEventTypeFor(_.GetParameters()[0].ParameterType), _ => _);
        }

        /// <inheritdoc/>
        public Task Invoke(object content, EventContext eventContext)
        {
            var actualObserver = _serviceProvider.GetService(_targetType);
            var eventType = _eventTypes.GetEventTypeFor(content.GetType());

            object returnValue = null!;
            if (_methodsByEventTypeId.ContainsKey(eventType))
            {
                var method = _methodsByEventTypeId[eventType];
                var parameters = method.GetParameters();

                if (parameters.Length == 2)
                {
                    returnValue = (Task)method.Invoke(actualObserver, new object[] { content, eventContext })!;
                }
                else
                {
                    returnValue = (Task)method.Invoke(actualObserver, new object[] { content })!;
                }
            }
            if (returnValue != null) return (Task)returnValue;

            return Task.CompletedTask;
        }

        bool IsObservingMethod(MethodInfo methodInfo)
        {
            var isObservingMethod = methodInfo.ReturnType.IsAssignableTo(typeof(Task)) ||
                                    methodInfo.ReturnType == typeof(void);

            if (!isObservingMethod) return false;
            var parameters = methodInfo.GetParameters();
            if (parameters.Length >= 1)
            {
                isObservingMethod = _eventTypes.HasFor(parameters[0].ParameterType);
                if (parameters.Length == 2)
                {
                    isObservingMethod &= parameters[1].ParameterType == typeof(EventContext);
                }
                else if (parameters.Length > 2)
                {
                    isObservingMethod = false;
                }
                return isObservingMethod;
            }

            return false;
        }
    }
}
