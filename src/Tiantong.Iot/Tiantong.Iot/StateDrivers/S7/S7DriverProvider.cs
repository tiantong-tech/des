using System.Net.Sockets;
using Tiantong.Iot.Protocol;

namespace Tiantong.Iot
{
  public abstract class S7DriverProvider : IStateDriverProvider
  {
    protected S7TcpClient _client;

    public S7DriverProvider(string host, int port)
    {
      _client = new S7TcpClient(host, port);
    }

    public IStateDriver Resolve()
    {
      return new StateTcpDriver(
        _client,
        new S7ReadRequest(),
        new S7ReadResponse(),
        new S7WriteRequest(),
        new S7WriteResponse()
      );
    }

    public abstract void Connect();

    public void Close()
    {
      _client.Close();
    }

  }

}
