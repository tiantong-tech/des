using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class IntervalTest
  {
    [TestCase]
    public void TestStart()
    {
      bool flag = false;
      var interval = new Interval();

      interval.SetTimes(1).SetHandler(() => flag = true);
      interval.Start().Wait();

      Assert.IsTrue(flag);
    }

    [TestCase(1)]
    [TestCase(99)]
    public void TestWithTimes(int n)
    {
      var times = 0;
      var interval = new Interval();

      interval.SetTime(0).SetTimes(n).SetHandler(() => times++);
      interval.Start().Wait();

      Assert.AreEqual(n, times);
    }

    [Test]
    public void TestWithoutTimes()
    {
      var times = 0;
      var interval = new Interval();
      interval.SetHandler(() => times++);
      interval.Start();
      interval.Stop();

      Assert.True(true);
    }

    [TestCase(0, 1)]
    [TestCase(1, 1)]
    [TestCase(10, 10)]
    public void TestMultipleRun(int n, int m)
    {
      var times = 0;
      var interval = new Interval();

      interval.SetTimes(m).SetHandler(() => times++);
      for (var i = 0; i < n; i++) {
        interval.Start().Wait();
      }

      Assert.AreEqual(times, n * m);
    }
  }
}
