using System;
using FluentAssertions;
using NUnit.Framework;

namespace CSVParser.UnitTests
{
    [TestFixture]
    public class StringTruncationTests
    {
        [Test]
        public void Truncate_null_should_return_null()
        {
            // arrange
            var sut = (string)null;
            // act
            var result = sut.Truncate(2);
            // assert
            result.Should().BeNull();
        }

        [Test]
        public void Truncate_source_is_empty_should_return_empty()
        {
            // arrange
            var sut = string.Empty;
            // act
            var result = sut.Truncate(2);
            // assert
            result.Should().BeEmpty();
        }

        [Test]
        public void Truncate_maxLenght_lower_than_string_length_should_throw_argument_out_of_range_exception()
        {
            // arrange
            var sut = "TestString";
            // act
            Action act = () => sut.Truncate(-1);
            // assert
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [TestCase("abcdef", 2, ExpectedResult = "ab")]
        [TestCase("abcdef", 0, ExpectedResult = "")]
        public string Truncategood_arguments_should_return_proper_result(string source, int length)
        {
            // arrange
            // act
            var actual = source.Truncate(length);
            // assert
            TestContext.Out.WriteLine($"actual = '{actual}'");
            return actual;
        }
    }
}
