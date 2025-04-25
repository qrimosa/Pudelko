using PudelkoLib;
using Xunit;

namespace PudelkoTests;

public class PudelkoTests
{
    [Theory]
    [InlineData(2.0, 3.0, 4.0, 24.0)]
    [InlineData(0.1, 0.1, 0.1, 0.001)]
    [InlineData(1.5, 2.5, 3.5, 13.125)]
    public void Objetosc_ReturnsCorrectVolume(double a, double b, double c, double expected)
    {
        var box = new Pudelko(a, b, c);
        Assert.Equal(expected, box.Objetosc(), 9);
    }

    [Theory]
    [InlineData(2.0, 3.0, 4.0, 52.000000)]
    [InlineData(0.1, 0.1, 0.1, 0.060000)]
    [InlineData(1.5, 2.5, 3.5, 35.500000)]
    public void Pole_ReturnsCorrectSurfaceArea(double a, double b, double c, double expected)
    {
        var box = new Pudelko(a, b, c);
        Assert.Equal(expected, box.Pole(), 6);
    }

    [Fact]
    public void OperatorPlus_CombinesBoxesCorrectly()
    {
        var box1 = new Pudelko(1.0, 2.0, 3.0);
        var box2 = new Pudelko(1.5, 2.5, 0.5);
        var combined = box1 + box2;

        Assert.Equal(1.000, combined.A, 3);
        Assert.Equal(2.000, combined.B, 3);
        Assert.Equal(5.500, combined.C, 3);
    }

    [Theory]
    [InlineData(1.0, 2.0, 3.0, 3.0, 2.0, 1.0, true)]
    [InlineData(1.0, 1.0, 1.0, 1.0, 1.0, 1.001, false)]
    public void OperatorEquals_ComparesBoxesCorrectly(
        double a1, double b1, double c1,
        double a2, double b2, double c2,
        bool expectedEqual)
    {
        var box1 = new Pudelko(a1, b1, c1);
        var box2 = new Pudelko(a2, b2, c2);

        Assert.Equal(expectedEqual, box1 == box2);
    }
}
