using System.Collections;

namespace CommonUtilities.Test.InlineData;

public class EmptyStringInLineDataTest : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "" };
        yield return new object[] { "  " };
        yield return new object[] { null };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
