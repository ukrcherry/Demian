﻿using System;
using System.Linq;
using Shouldly;
using Xunit;

namespace Demian.Tests.Text
{
    public class InMemoryTextTest
    {
        [Theory]
        [InlineData("")]
        [InlineData("Hello, world.")]
        [InlineData("3.14")]
        [InlineData("Ready to set the world on fire, hehehehehe.")]
        public void Ctor_ShouldInitializeCharactersFromProvidedString(string value)
        {
            var text = new InMemoryText(value);

            text.Characters.ShouldBe(value.Select(x => new Character(x)));
        }

        [Fact]
        public void Ctor_ShouldThrowArgumentNullException_IfStringIsNull() =>
            Assert.Throws<ArgumentNullException>(() => new InMemoryText(null));

        [Theory]
        [InlineData("", "Hello!", 0)]
        [InlineData("I was here already.", "Hello!", 0)]
        [InlineData("012345", "6", 1)]
        [InlineData("012345", "6", 2)]
        [InlineData("012345", "6", 3)]
        [InlineData("012345", "666", 4)]
        [InlineData("012345", "666", 5)]
        public void Write_ShouldInsertTextAtSpecifiedIndex(string initial, string toInsert, int at)
        {
            var text = new InMemoryText(initial);
            var textAsString = text.AsString();

            text.Write(toInsert, at);
            textAsString = textAsString.Insert(at, toInsert);

            text.AsString().ShouldBe(textAsString);
        }

        [Fact]
        public void Write_ShouldFail_IfValueIsNull() =>
            new InMemoryText("1").Write(null, 0).Fail.ShouldBe(true);

        [Fact]
        public void Write_ShouldFail_IfIndexIsLessThanZero() =>
            new InMemoryText("1").Write("", -1).Fail.ShouldBe(true);
        
        [Fact]
        public void Write_ShouldFail_IfIndexIsGreaterThanText() =>
            new InMemoryText("1234").Write("", 5).Fail.ShouldBe(true);
    }
}