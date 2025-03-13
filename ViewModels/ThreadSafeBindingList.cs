using Equin.ApplicationFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamTools.ViewModels
{
    public class ThreadSafeBindingList<T>
    {
        private SynchronizationContext _syncContext;

        private readonly BindingList<T> _bindingList = new BindingList<T>();
        private BindingSource _bindingSource = new BindingSource();
        private readonly BindingListView<T> _bindingListview;

        public ThreadSafeBindingList(SynchronizationContext syncContext)
        {
            _syncContext = syncContext;
            _bindingListview = new BindingListView<T>(_bindingList);
            _bindingSource.DataSource = _bindingListview;
        }
        public BindingSource BindingSource => _bindingSource;

        public T SelectedObject => _bindingSource.Current is ObjectView<T> objSelected ? objSelected.Object : default;

        public IEnumerable<T> GetAll()
        {
            return _bindingList.ToArray();
        }

        public bool Exist(Func<T, bool> compare)
        {
            return _bindingList.FirstOrDefault(compare) != null;
        }

        public T GetOne(Func<T, bool> func)
        {
            if (func == null) return default;
            return _bindingList.FirstOrDefault(func);
        }
        public IEnumerable<T> GetMany(Func<T, bool> func)
        {

            if (func == null) return default;
            return _bindingList.Where(func);
        }
        public IEnumerable<TResult> Select<TResult>(Func<T, TResult> selector)
        {

            if (selector == null) return default;
            return _bindingList.Select(selector);

        }
        public bool Any(Func<T, bool> func)
        {
            if (func == null) return default;
            return _bindingList.Any(func);
        }

        public int Count()
        {
            return _bindingList.Count();
        }
        public int Count(Func<T, bool> compare)
        {
            return _bindingList.Count(compare);
        }


        public void Add(T obj)
        {
            if (_syncContext != null)
            {
                _syncContext.Send(_ => {
                    _bindingList.Add(obj);
                }, null);
            }
            else _bindingList.Add(obj);

        }

        public void Remove(T obj)
        {
            if (_syncContext != null)
            {
                _syncContext.Send(_ => {
                    _bindingList.Remove(obj);
                }, null);
            }
            else _bindingList.Remove(obj);
        }


        public void Edit(Func<T, bool> compare, Action<T> action)
        {
            if (_syncContext != null)
            {
                _syncContext.Send(_ =>
                {
                    var obj = _bindingList.FirstOrDefault(compare);
                    if (obj == null) return;
                    action(obj);
                }, null);
            }
            else
            {
                var obj = _bindingList.FirstOrDefault(compare);
                if (obj == null) return;
                action(obj);
            }

        }
        public void Edit(T currentObj, Action<T> action)
        {
            if (_syncContext != null)
            {
                _syncContext.Send(_ =>
                {
                    var obj = _bindingList.FirstOrDefault(x => x.Equals(currentObj));
                    if (obj == null) return;
                    action(obj);
                }, null);
            }
            else
            {
                var obj = _bindingList.FirstOrDefault(x => x.Equals(currentObj));
                if (obj == null) return;
                action(obj);
            }

        }
        public void Clear()
        {
            if (_syncContext != null)
            {
                _syncContext.Send(_ => {
                    _bindingList.Clear();
                }, null);
            }
            else _bindingList.Clear();


        }
        public void Filter(Predicate<T> includeItem)
        {
            if (_syncContext != null)
            {
                _syncContext.Send(_ => {
                    _bindingListview.ApplyFilter(includeItem);
                }, null);
            }
            else _bindingListview.ApplyFilter(includeItem);

        }

        public void RemoveFilter()
        {
            if (_syncContext != null)
            {
                _syncContext.Send(_ => {
                    _bindingListview.RemoveFilter();
                }, null);
            }
            else _bindingListview.RemoveFilter();


        }
    }
}
