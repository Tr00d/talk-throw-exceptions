using Xunit;
using Xunit.Abstractions;

namespace TalkThrowExceptionsSnippets.Tests.CatInBox;

public class CatInBoxTest
{
    private readonly ITestOutputHelper helper;

    public CatInBoxTest(ITestOutputHelper outputHelper) => this.helper = outputHelper;

    [Fact]
    public void AliveCatSaysMeowMeowMeow() =>
        Box.WithAliveCat(this.GetAliveCat())
            .Shake()
            .Shake()
            .Shake();

    [Fact]
    public void DeadCatSaysNothing() =>
        Box.WithDeadCat()
            .Shake()
            .Shake()
            .Shake();

    [Fact]
    public void ShakesTooHardKillsTheCat() =>
        Box.WithAliveCat(this.GetAliveCat())
            .Shake()
            .ShakeTooHard()
            .Shake();

    private Cat GetAliveCat() => new(this.helper);

    internal class Cat
    {
        private readonly ITestOutputHelper helper;

        public Cat(ITestOutputHelper testOutputHelper) => this.helper = testOutputHelper;

        public void Meow() => this.helper.WriteLine("Meow");
    }

    internal class Box
    {
        private readonly Cat? cat;

        private Box(Cat cat) => this.cat = cat;

        private Box()
        {
        }

        public Box Shake()
        {
            this.cat?.Meow();
            return this;
        }

        public Box ShakeTooHard()
        {
            this.Shake();
            return WithDeadCat();
        }

        public static Box WithAliveCat(Cat cat) => new(cat);

        public static Box WithDeadCat() => new();
    }
}