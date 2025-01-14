using System;
using DHBTestApplication.Domain;
using FluentAssertions;
using NUnit.Framework;

namespace DHBTestApplication.Domain.Tests;

[TestFixture]
[TestOf(typeof(Country))]
public class CountryTest
{
    private Country country;
    [SetUp]
    public void SetUp()=>this.country = new Country();

    [TestCase(0,1,0)]
    [TestCase(0,double.MaxValue,0)]
    [TestCase(1,double.MaxValue,0)]
    [TestCase(int.MaxValue,1,int.MaxValue)]
    [TestCase(int.MaxValue,0.5,4294967294.0)]
    public void CalculateDensity(int population, double area, double expectedOutput)
    {
        this.country.Population = population;
        this.country.Area = area;
        this.country.CalculateDensity().Should().BeApproximately(expectedOutput, 0.0001);
    }

    [TestCase(-1,1)]
    [TestCase(-1,-1)]
    [TestCase(1,0)]
    [TestCase(-1,0)]
    [TestCase(int.MinValue,0)]
    [TestCase(int.MaxValue,0)]
    [TestCase(0,double.MinValue)]
    [TestCase(int.MinValue,double.MinValue)]
    [TestCase(int.MinValue,double.MaxValue)]
    [TestCase(int.MaxValue,double.MinValue)]
    [TestCase(int.MaxValue,0.00009)]
    [TestCase(int.MaxValue,(int.MaxValue)/(double.MaxValue+1))]
    public void CalculateDensityValidation(int population, double area)
    {
        this.country.Population = population;
        this.country.Area = area;
        Assert.Throws<InvalidOperationException>(() => country.CalculateDensity());
    }
}