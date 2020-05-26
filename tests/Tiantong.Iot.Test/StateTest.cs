using System.Threading.Tasks;
using NUnit.Framework;

namespace Tiantong.Iot.Test
{
  [TestFixture]
  public class StateTest
  {
    public T ResolveState<T, U>() where T : State<U>, new()
    {
      var state = new T();
      var driver = new StateTestDriverProvider().Resolve();

      state.Name("test").Address("D100").Build(driver);

      return state;
    }

    [TestCase(true)]
    public void TestStateBool(bool value)
    {
      var state = ResolveState<StateBool, bool>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(1000)]
    public void TestStateUInt16(int value)
    {
      var state = ResolveState<StateUInt16, ushort>();
      state.Set((ushort) value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(0)]
    [TestCase(10)]
    [TestCase(1000)]
    public void TestStateInt32(int value)
    {
      var state = ResolveState<StateInt32, int>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase("hello word")]
    [TestCase("happy hacking")]
    public void TestStateString(string value)
    {
      var state = ResolveState<StateString, string>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(new byte[] { 0x00 })]
    [TestCase(new byte[] { 0x00, 0x01, 0x02 })]
    public void TestStateBytes(byte[] value)
    {
      var state = ResolveState<StateBytes, byte[]>();
      state.Set(value);
      var result = state.Get();

      Assert.AreEqual(value, result);
    }

    [TestCase(1)]
    public void TestStateHook(int value)
    {
      var getHookData = 0;
      var setHookData = 0;
      var state = ResolveState<StateInt32, int>();

      state.AddSetHook(data => setHookData = data);
      state.Set(value);
      Task.Delay(3).GetAwaiter().GetResult();
      Assert.AreEqual(value, setHookData);

      state.AddGetHook(data => getHookData = data);
      state.Get();
      Task.Delay(3).GetAwaiter().GetResult();
      Assert.AreEqual(value, getHookData);
    }

    [Test]
    public void TestSetStringBool()
    {
      var state = ResolveState<StateBool, bool>();
      state.SetString("true");
      var value = state.Get();
      Assert.AreEqual(value, true);
    }

    [Test]
    public void TestSetStringUShort()
    {
      var state = ResolveState<StateUInt16, ushort>();
      state.SetString("100");
      var value = state.Get();
      Assert.AreEqual(value, 100);
    }

    [Test]
    public void TestSetStringInt32()
    {
      var state = ResolveState<StateInt32, int>();
      state.SetString("10000");
      var value = state.Get();
      Assert.AreEqual(value, 10000);
    }

    [Test]
    public void TestSetStringString()
    {
      var state = ResolveState<StateString, string>();
      state.SetString("hello world");
      var value = state.Get();
      Assert.AreEqual(value, "hello world");
    }

  }

}
