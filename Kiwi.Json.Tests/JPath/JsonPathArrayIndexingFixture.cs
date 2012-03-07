using System;
using System.Collections.Generic;
using System.Linq;
using Kiwi.Json.JPath;
using NUnit.Framework;

namespace Kiwi.Json.Tests.JPath
{
    [TestFixture]
    public class JsonPathArrayIndexingFixture
    {
        public string JPath { get; set; }
        public object InputObject { get; set; }
        public object ExpectedResult { get; set; }

        public IEnumerable<TestCaseData> TestCaseData
        {
            get { return ArrayIndexingTestData.Concat(ObjectMemberTestData).Concat(RecusiveDescentTestData); }
        }

        public IEnumerable<TestCaseData> ArrayIndexingTestData
        {
            get
            {
                yield return new TestCaseData("$[*]", new[] {1, 2, 3}).Returns(new[] {1, 2, 3});
                yield return new TestCaseData("$[1]", new[] {1, 2, 3}).Returns(new[] {2});
                yield return new TestCaseData("$[-1:]", new[] {1, 2, 3}).Returns(new[] {3});
                yield return new TestCaseData("$[0,1,4]", new[] {1, 2, 3, 4, 5}).Returns(new[] {1, 2, 5});
                yield return
                    new TestCaseData("$[100]", new[] {1, 2, 3}).Returns(new int[0]).SetDescription(
                        "Array index out of bounds silently ignored");
            }
        }

        public IEnumerable<TestCaseData> ObjectMemberTestData
        {
            get
            {
                yield return new TestCaseData("$.MissingProperty", new {Title = "Hello"}).Returns(new object[0]);
                yield return new TestCaseData("$.Title", new {Title = "Hello"}).Returns(new[] {"Hello"});
                yield return new TestCaseData("$[\"Title\"]", new {Title = "Hello"}).Returns(new[] {"Hello"});
            }
        }

        public IEnumerable<TestCaseData> RecusiveDescentTestData
        {
            get
            {
                yield return new TestCaseData("$..*", new
                                                          {
                                                              Foo = "D",
                                                              Data = new[] {1, 2, 3},
                                                              Extra = new
                                                                          {
                                                                              Bar = "X"
                                                                          }
                                                          }).Returns(new object[] {"D", 1, 2, 3, "X"});
                yield return new TestCaseData("$..Data[0]", new
                                                                {
                                                                    Foo = "D",
                                                                    Data = new[] {1, 2, 3},
                                                                    Extra = new
                                                                                {
                                                                                    Bar = "X",
                                                                                    Data = new[] {"hit", "miss", "miss"},
                                                                                }
                                                                }).Returns(new object[] {1, "hit"});
            }
        }


        [TestCaseSource("TestCaseData")]
        public object[] TestJsonPath(string jpath, object inputObject)
        {
            Console.Out.WriteLine("jpath: " + jpath);
            Console.Out.WriteLine("input object (as json): " + JSON.Write(inputObject));
            var result = (from v in new JsonPath(jpath).Evaluate(JSON.ToJson(inputObject))
                          select v.ToObject()).ToArray();

            Console.Out.WriteLine("result to native (as json): " + JSON.Write(result));
            return result;
        }
    }
}