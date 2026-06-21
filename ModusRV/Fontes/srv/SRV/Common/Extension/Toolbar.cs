using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace SRV.Common.Extension
{
    /// <summary>
    /// Representa a Toolbar de um projeto
    /// </summary>
    public class Toolbar : IDisposable
    {
    private bool _disposed;
    private readonly ViewContext _viewContext;
    private readonly TextWriter _writer;

    /// <summary>
    /// Inicializa uma nova instancia da classe <see cref="Toolbar"/>.
    /// </summary>
    /// <param name="viewContext">view context.</param>
    public Toolbar(ViewContext viewContext)
    {
        if (viewContext == null) {
            throw new ArgumentNullException("viewContext");
        }
        _viewContext = viewContext;
        _writer = viewContext.Writer;
    }

    /// <summary>
    /// Performs application-defined tasks associated with 
    /// freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true /* disposing */);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both 
    /// managed and unmanaged resources; <c>false</c> 
    /// to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            _disposed = true;
            _writer.Write("</div></div>");
        }
    }

    /// <summary>
    /// Ends the div.
    /// </summary>
    public void EndToolbar()
    {
        Dispose(true);
    }

    }
}