﻿using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Windows.Interop;
using System.Windows.Threading;

namespace ClangPowerTools
{
  public class ErrorsManager
  {
    #region Members

    private static ErrorWindow mErrorWindow = new ErrorWindow();
    private Dispatcher mDispatcher;

    #endregion

    #region Constructor

    public ErrorsManager(IServiceProvider aServiceProvider, DTE2 aDte)
    {
      mErrorWindow.Initialize(aServiceProvider);
      mDispatcher = HwndSource.FromHwnd((IntPtr)aDte.MainWindow.HWnd).RootVisual.Dispatcher;
    }
    #endregion

    #region Public Methods

    public void AddError(TaskError aError)
    {
      if (!String.IsNullOrWhiteSpace(aError.Message))
        mErrorWindow.AddError(aError);
    }

    public void AddErrors(IEnumerable<TaskError> aErrors)
    {
      mDispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
      {
        mErrorWindow.Clear();
        foreach (TaskError error in aErrors)
          mErrorWindow.AddError(error);
        mErrorWindow.Show();
      }));

    }

    public void Clear() => mErrorWindow.Clear();

    #endregion
  }
}
