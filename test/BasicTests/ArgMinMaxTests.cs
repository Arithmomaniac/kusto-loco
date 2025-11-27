using AwesomeAssertions;

namespace BasicTests;

[TestClass]
public class ArgMinMaxTests : TestMethods
{
    [TestMethod]
    public async Task Small()
    {
        var query = """
                    datatable(A: int, B:int) [
                    1,2,
                    3,3,
                    ]
                    | summarize arg_max(A) by B
                    """;
        var result = await ResultAsString(query);
        result.Should().Be("2,1,3,3");
    }

    public async Task SmallStar()
    {
        var query = """
                    datatable(A: int, B:int) [
                    1,2,
                    3,3,
                    ]
                    | summarize arg_max(A) by B
                    """;
        var result = await ResultAsLines(query);
        result.Should().Be("""
                           2,1
                           3,3
                           """);
    }

    [TestMethod]
    public async Task SmallA()
    {
        var query = """
                    datatable(A: int, B:int) [
                    1,2,
                    3,3,
                    ]
                    | summarize arg_max(A)
                    """;
        var result = await ResultAsString(query);
        result.Should().Be("3");
    }

    [TestMethod]
    public async Task Small2()
    {
        var query = """
                    datatable(A: int, B:int,C:int) [
                    1,2,7,
                    3,3,7,
                    ]
                    | summarize arg_max(A,C) by B
                    """;
        var result = await ResultAsLines(query);
        result.Should().Be("""
                           2,1,7
                           3,3,7
                           """);
    }

    [TestMethod]
    public async Task MultiColumns()
    {
        var query = """
                    datatable(A: int, B:int,C:int,D:int) [
                    1,2,7,10,
                    1,10,5,5,
                    3,3,7,10,
                    ]
                    | summarize arg_max(B,C,D) by A
                    """;
        var result = await ResultAsLines(query);
        result.Should().Be("""
                           1,10,5,5
                           3,3,7,10
                           """);
    }

    [TestMethod]
    public async Task ArgMaxStar()
    {
        var query = """
                    datatable(Fruit: string, Color: string, Version: int) [
                        "Apple", "Red", 1,
                        "Apple", "Green", int(null),
                        "Banana", "Yellow", int(null),
                        "Banana", "Green", int(null),
                        "Pear", "Brown", 1,
                        "Pear", "Green", 2,
                    ]
                    | summarize arg_max(Version, *)
                    """;

        var result = await LastLineOfResult(query);
        result.Should().Be("2,Pear,Green");
    }

    [TestMethod]
    public async Task ArgMinStar()
    {
        var query = """
                    datatable(Fruit: string, Color: string, Version: long) [
                        "Apple", "Red", -1,
                        "Apple", "Green", int(null),
                        "Banana", "Yellow", int(null),
                        "Banana", "Green", int(null),
                        "Pear", "Brown", 1,
                        "Pear", "Green", 2,
                    ]
                    | summarize arg_min(Version, *)
                    """;

        var result = await LastLineOfResult(query);
        result.Should().Be("-1,Apple,Red");
    }


    [TestMethod]
    public async Task ArgMaxMultiple()
    {
        var query = """
                    datatable(Fruit: string, Color: string, Version: int,Price: long) [
                        "Apple", "Red", 1,100,
                        "Pear", "Brown", 1,200,
                        "Pear", "Green", 2,300,
                    ]
                    | summarize arg_max(Price,Fruit,Color)
                    """;

        var result = await LastLineOfResult(query);
        result.Should().Be("300,Pear,Green");
    }


    [TestMethod]
    public async Task ArgMaxNamed()
    {
        var query = """
                    datatable(Fruit: string, Color: string, Version: long) [
                        "Apple", "Red", 1,
                        "Apple", "Green", 5,
                        "Banana", "Yellow", 6,
                        "Banana", "Green", 7,
                        "Pear", "Brown", 1,
                        "Pear", "Green", 2,
                    ]
                    | summarize arg_max(Version, Color) by Fruit
                    | order by Version asc
                    """;

        var result = await LastLineOfResult(query);
        result.Should().Be("Banana,7,Green");
    }

    [TestMethod]
    public async Task ArgMaxWithCalc()
    {
        var query = """
                    datatable(Name: string, Start: real , End:real) [
                        "Apple", 3, 1,
                        "Pear", 5, 5,
                    ]
                    | summarize arg_max(Start-End,*)
                    """;

        var result = await LastLineOfResult(query);
        result.Should().Be("2,Apple,3,1");
    }
    [TestMethod]
    public async Task ArgMaxWithCalc2()
    {
        var query = """
                    datatable(Name: string, Start: real , End:real) [
                        "Apple", 3, 1,
                        "Pear", 5, 5,
                        "Apple",10, 1,
                    ]
                    | summarize arg_max(Start-End,*) by Name
                    | order by Start asc
                    """;

        var result = await ResultAsString(query,Environment.NewLine);
        result.Should().Be(
            """
            Pear,0,5,5
            Apple,9,10,1
            """

            );
    }

    [TestMethod]
    public async Task Arg_max()
    {
        var query = "print x=1,y=2 | summarize arg_max(x,*) by y";
        var result = await LastLineOfResult(query);
        result.Should().Be("2,1");
    }

    /// <summary>
    /// Based on Microsoft Documentation example:
    /// Find the row with the highest Version for each Fruit, returning all columns
    /// https://learn.microsoft.com/en-us/kusto/query/arg-max-aggregation-function
    /// </summary>
    [TestMethod]
    public async Task ArgMaxStarByFruit_FromDocumentation()
    {
        var query = """
                    datatable(Fruit: string, Color: string, Version: int) [
                        "Apple", "Red", 1,
                        "Apple", "Green", int(null),
                        "Banana", "Yellow", int(null),
                        "Banana", "Green", int(null),
                        "Pear", "Brown", 1,
                        "Pear", "Green", 2,
                    ]
                    | summarize arg_max(Version, *) by Fruit
                    | order by Fruit asc
                    """;

        var result = await ResultAsLines(query);
        // Apple: Version 1 is max (null is ignored), returns Red
        // Banana: both null, arbitrary row picked (either Yellow or Green)
        // Pear: Version 2 is max, returns Green
        result.Should().Contain("Apple,1,Red");
        result.Should().Contain("Pear,2,Green");
    }

    /// <summary>
    /// Based on Microsoft Documentation example:
    /// Find the row with the lowest Version for each Fruit, returning all columns
    /// https://learn.microsoft.com/en-us/kusto/query/arg-min-aggregation-function
    /// </summary>
    [TestMethod]
    public async Task ArgMinStarByFruit_FromDocumentation()
    {
        var query = """
                    datatable(Fruit: string, Color: string, Version: int) [
                        "Apple", "Red", 1,
                        "Apple", "Green", int(null),
                        "Banana", "Yellow", int(null),
                        "Banana", "Green", int(null),
                        "Pear", "Brown", 1,
                        "Pear", "Green", 2,
                    ]
                    | summarize arg_min(Version, *) by Fruit
                    | order by Fruit asc
                    """;

        var result = await ResultAsLines(query);
        // Apple: Version 1 is min (null is ignored), returns Red
        // Banana: both null, arbitrary row picked
        // Pear: Version 1 is min, returns Brown
        result.Should().Contain("Apple,1,Red");
        result.Should().Contain("Pear,1,Brown");
    }

    /// <summary>
    /// Based on Microsoft Documentation example:
    /// arg_min with multiple return columns
    /// https://learn.microsoft.com/en-us/kusto/query/arg-min-aggregation-function
    /// </summary>
    [TestMethod]
    public async Task ArgMinMultipleReturnColumns_FromDocumentation()
    {
        var query = """
                    datatable(State: string, BeginLat: real, BeginLocation: string, EventType: string) [
                        "ALABAMA", 30.5, "Location1", "Tornado",
                        "ALABAMA", 31.2, "Location2", "Hail",
                        "FLORIDA", 25.1, "Location3", "Storm",
                        "FLORIDA", 26.3, "Location4", "Wind",
                    ]
                    | summarize arg_min(BeginLat, BeginLocation, EventType) by State
                    | order by State asc
                    """;

        var result = await ResultAsLines(query);
        // For ALABAMA: min BeginLat is 30.5, so returns Location1, Tornado
        // For FLORIDA: min BeginLat is 25.1, so returns Location3, Storm
        result.Should().Be("""
                           ALABAMA,30.5,Location1,Tornado
                           FLORIDA,25.1,Location3,Storm
                           """);
    }

    /// <summary>
    /// Based on Microsoft Documentation example:
    /// arg_max with multiple return columns
    /// https://learn.microsoft.com/en-us/kusto/query/arg-max-aggregation-function
    /// </summary>
    [TestMethod]
    public async Task ArgMaxMultipleReturnColumns_FromDocumentation()
    {
        var query = """
                    datatable(State: string, BeginLat: real, BeginLocation: string, EventType: string) [
                        "ALABAMA", 30.5, "Location1", "Tornado",
                        "ALABAMA", 31.2, "Location2", "Hail",
                        "FLORIDA", 25.1, "Location3", "Storm",
                        "FLORIDA", 26.3, "Location4", "Wind",
                    ]
                    | summarize arg_max(BeginLat, BeginLocation) by State
                    | order by State asc
                    """;

        var result = await ResultAsLines(query);
        // For ALABAMA: max BeginLat is 31.2, so returns Location2
        // For FLORIDA: max BeginLat is 26.3, so returns Location4
        result.Should().Be("""
                           ALABAMA,31.2,Location2
                           FLORIDA,26.3,Location4
                           """);
    }
}
