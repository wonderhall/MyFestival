using System;

[Serializable]
public class ServerException
{
    public int status;
    public long timestamp;
    public string code;
    public string error;
}