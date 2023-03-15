using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace TalkThrowExceptionsSnippets.Tests.LinqExample;

public class LinqExampleTest
{
    private readonly List<Player> top10Players;

    public LinqExampleTest() =>
        this.top10Players = new List<Player>
        {
            new("Erling", "Haaland", 10),
            new("Kylian", "Mbappé", 6),
            new("Luka", "Modrić", 9),
            new("Kevin", "De Bruyne", 3),
            new("Thibault", "Courtois", 7),
            new("Karim", "Benzema", 1),
            new("Robert", "Lewandowski", 4),
            new("Sadio", "Mané", 2),
            new("Mohamed", "Salah", 5),
            new("Vinicius", "Júnior  ", 8),
        };

    [Fact]
    public void GetTop3Players()
    {
        var output = new string[3];
        foreach (var player in this.top10Players)
        {
            if (player.Rank <= 3)
            {
                output[player.Rank - 1] = player.GetDisplayName();
            }
        }

        output
            .Should()
            .BeEquivalentTo("Karim Benzema", "Sadio Mané", "Kevin De Bruyne");
    }

    [Fact]
    public void GetTop3PlayersWithLinq() =>
        this.top10Players
            .OrderBy(player => player.Rank)
            .Take(3)
            .Select(player => player.GetDisplayName())
            .Should()
            .BeEquivalentTo("Karim Benzema", "Sadio Mané", "Kevin De Bruyne");

    private record Player(string Firstname, string Lastname, int Rank)
    {
        public string GetDisplayName() => string.Join(' ', this.Firstname, this.Lastname);
    }
}