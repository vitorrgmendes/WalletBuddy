using System.Collections;

namespace CommonUtilities.Test.InlineData;

public class InvalidPasswordInLineData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { "" };
        yield return new object[] { "  " };
        yield return new object[] { null };
        yield return new object[] { "a" };
        yield return new object[] { "aa" };
        yield return new object[] { "aaa" };
        yield return new object[] { "aaaa" };
        yield return new object[] { "aaaaa" };
        yield return new object[] { "aaaaaa" };
        yield return new object[] { "aaaaaaa" };
        yield return new object[] { "test123!-" };
        yield return new object[] { "TEST123-!SA" };
        yield return new object[] { "TeSt!@-Ab" };
        yield return new object[] { "Test123Sa4" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
