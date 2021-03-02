using System.Collections;
using System.Runtime.CompilerServices;
using System;
using Microsoft.CSharp.RuntimeBinder;

namespace QuickMember
{
    internal static class CallSiteCache
    {
        private static readonly Hashtable _getters = new Hashtable();
        private static readonly Hashtable _setters = new Hashtable();

        internal static object GetValue(string name, object target)
        {
            var callSite = (CallSite<Func<CallSite, object, object>>)_getters[name];
            if (callSite == null)
            {
                var newSite = CallSite<Func<CallSite, object, object>>
                    .Create(
                        Binder.GetMember(
                            CSharpBinderFlags.None,
                            name,
                            typeof(CallSiteCache),
                            new CSharpArgumentInfo[] { CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null) }));

                lock (_getters)
                {
                    callSite = (CallSite<Func<CallSite, object, object>>)_getters[name];
                    if (callSite == null)
                    {
                        _getters[name] = callSite = newSite;
                    }
                }
            }
            return callSite.Target(callSite, target);
        }

        internal static void SetValue(string name, object target, object value)
        {
            var callSite = (CallSite<Func<CallSite, object, object, object>>)_setters[name];
            if (callSite == null)
            {
                var newSite = CallSite<Func<CallSite, object, object, object>>
                    .Create(
                        Binder.SetMember(
                            CSharpBinderFlags.None,
                            name,
                            typeof(CallSiteCache),
                            new CSharpArgumentInfo[]
                            {
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null),
                                CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, null)
                            }));

                lock (_setters)
                {
                    callSite = (CallSite<Func<CallSite, object, object, object>>)_setters[name];
                    if (callSite == null)
                    {
                        _setters[name] = callSite = newSite;
                    }
                }
            }
            callSite.Target(callSite, target, value);
        }      
    }
}
