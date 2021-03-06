using System;
using System.Text;

namespace Tiantong.Iot.Protocol
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
        GetDataLength();
        GetData();
      }
    }

    public bool IsDataResponse;

    public byte[] ErrorCode;

    public int DataLength;

    public byte DataCode;

    public byte[] Data;

    private int _length;

    public void UseBool()
    {
      _length = 1;
      Data = new byte[1];
    }

    public void UseUInt16()
    {
      _length = 2;
      Data = new byte[2];
    }

    public void UseInt32()
    {
      _length = 4;
      Data = new byte[4];
    }

    public void UseString(int length)
    {
      _length = length + 1;
      Data = new byte[_length];
    }

    public void UseBytes(int length)
    {
      throw KnownException.Error("暂时不支持 Bytes 类型");
    }

    private void GetIsDataResponse()
    {
      IsDataResponse = Message[8] == 0x03;
    }

    private void GetErrorCode()
    {
      ErrorCode = new [] { Message[17], Message[18] };
      if (ErrorCode[0] != 0x00 || ErrorCode[1] != 0x00) {
        var errorCode = BitConverter.ToString(ErrorCode);

        throw KnownException.Error($"错误类型、错误代码: {errorCode}");
      }
    }

    private void GetDataCode()
    {
      DataCode = Message[21];
    }

    private void GetDataLength()
    {
      DataLength = BitConverter.ToUInt16(new byte[] { Message[16], Message[15] }) - 4;
    }

    private void GetData()
    {
      Array.Copy(Message, 25, Data, 0, _length);
      if (DataLength != _length) {
        var byteString = BitConverter.ToString(Data);
        throw KnownException.Error($"数据校验失败，长度应为: {_length}，实际长度: {DataLength}, 二进制数据: {byteString}");
      }
    }

    //

    public byte[] GetBytes(bool reverse = true)
    {
      if (reverse) {
        Array.Reverse(Data);
      }

      return Data;
    }

    public bool GetBool()
    {
      return BitConverter.ToBoolean(GetBytes());
    }

    public ushort GetUInt16()
    {
      return BitConverter.ToUInt16(GetBytes());
    }

    public int GetInt32()
    {
      return BitConverter.ToInt32(GetBytes());
    }

    public string GetString()
    {
      return Encoding.ASCII.GetString(GetBytes(false)[1..]);
    }

    public byte[] GetBytes()
    {
      return GetBytes(true);
    }
  }
}