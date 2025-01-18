using Asjc.Natex.Matchers;

namespace Asjc.Natex.Tests
{
    [TestClass]
    public class NatexTests
    {
        [TestMethod]
        public void CaseInsensitive()
        {
            Natex natex = new("A");
            Assert.IsTrue(natex.Match("a"));
            natex.CaseInsensitive = false;
            Assert.IsFalse(natex.Match("a"));
        }

        [TestMethod]
        public void PartialMode()
        {
            Natex natex = new("A");
            Assert.IsFalse(natex.Match("AB"));
            Assert.IsFalse(natex.Match("BAB"));
            natex.Mode = NatexMode.Partial;
            Assert.IsTrue(natex.Match("AB"));
            Assert.IsTrue(natex.Match("BAB"));
        }

        [TestMethod]
        public void ParallelMatch()
        {
            Natex natex = new();
            Parallel.For(0, 1024, i => natex.Match(i));
        }

        [TestMethod]
        public void AnythingMatcher()
        {
            Assert.IsFalse(new Natex("").Match(new object()));
            Assert.IsTrue(new Natex("*").Match(new object()));
        }

        [TestMethod]
        public void Comparison()
        {
            Assert.IsTrue(new Natex(">0").Match(1));
            Assert.IsTrue(new Natex("＞0").Match(1));
            Assert.IsTrue(new Natex(">=1").Match(1));
            Assert.IsTrue(new Natex("≥1").Match(1));
            Assert.IsTrue(new Natex(">2024/1/1 00:00:00").Match(new DateTime(2024, 1, 1, 12, 0, 0)));
            Assert.IsTrue(new Natex("6/1").Match(DateTime.Parse("6/1")));
        }

        [TestMethod]
        public void Comparison_Invalid()
        {
            Assert.IsFalse(new Natex(">foo").Match(0));
        }

        [TestMethod]
        public void List()
        {
            Assert.IsTrue(new Natex("a,b").Match(new List<string>() { "a", "b" }));
            Assert.IsFalse(new Natex("b,a").Match(new List<string>() { "a", "b" }));
            Assert.IsTrue(new Natex("0,>0") { Mode = NatexMode.Partial }.Match(new int[] { 0, 1, 2 }));
        }

        [TestMethod]
        public void MultiPattern()
        {
            Assert.IsTrue(new Natex("Text:H. Number:1").Match(new { Text = "Hi", Number = 1 }));
        }

        [TestMethod]
        public void MultiPattern_ExtraSpace()
        {
            Assert.IsTrue(new Natex("* *").Match(new object()));
            Assert.IsFalse(new Natex(" * *").Match(new object()));
        }

        [TestMethod]
        public void Negation()
        {
            Assert.IsTrue(new Natex("!1").Match(0));
            Assert.IsFalse(new Natex("!1").Match(1));
        }

        [TestMethod]
        public void NullOrEmpty()
        {
            Assert.IsTrue(new Natex("null").Match(null));
            Assert.IsTrue(new Natex("empty").Match(string.Empty));
            Assert.IsTrue(new Natex("empty").Match(Array.Empty<int>()));
        }

        [TestMethod]
        public void Property()
        {
            Assert.IsTrue(new Natex("Length:1").Match(" "));
            Assert.IsTrue(new Natex("Number:0").Match(new { Number = 0 }));
            Assert.IsTrue(new Natex("Number:>0").Match(new { Number = 1 }));
            Assert.IsTrue(new Natex("Text.Length:1").Match(new { Text = " " }));
        }

        [TestMethod]
        public void Property_DefaultPaths()
        {
            Natex natex = new("3");
            natex.Matchers.Get<PropertyMatcher>()!.DefaultPaths = [["Foo"], ["Text", "Length"], ["Foo"]];
            Assert.IsTrue(natex.Match(new { Text = "ABC", Number = 1 }));
        }

        [TestMethod]
        public void Property_DefaultPaths_DateTime()
        {
            var now = DateTime.Now;
            Natex natex = new(now.ToString());
            natex.Matchers.Get<PropertyMatcher>()!.DefaultPaths = [["DateTime"]];
            Assert.IsTrue(natex.Match(new { DateTime = now }));
        }

        [TestMethod]
        public void Range() => Assert.IsTrue(new Natex("1-3").Match(2));

        [TestMethod]
        public void Regex()
        {
            Assert.IsTrue(new Natex("A.*D").Match("ABCD"));
            Assert.IsTrue(new Natex("/ABC/").Match("ABC"));
        }

        [TestMethod]
        public void Regex_Invalid()
        {
            Natex natex = new("\\");
            natex.Match("");
            natex.Match("");
        }

        [TestMethod]
        public void String() => Assert.IsTrue(new Natex("{ Text = A, Number = 1 }").Match(new { Text = "A", Number = 1 }));


        [TestMethod]
        public void Variable()
        {
            Assert.IsTrue(new Natex("{Today}").Match(DateTime.Today));
            Assert.IsTrue(new Natex("-{UserName}-").Match($"-{Environment.UserName}-"));
        }
    }
}