using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace Practika2_OPAM_Ubohyi_Stanislav.ViewModels
{
    public class ViewModelBase : ReactiveObject, IDisposable
    {
        protected CompositeDisposable Disposables { get; } = new CompositeDisposable();

        public virtual void Dispose()
        {
            Disposables?.Dispose();
        }
    }
}
