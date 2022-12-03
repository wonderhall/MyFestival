using System;

[Serializable]
public class ReqSignup
{
    public string username;
    public string email;
    public string hashedPassword;
    public string[] userRoles;
}