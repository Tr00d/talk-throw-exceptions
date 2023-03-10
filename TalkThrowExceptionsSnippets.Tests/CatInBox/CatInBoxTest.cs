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
    public void DeadCatIsSadlyDead() =>
        SchrodingerBox.WithDeadCat()
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

    private Cat GetAliveCat() => new(this.helper);

    internal readonly struct Cat
    {
        private readonly ITestOutputHelper helper;

        public Cat(ITestOutputHelper testOutputHelper) => this.helper = testOutputHelper;

        public void Meow() => this.helper.WriteLine("Meow");
    }

    internal class SchrodingerBox
    {
        private readonly Cat? cat;

        private SchrodingerBox(Cat cat) => this.cat = cat;

        private SchrodingerBox()
        {
        }

        public Cat? OpenBox() => this.cat;

        public SchrodingerBox Shake()
        {
            this.cat?.Meow();
            return this;
        }

        public SchrodingerBox ShakeTooHard()
        {
            this.Shake();
            return WithDeadCat();
        }

        public static SchrodingerBox WithAliveCat(Cat cat) => new(cat);

        public static SchrodingerBox WithDeadCat() => new();
    }
}