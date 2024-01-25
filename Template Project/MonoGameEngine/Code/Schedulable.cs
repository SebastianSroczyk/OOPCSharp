using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MonoGameEngine
{
    internal sealed class Schedulable
    {
        private readonly object _callingObject;
        private readonly string _methodName;
        private readonly object[] _arguments;

        private float _scheduleFor;

        internal Schedulable(object callingObject, string methodName, object[] arguments, float scheduleFor)
        {
            _callingObject = callingObject;
            _methodName = methodName;
            _scheduleFor = scheduleFor;
            if (arguments == null)
                _arguments = new object[] { };
            else
                _arguments = arguments;
        }

        internal void Invoke()
        {
            //var method = CreateDelegate(_callingObject.GetType().GetMethod(_methodName, BindingFlags.NonPublic | BindingFlags.Instance));
            //method.DynamicInvoke(_arguments);
            var method = _callingObject.GetType().GetMethod(_methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_callingObject, _arguments);
        }

        internal void UpdateTime(float deltaTime)
        {
            _scheduleFor -= deltaTime;
            _scheduleFor = MathF.Max(0, _scheduleFor);
        }

        internal bool IsReadyToInvoke()
        {
            return _scheduleFor == 0;
        }

        internal Delegate CreateDelegate(MethodInfo method)
        {
            if(method == null)
            {
                throw new ArgumentNullException("method");
            }
            if (!method.IsStatic)
            {
                //throw new ArgumentException("The provided method must be static.", "method");
            }
            if (method.IsGenericMethod)
            {
                throw new ArgumentException("The provided method must not be generic.", "method");
            }

            return method.CreateDelegate(Expression.GetDelegateType(
                (from parameter in method.GetParameters() select parameter.ParameterType)
                .Concat(new[] { method.ReturnType })
                .ToArray()));
        }
    }
}
