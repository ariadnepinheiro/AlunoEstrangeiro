// HadesLyc vProducao
// Gerado por:
//   victor.quintana em 20/04/2009 às 12:21h
//   CronosRN, Version=1.1.3385.29867, Culture=neutral, PublicKeyToken=4a25cea0e390af6e
// Último load por leo em 01/12/2008 às 17:29:58 do banco SQLServer (Cronos@Techne17\Usuarios_SQL:HadCronosProducao)

using System;
using System.Reflection;

using Techne.Data;

namespace Techne.HadesLyc {
  public class Config {
    private static string connectionString = null;

    public static string ConnectionString {
      get {
        if(connectionString == null)
          connectionString = ConnectionList.GetConnectionString("Hades");
        return connectionString;
      } 
    }

    public static TConnection CreateConnection(string transacao) {
      return ConnectionList.CreateConnection("Hades", transacao);
    }

    public static TConnection CreateConnection() {
      return ConnectionList.CreateConnection("Hades");
    }

    public static TConnectionWritable CreateWritableConnection(TPermission permission, string transacao) {
      return ConnectionList.CreateWritableConnection(permission, "Hades", transacao);
    }

    public static TConnectionWritable CreateWritableConnection(TPermission permission) {
      return ConnectionList.CreateWritableConnection(permission, "Hades");
    }

    public static TConnectionWritable CreateWritableConnection(string transacao) {
      return ConnectionList.CreateWritableConnection("Hades", transacao);
    }

    public static TConnectionWritable CreateWritableConnection() {
      return ConnectionList.CreateWritableConnection("Hades");
    }

  }

  internal class CRCustomLib : CRCustomLibBase {
    private static bool isCustomAssemblyInited = false;
    private static bool isTCustomAssemblyInited = false;
    private static Assembly customAssembly = null;
    private static Assembly tcustomAssembly = null;

    public static ConstructorInfo GetTCustomConstructor(string cronosDatasetName) {
      return GetTCustomConstructor(cronosDatasetName, "HadesLyc", Assembly.GetExecutingAssembly(), TCustomAssembly);
    }

    public static ConstructorInfo GetCustomConstructor(string cronosDatasetName) {
      return GetCustomConstructor(cronosDatasetName, "HadesLyc", Assembly.GetExecutingAssembly(), CustomAssembly);
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

