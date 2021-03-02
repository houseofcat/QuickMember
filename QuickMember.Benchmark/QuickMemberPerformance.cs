using BenchmarkDotNet.Attributes;
using System;
using System.ComponentModel;
using System.Reflection;

namespace QuickMember.Benchmark
{
    public class QuickMemberPerformance
    {
        public string Value { get; set; }

        private QuickMemberPerformance _obj;
        private dynamic _dlr;
        private PropertyInfo _prop;
        private PropertyDescriptor _descriptor;

        private TypeAccessor _accessor;
        private ObjectAccessor _wrapped;

        private Type _type;

        [GlobalSetup]
        public void Setup()
        {
            _obj = new QuickMemberPerformance();
            _dlr = _obj;
            _prop = typeof(QuickMemberPerformance).GetProperty("Value");
            _descriptor = TypeDescriptor.GetProperties(_obj)["Value"];

            // QuickMember specific code
            _accessor = QuickMember.TypeAccessor.Create(typeof(QuickMemberPerformance));
            _wrapped = QuickMember.ObjectAccessor.Create(_obj);

            _type = typeof(QuickMemberPerformance);
        }

        [Benchmark(Description = "1. Static C#", Baseline = true)]
        public string StaticCSharp()
        {
            _obj.Value = "abc";
            return _obj.Value;
        }

        [Benchmark(Description = "2. Dynamic C#")]
        public string DynamicCSharp()
        {
            _dlr.Value = "abc";
            return _dlr.Value;
        }

        [Benchmark(Description = "3. PropertyInfo")]
        public string PropertyInfo()
        {
            _prop.SetValue(_obj, "abc", null);
            return (string)_prop.GetValue(_obj, null);
        }

        [Benchmark(Description = "4. PropertyDescriptor")]
        public string PropertyDescriptor()
        {
            _descriptor.SetValue(_obj, "abc");
            return (string)_descriptor.GetValue(_obj);
        }

        [Benchmark(Description = "5. TypeAccessor.Create")]
        public string TypeAccessor()
        {
            _accessor[_obj, "Value"] = "abc";
            return (string)_accessor[_obj, "Value"];
        }

        [Benchmark(Description = "6. ObjectAccessor.Create")]
        public string ObjectAccessor()
        {
            _wrapped["Value"] = "abc";
            return (string)_wrapped["Value"];
        }

        [Benchmark(Description = "7. c# new()")]
        public QuickMemberPerformance CSharpNew()
        {
            return new QuickMemberPerformance();
        }

        [Benchmark(Description = "8. Activator.CreateInstance")]
        public object ActivatorCreateInstance()
        {
            return Activator.CreateInstance(_type);
        }

        [Benchmark(Description = "9. TypeAccessor.CreateNew")]
        public object TypeAccessorCreateNew()
        {
            return _accessor.CreateNew();
        }
    }
}