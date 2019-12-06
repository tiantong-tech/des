using NUnit.Framework;

namespace Wcs.Plc.Test
{
  [TestFixture]
  public class StateTest
  {
    private (StateTestDriver, IntervalManager) SetContext(IState state)
    {
      var driver = new StateTestDriver();
      var manager = new IntervalManager();

      state.SetStateDriver(driver);
      state.SetIntervalManager(manager);
      state.SetKey("D100");
      state.SetLength(1);

      return (driver, manager);
    }

    [TestCase(0)]
    public void TestWordState(int value)
    {
      var state = new StateWord();

      SetContext(state);
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase("happy hacking")]
    public void TestWordsState(string value)
    {
      var state = new StateWords();

      SetContext(state);
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(true)]
    public void TestBitState(bool value)
    {
      var state = new StateBit();
      SetContext(state);
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase("0011")]
    public void TestBitsState(string value)
    {
      var state = new StateBits();
      SetContext(state);
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(1)]
    public void TestStateHook(int value)
    {
      var state = new StateWord();
      var getHookData = 0;
      var setHookData = 0;

      SetContext(state);
      state.AddSetHook(data => setHookData = data);
      state.Set(value);
      Assert.AreEqual(value, setHookData);

      state.AddGetHook(data => getHookData = data);
      state.Get();
      Assert.AreEqual(value, getHookData);
    }

    [Test]
    public void TestStateCollectAndUncollect()
    {
      var state = new StateWord();
      var (_, manager) = SetContext(state);
      var flag = false;

      state.Collect(0);
      state.AddGetHook(value => {
        flag = true;
        state.UncollectAsync();
      });
      state.Set(100);
      manager.Start();
      manager.Wait();
      Assert.IsTrue(flag);
    }

    [Test]
    public void TestStateHearteatUnheartbeat()
    {
      var state = new StateWord();
      var (_, manager) = SetContext(state);
      var flag = false;
      state.Heartbeat(0);
      state.AddSetHook(value => {
        flag = true;
        state.UnheartbeatAsync();
      });
      manager.Start();
      manager.Wait();
      Assert.IsTrue(flag);
    }
  }
}
