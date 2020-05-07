using System;
using System.Text;

namespace Wcs.Plc.Protocol
{
  public class S7ReadResponse: IPlcReadResponse
  {
    private byte[] _message; 

    public byte[] Message
    {
      get => _message;
      set {
        _message = value;
        GetIsDataResponse();
        GetErrorCode();
        GetDataCode();
        GetData();
      }
    }

    public bool IsDataResponse;

    public byte[] ErrorCode;

    public byte DataCode;

    public byte[] Data;

    private int _length;

    public void UseBool()
    {
      _length = 1;
    }

    public void UseUShort()
    {
      _length = 4;
    }

    public void UseString(int length)
    {
      // Snap7: 在字符串前存储字符串的长度
      _length = length + 1;
    }

    public void UseBytes(int length)
    {
      _length = length;
    }

    private void GetIsDataResponse()
    {
      IsDataResponse = Message[8] == 0x03;
    }

    private void GetErrorCode()
    {
      ErrorCode = new [] { Message[17], Message[18] };
    }

    private void GetDataCode()
    {
      DataCode = Message[21];
    }

    private void GetData()
    {
      var length = BitConverter.ToInt32(new byte[] { Message[16], Message[15], 0, 0 }) - 4;

      if (length <= 0) {
        return;
      }

      var data = new byte[length];
      Array.Copy(Message, 25, data, 0, length);

      Data = data;
    }

    //

    public ushort GetUInt16()
    {
      return 0;
    }

    public int GetInt32()
    {
      if (Data.Length != _length) {
        throw new Exception($"byte array length is not {_length}");
      }
      var data = new byte[Data.Length];

      Array.Copy(Data, data, data.Length);
      Array.Reverse(data);

      return BitConverter.ToInt32(data);
    }

    public bool GetBool()
    {
      if (Data.Length != _length) {
        throw new Exception($"byte array length is not {_length}");
      }

      return BitConverter.ToBoolean(Data);
    }

    public string GetString()
    {
      return Encoding.ASCII.GetString(Data, 1, Data.Length - 1);
    }

    public byte[] GetBytes()
    {
      return Data;
    }

  }
}
