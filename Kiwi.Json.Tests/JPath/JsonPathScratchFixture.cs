﻿using System.Linq;
using Kiwi.Json.JPath;
using Kiwi.Json.Untyped;
using NUnit.Framework;
using SharpTestsEx;

namespace Kiwi.Json.Tests.JPath
{
    [TestFixture]
    public class JsonPathScratchFixture
    {
        [Test]
        public void ConstructorThrowsIfCannotParse()
        {
            Assert.Throws<JsonPathException>(() => new JsonPath(@"$."));
            Assert.Throws<JsonPathException>(() => new JsonPath(@"$.["));
            Assert.Throws<JsonPathException>(() => new JsonPath(@"$.A["));
            Assert.Throws<JsonPathException>(() => new JsonPath(@"$.A[]"));

            Assert.Throws<JsonPathException>(() => new JsonPath(@"A"));
        }

        [Test]
        public void GetValue()
        {
            var jpath = new JsonPath(@"$.A[""B""][2]");

            var j = JsonConvert.ToJson(new { A = new { B = new[] { 1, 2, 3 } } });

            var a = jpath.Evaluate(j);

            jpath.Evaluate(j)
                .Should().Have.Count.EqualTo(1);

            jpath.Evaluate(j)
                .First()
                .Should().Be.InstanceOf<IJsonInteger>()
                .And.Value.Value.Should().Be.EqualTo(3);
        }

        [Test]
        public void Test()
        {
            var json = JsonConvert.ToJson(new { A = "a", B = new { X = 1, Y = 2 } });

            json.JsonPathValues().Select(v => v.Path.Path).ToArray()
                .Should().Have.SameSequenceAs("$.A", "$.B.X", "$.B.Y");

            json.JsonPathValues().Select(v => v.Value.ToObject()).ToArray()
                .Should().Have.SameSequenceAs("a", (long)1, (long)2);
        }
    }
}