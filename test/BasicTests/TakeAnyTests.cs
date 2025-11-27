using AwesomeAssertions;
using NotNullStrings;

namespace BasicTests;

[TestClass]
public class TakeAnyTests : TestMethods
{

    [TestMethod] public async Task TakeAnyMultiple()
    {
        // Arrange
        var query = """
                    datatable(x:long, val:string)
                    [
                        0, 'first',
                        1, 'second',
                        2, 'third',
                        3, 'fourth',
                    ]
                    | summarize take_any(val,x)
                    """;
        var result = await LastLineOfResult(query);
        result.Should().ContainAny("first second third fourth".Tokenize());

    }

    [TestMethod]
    public async Task TakeAnySingle()
    {
        // Arrange
        var query = """
                    datatable(x:long, val:string)
                    [
                        0, 'first',
                        1, 'second',
                        2, 'third',
                        3, 'fourth',
                    ]
                    | summarize take_any(val)
                    """;
        var result = await LastLineOfResult(query);
        result.Should().ContainAny("first second third fourth".Tokenize());

    }
    [TestMethod]
    public async Task TakeAnyStar()
    {
        // Arrange
        var query = """
                    datatable(x:long, val:string)
                    [
                        0, 'first',
                        1, 'second',
                        2, 'third',
                        3, 'fourth',
                    ]
                    | summarize take_any(*)
                    """;
        var result = await LastLineOfResult(query);
        result.Should().ContainAny("first second third fourth".Tokenize());

    }

    /// <summary>
    /// Based on Microsoft Documentation example:
    /// Get a random event for each State beginning with 'A'
    /// https://learn.microsoft.com/en-us/kusto/query/take-any-aggregation-function
    /// </summary>
    [TestMethod]
    public async Task TakeAnyByState_FromDocumentation()
    {
        var query = """
                    datatable(State: string, StartTime: datetime, EpisodeId: long, EventType: string) [
                        "ALABAMA", datetime(2023-01-01), 1, "Tornado",
                        "ALABAMA", datetime(2023-01-02), 2, "Hail",
                        "ARIZONA", datetime(2023-02-01), 3, "Storm",
                        "ARIZONA", datetime(2023-02-02), 4, "Wind",
                        "ARKANSAS", datetime(2023-03-01), 5, "Flood",
                    ]
                    | where State startswith "A"
                    | summarize take_any(*) by State
                    | order by State asc
                    """;
        var result = await ResultAsLines(query);
        // Each state should return one row with some event type
        result.Should().Contain("ALABAMA");
        result.Should().Contain("ARIZONA");
        result.Should().Contain("ARKANSAS");
    }

    /// <summary>
    /// Based on Microsoft Documentation example:
    /// Use take_anyif to filter by predicate
    /// https://learn.microsoft.com/en-us/kusto/query/take-anyif-aggregation-function
    /// </summary>
    [TestMethod]
    public async Task TakeAnyIfBasic()
    {
        var query = """
                    datatable(EventType: string, EventNarrative: string) [
                        "Tornado", "strong wind damage",
                        "Hail", "ice damage",
                        "Wind", "strong wind damage",
                        "Flood", "water damage",
                    ]
                    | summarize take_anyif(EventType, EventNarrative has "strong wind")
                    """;
        var result = await LastLineOfResult(query);
        // Should return either "Tornado" or "Wind" since both have "strong wind" in narrative
        result.Should().ContainAny(["Tornado", "Wind"]);
    }

    /// <summary>
    /// take_anyif with no matching predicate should return null
    /// </summary>
    [TestMethod]
    public async Task TakeAnyIfNoMatch()
    {
        var query = """
                    datatable(EventType: string, EventNarrative: string) [
                        "Tornado", "wind damage",
                        "Hail", "ice damage",
                    ]
                    | summarize take_anyif(EventType, EventNarrative has "earthquake")
                    """;
        var result = await LastLineOfResult(query);
        // Should return empty/null since no narratives contain "earthquake"
        result.Should().BeEmpty();
    }

    /// <summary>
    /// take_anyif with by clause to group results
    /// </summary>
    [TestMethod]
    public async Task TakeAnyIfWithGroupBy()
    {
        var query = """
                    datatable(State: string, EventType: string, Severity: int) [
                        "ALABAMA", "Tornado", 5,
                        "ALABAMA", "Hail", 2,
                        "FLORIDA", "Storm", 3,
                        "FLORIDA", "Wind", 1,
                    ]
                    | summarize take_anyif(EventType, Severity > 2) by State
                    | order by State asc
                    """;
        var result = await ResultAsLines(query);
        // ALABAMA: both Tornado(5) and Hail(2) - only Tornado matches Severity > 2
        // FLORIDA: Storm(3) and Wind(1) - only Storm matches Severity > 2
        result.Should().Contain("ALABAMA,Tornado");
        result.Should().Contain("FLORIDA,Storm");
    }

    /// <summary>
    /// take_anyif with long type
    /// </summary>
    [TestMethod]
    public async Task TakeAnyIfLongType()
    {
        var query = """
                    datatable(Id: long, IsActive: bool) [
                        1, true,
                        2, false,
                        3, true,
                        4, false,
                    ]
                    | summarize take_anyif(Id, IsActive == true)
                    """;
        var result = await LastLineOfResult(query);
        // Should return either 1 or 3 since both have IsActive == true
        result.Should().ContainAny(["1", "3"]);
    }

    /// <summary>
    /// take_anyif with int type
    /// </summary>
    [TestMethod]
    public async Task TakeAnyIfIntType()
    {
        var query = """
                    datatable(Value: int, Flag: bool) [
                        10, true,
                        20, false,
                        30, true,
                    ]
                    | summarize take_anyif(Value, Flag)
                    """;
        var result = await LastLineOfResult(query);
        // Should return either 10 or 30 since both have Flag == true
        result.Should().ContainAny(["10", "30"]);
    }

    /// <summary>
    /// take_anyif with real/double type
    /// </summary>
    [TestMethod]
    public async Task TakeAnyIfRealType()
    {
        var query = """
                    datatable(Value: real, IsPositive: bool) [
                        1.5, true,
                        -2.5, false,
                        3.5, true,
                    ]
                    | summarize take_anyif(Value, IsPositive)
                    """;
        var result = await LastLineOfResult(query);
        // Should return either 1.5 or 3.5 since both have IsPositive == true
        result.Should().ContainAny(["1.5", "3.5"]);
    }
}
