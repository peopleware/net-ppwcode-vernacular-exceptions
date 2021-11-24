// Copyright 2019 by PeopleWare n.v..
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using NUnit.Framework;

namespace PPWCode.Vernacular.Exceptions.IV.Tests
{
    [TestFixture]
    public class DummyTest
    {
        [Test]
        public void TestAdd()
        {
            // arrange
            long x = 2;
            int y = 5;

            // act
            long sum = x + y;

            // assert
            Assert.That(sum, Is.EqualTo(7));
        }

        [Test]
        public void TestFibonacci()
        {
            // arrange
            static long fibonacci(long x)
                => x == 0
                       ? 0
                       : x == 1
                           ? 1
                           : fibonacci(x - 1) + fibonacci(x - 2);

            // act
            long z = fibonacci(21);

            // assert
            Assert.That(z, Is.EqualTo(10946));
        }
    }
}
