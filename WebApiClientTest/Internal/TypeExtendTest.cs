﻿using System;
using System.Collections.Generic;
using System.Text;
using WebApiClient;
using WebApiClient.DataAnnotations;
using Xunit;

namespace WebApiClientTest.Internal
{
    public class TypeExtendTest
    {
        [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
        class A : Attribute
        {
        }

        [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
        class B : Attribute
        {
        }

        class C : B
        {
            [AliasAs("age", Scope = FormatScope.JsonFormat)]
            public int Age { get; set; }

            public string Name { get; set; }
        }

        interface InternalInterface
        {

        }


        public interface IGet
        {
            ITask<string> Get();
        }

        public interface IPost
        {
            ITask<string> Post();
        }

        public interface IMyApi : IGet, IPost
        {
            ITask<int> Delete();
        }

        [Fact]
        public void AllowMultipleTest()
        {
            Assert.False(typeof(A).IsAllowMultiple());
            Assert.True(typeof(B).IsAllowMultiple());
            Assert.True(typeof(C).IsAllowMultiple());
        }

        [Fact]
        public void IsInheritFromTest()
        {
            Assert.True(typeof(C).IsInheritFrom<B>());
            Assert.False(typeof(B).IsInheritFrom<A>());
        }

        [Fact]
        public void IsDefinedFormatScopeTest()
        {
            var age = typeof(C).GetProperty("Age");
            var name = typeof(C).GetProperty("Name");

            Assert.True(age.IsDefinedFormatScope<AliasAsAttribute>(FormatScope.JsonFormat));
            Assert.False(age.IsDefinedFormatScope<AliasAsAttribute>(FormatScope.KeyValueFormat));
            Assert.False(name.IsDefinedFormatScope<AliasAsAttribute>(FormatScope.JsonFormat));
        }

        [Fact]
        public void GetAllApiMethodsTest()
        {
            Assert.Throws<NotSupportedException>(() => typeof(InternalInterface).GetAllApiMethods());

            var m1 = typeof(IMyApi).GetAllApiMethods();
            var m2 = typeof(IMyApi).GetAllApiMethods();

            Assert.True(object.ReferenceEquals(m1, m2));
            Assert.True(m1.Length == 3);
        }
    }


}
