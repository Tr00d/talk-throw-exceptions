using System;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace TalkThrowExceptionsSnippets.Tests.CatInBox;

public class CatInBoxTest
{
    private readonly ITestOutputHelper helper;

    public CatInBoxTest(ITestOutputHelper outputHelper) => this.helper = outputHelper;

    [Fact]
    public void AliveCatMeowsMeowsMeows() =>
        SchrodingerBox.WithAliveCat(this.GetAliveCat())
            .Shake()
            .Shake()
            .Shake();

    [Fact]
    public void OpenBox_ReturnsAliveCat_WhenCatIsAlive() =>
        SchrodingerBox.WithAliveCat(this.GetAliveCat())
            .Shake()
            .OpenBox()
            .Should()
            .Be(this.GetAliveCat());

    [Fact]
    public void OpenBox_ReturnsDeadCat_WhenCatIsDead() =>
        SchrodingerBox.WithAliveCat(this.GetAliveCat())
            .ShakeTooHard()
            .OpenBox()
            .Should()
            .BeNull();

    [Fact]
    public void ShakeTooHardUnfortunatelyKillsTheCat() =>
        SchrodingerBox.WithAliveCat(this.GetAliveCat())
            .Shake()
            .ShakeTooHard()
            .Shake();

    private Cat GetAliveCat() => new(value => this.helper.WriteLine(value));

    internal readonly struct Cat
    {
        private readonly Action<string> log;

        public Cat(Action<string> log) => this.log = log;

        public void Meow() => this.log("Meow");
    }

    internal class SchrodingerBox
    {
        private readonly Cat? cat;

        // Creates a box with an alive cat inside
        private SchrodingerBox(Cat cat) => this.cat = cat;

        // Creates a box with a dead cat
        private SchrodingerBox()
        {
        }

        // Look inside the box
        public Cat? OpenBox() => this.cat;

        // Shakes the box. The cat doesn't like it. Meow.
        public SchrodingerBox Shake()
        {
            this.cat?.Meow();
            return this;
        }

        // Shakes the box (too hard), then returns a new box with a dead cat
        public SchrodingerBox ShakeTooHard()
        {
            this.Shake();
            return new SchrodingerBox();
        }

        // Creates a box with a cat
        public static SchrodingerBox WithAliveCat(Cat cat) => new(cat);
    }
}