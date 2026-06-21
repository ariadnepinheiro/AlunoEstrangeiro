// Lyceum vDesenvSP
// Gerado por:
//   ercastro em 15/12/2008 às 16:39h
//   CronosRN, Version=1.1.3271.29361, Culture=neutral, PublicKeyToken=4a25cea0e390af6e
// Último load por ercastro em 10/12/2008 às 14:28:46 do banco SQLServer (sa@techne30:LyceumDesenvSP)

using System;
using System.Reflection;

using Techne.Data;

namespace Techne.Lyceum {
  public class Config {
    private static string connectionString = null;

    public static string ConnectionString {
      get {
        if(connectionString == null)
          connectionString = ConnectionList.GetConnectionString("Lyceum");
        return connectionString;
      } 
    }

    public static TConnection CreateConnection(string transacao) {
      return ConnectionList.CreateConnection("Lyceum", transacao);
    }

    public static TConnection CreateConnection() {
      return ConnectionList.CreateConnection("Lyceum");
    }

    public static TConnectionWritable CreateWritableConnection(TPermission permission, string transacao) {
      return ConnectionList.CreateWritableConnection(permission, "Lyceum", transacao);
    }

    public static TConnectionWritable CreateWritableConnection(TPermission permission) {
      return ConnectionList.CreateWritableConnection(permission, "Lyceum");
    }

    public static TConnectionWritable CreateWritableConnection(string transacao)
    {
      return ConnectionList.CreateWritableConnection("Lyceum", transacao);
    }

    public static TConnectionWritable CreateWritableConnection()
    {
      return ConnectionList.CreateWritableConnection("Lyceum");
    }

  }

  internal class CRCustomLib : CRCustomLibBase {
    private static bool isCustomAssemblyInited = false;
    private static bool isTCustomAssemblyInited = false;
    private static Assembly customAssembly = null;
    private static Assembly tcustomAssembly = null;

    public static ConstructorInfo GetTCustomConstructor(string cronosDatasetName) {
      return GetTCustomConstructor(cronosDatasetName, "Lyceum", Assembly.GetExecutingAssembly(), TCustomAssembly);
    }

    public static ConstructorInfo GetCustomConstructor(string cronosDatasetName) {
      return GetCustomConstructor(cronosDatasetName, "Lyceum", Assembly.GetExecutingAssembly(), CustomAssembly);
    }


    private static Assembly TCustomAssembly {
      get {
        if(!isTCustomAssemblyInited) {
          tcustomAssembly = GetTCustomAssembly("Lyceum");
          isTCustomAssemblyInited = true;
        }
        return tcustomAssembly;
      }
    }

    private static Assembly CustomAssembly {
      get {
        if(!isCustomAssemblyInited) {
          customAssembly = GetCustomAssembly("Lyceum");
          isCustomAssemblyInited = true;
        }
        return customAssembly;
      }
    }

  }

}

