using System;
using System.IO;

public static class CurrentServer
{
    public static string MapPath(string path)
    {
        return Path.Combine(
            (string)AppDomain.CurrentDomain.GetData("ContentRootPath"),
            path);
    }
}