using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MichaelFramework.Model
{

    public class UndoableObjectListBase<T> : UndoableBase,IList,IList<T> where T : UndoableObjectBase
    {
        private enum eItemState
        {
            New = 0,
            Added,
            Removed,
            Modified
        }

        private class ItemData
        {
            public int EditLevel { get; set; }
            public int Index { get; set; }
            public eItemState State { get; set; }

            public ItemData()
            {

            }

            public ItemData(int editLevel, int index,eItemState state)
            {
                EditLevel = editLevel;
                Index = index;
                State = state;
            }
        }

        private class UndoRedoStack
        {
            private Stack<ItemData> undoStack = null;
            public Stack<ItemData> UndoStack
            {
                get
                {
                    if (undoStack == null) undoStack = new Stack<ItemData>();
                    return undoStack;
                }
            }

            private Stack<ItemData> redoStack = null;
            public Stack<ItemData> RedoStack
            {
                get
                {
                    if (redoStack == null) redoStack = new Stack<ItemData>();
                    return redoStack;
                }
            }
        }

        private int editLevel = 0;
        private List<T> innerList = new List<T>();
        private List<T> deletedList = new List<T>();

        private bool undoredoing = false;

        private Dictionary<T, UndoRedoStack> itemStack = new Dictionary<T, UndoRedoStack>();

        public event EventHandler<ItemUpdatedEventArgs<T>> ItemAdding;
        public event EventHandler<ItemUpdatedEventArgs<T>> ItemAdded;
        public event EventHandler<ItemUpdatedEventArgs<T>> ItemRemoving;
        public event EventHandler<ItemUpdatedEventArgs<T>> ItemRemoved;
        public event EventHandler<ItemUpdatedEventArgs<T>> ItemModifing;
        public event EventHandler<ItemUpdatedEventArgs<T>> ItemModified;

        protected virtual void OnItemAdding(ItemUpdatedEventArgs<T> e)
        {
            if (ItemAdding != null)
                ItemAdding(this, e);
        }

        protected virtual void OnItemAdded(ItemUpdatedEventArgs<T> e)
        {
            if (ItemAdded != null)
                ItemAdded(this, e);
        }

        protected virtual void OnItemRemoving(ItemUpdatedEventArgs<T> e)
        {
            if (ItemRemoving != null)
                ItemRemoving(this, e);
        }

        protected virtual void OnItemRemoved(ItemUpdatedEventArgs<T> e)
        {
            if (ItemRemoved != null)
                ItemRemoved(this, e);
        }

        protected virtual void OnItemModifing(ItemUpdatedEventArgs<T> e)
        {
            if (ItemModifing != null)
                ItemModifing(this, e);
        }

        protected virtual void OnItemModified(ItemUpdatedEventArgs<T> e)
        {
            if (ItemModified != null)
                ItemModified(this, e);
        }

        # region 初始化

        public UndoableObjectListBase()
        {

        }

        # endregion

        # region 操作

        private bool editStarted = false;
        public void BeginEdit()
        {
            editStarted = true;
            OnPropertyChanging(new PropertyChangingEventArgs() { PropertyName="",PropertyValue=null});
            ClearRedoState();
            this.editLevel++;
        }

        public void EndEdit()
        {
            foreach (KeyValuePair<T, UndoRedoStack> k in itemStack)
            {
                if (k.Value.UndoStack.Count > 0 && k.Value.UndoStack.Peek().EditLevel >= this.editLevel)
                {
                    OnPropertyChanged(new PropertyChangedEventArgs() { PropertyName="",PropertyValue=null});
                    editStarted = false;
                    return;
                }
            }
            this.editLevel--;
            editStarted = false;
        }

        public void CancelEdit()
        {
            if (this.editLevel > 0)
            {
                this.editLevel--;
                ClearRedoState();
            }
        }

        public void Undo()
        {
            if (editLevel <= 0) return;

            undoredoing = true;

            List<T> opList = new List<T>();

            for (int i = innerList.Count - 1; i >= 0; i--)
            {
                T item = innerList[i];

                Stack<ItemData> undoStack = itemStack[item].UndoStack;
                Stack<ItemData> redoStack = itemStack[item].RedoStack;
                if (undoStack.Count == 0) continue;

                ItemData data = undoStack.Peek();
                if (data.EditLevel == this.editLevel)
                {
                    opList.Add(item);

                    if (data.State == eItemState.Added)
                    {
                        remove(item);
                    }
                    else if (data.State == UndoableObjectListBase<T>.eItemState.Modified)
                    {
                        item.Undo();
                    }

                    redoStack.Push(undoStack.Pop());
                }
            }
            for (int i = deletedList.Count - 1; i >= 0; i--)
            {
                T item = deletedList[i];
                if (opList.Contains(item)) continue;

                Stack<ItemData> undoStack = itemStack[item].UndoStack;
                Stack<ItemData> redoStack = itemStack[item].RedoStack;
                if (undoStack.Count == 0) continue;

                ItemData data = undoStack.Peek();
                if (data.EditLevel == this.editLevel)
                {
                    insert(data.Index, item);

                    redoStack.Push(undoStack.Pop());
                }
            }
            editLevel--;

            undoredoing = false;
        }

        public void Redo()
        {
            undoredoing = true;

            List<T> opList = new List<T>();

            int n = 0;
            for (int i = innerList.Count - 1; i >= 0; i--)
            {
                T item = innerList[i];

                Stack<ItemData> undoStack = itemStack[item].UndoStack;
                Stack<ItemData> redoStack = itemStack[item].RedoStack;
                if (redoStack.Count == 0) continue;

                ItemData data = redoStack.Peek();
                if (data.EditLevel == this.editLevel + 1)
                {
                    opList.Add(item);

                    n++;
                    if (data.State == eItemState.Added)
                    {
                        insert(data.Index, item);
                    }
                    else if (data.State == UndoableObjectListBase<T>.eItemState.Modified)
                    {
                        item.Redo();
                    }
                    else if (data.State == eItemState.New || data.State == eItemState.Removed)
                    {
                        remove(item);
                    }
                    undoStack.Push(redoStack.Pop());
                }
            }
            for (int i = deletedList.Count - 1; i >= 0; i--)
            {
                T item = deletedList[i];

                if (opList.Contains(item)) continue;
                Stack<ItemData> undoStack = itemStack[item].UndoStack;
                Stack<ItemData> redoStack = itemStack[item].RedoStack;
                if (redoStack.Count == 0) continue;

                ItemData data = redoStack.Peek();
                if (data.EditLevel == this.editLevel + 1)
                {
                    n++;

                    insert(data.Index, item);

                    undoStack.Push(redoStack.Pop());
                }
            }
            if (n > 0)
                this.editLevel++;

            undoredoing = false;
        }

        private void ClearRedoState()
        {
            foreach (T item1 in innerList)
            {
                if (itemStack[item1].RedoStack.Count == 0) continue;
                if (itemStack[item1].RedoStack.Peek().EditLevel == this.editLevel + 1)
                    itemStack[item1].RedoStack.Clear();
            }
            for (int i = deletedList.Count - 1; i >= 0; i--)
            {
                T item2 = deletedList[i];
                if (itemStack[item2].RedoStack.Count == 0) continue;
                if (itemStack[item2].RedoStack.Peek().EditLevel == this.editLevel + 1)
                {
                    deletedList.Remove(item2);
                    itemStack.Remove(item2);
                }
            }
        }

        private void insert(int Index, T item)
        {
            ItemUpdatedEventArgs<T> e = new ItemUpdatedEventArgs<T>() { Item=item};

            OnItemAdding(e);
            if (e.Cancel) return;

            if (deletedList.Contains(item))
                deletedList.Remove(item);
            innerList.Insert(Index, item);
            item.PropertyChanging += new EventHandler<PropertyChangingEventArgs>(item_PropertyChanging);
            item.PropertyChanged += new EventHandler<PropertyChangedEventArgs>(item_PropertyChanged);

            OnItemAdded(e);
        }

        private void remove(T item)
        {
            ItemUpdatedEventArgs<T> e = new ItemUpdatedEventArgs<T>() { Item = item };

            OnItemRemoving(e);
            if (e.Cancel) return;

            innerList.Remove(item);
            deletedList.Add(item);
            item.PropertyChanging -= new EventHandler<PropertyChangingEventArgs>(item_PropertyChanging);
            item.PropertyChanged -= new EventHandler<PropertyChangedEventArgs>(item_PropertyChanged);

            OnItemRemoved(e);
        }

        private void appendState(T item, ItemData data)
        {
            if (!itemStack.ContainsKey(item))
                itemStack.Add(item, new UndoableObjectListBase<T>.UndoRedoStack());
            itemStack[item].UndoStack.Push(data);
            itemStack[item].RedoStack.Clear();
        }

        # endregion

        #region IList<T> 成员

        public int IndexOf(T item)
        {
            return innerList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (!innerList.Contains(item))
            {
                insert(index, item);
                ItemData data = new UndoableObjectListBase<T>.ItemData(this.editLevel,
                    innerList.IndexOf(item), eItemState.Added);
                appendState(item, data);
            }
        }

        public void RemoveAt(int index)
        {
            Remove(innerList[index]);
        }

        public T this[int index]
        {
            get
            {
                return innerList[index];
            }
            set
            {
                if (index < 0 || index > innerList.Count - 1 ||
                    innerList[index] == value) return;
                ItemData data = new UndoableObjectListBase<T>.ItemData(this.editLevel,
                   index, eItemState.Removed);
                appendState(innerList[index], data);
                remove(innerList[index]);
                

                insert(index, value);
                data = new UndoableObjectListBase<T>.ItemData(this.editLevel,
                    index, eItemState.Added);
                appendState(value, data);
            }
        }

        #endregion

        #region ICollection<T> 成员

        public void Add(T item)
        {
            bool temp = editStarted;
            if (!temp)
                BeginEdit();

            Insert(this.Count, item);

            if (!temp)
                EndEdit();
        }

        public void AddRange(IEnumerable<T> collection)
        {
            bool temp = editStarted;
            if (!temp)
                BeginEdit();

            int count = innerList.Intersect(collection).Count();
            if (count > 0 && count==Math.Min(collection.Count(),innerList.Count))
                return;

            foreach (T item in collection)
            {
                if (!innerList.Contains(item))
                {
                    insert(this.Count, item);
                    ItemData data = new UndoableObjectListBase<T>.ItemData(this.editLevel,
                    innerList.IndexOf(item), eItemState.Added);
                    appendState(item, data);
                }
            }

            if (!temp)
                EndEdit();
        }

        void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!undoredoing)
            {
                ItemData data = new UndoableObjectListBase<T>.ItemData(this.editLevel,
                    this.IndexOf(sender as T), UndoableObjectListBase<T>.eItemState.Modified);
                if (!itemStack.ContainsKey(sender as T))
                    itemStack.Add(sender as T, new UndoableObjectListBase<T>.UndoRedoStack());
                itemStack[sender as T].UndoStack.Push(data);
            }
            OnItemModified(new ItemUpdatedEventArgs<T>() { Item = sender as T });
        }

        void item_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            ItemUpdatedEventArgs<T> ex = new ItemUpdatedEventArgs<T>() { Item = sender as T };
            OnItemModifing(ex);
            e.Cancel = ex.Cancel;
        }

        public void Clear()
        {
            RemoveRange(innerList.ToArray());
        }

        public bool Contains(T item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        new public int Count
        {
            get { return innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            bool temp = editStarted;
            if (!temp)
                BeginEdit();

            if (innerList.Contains(item))
            {
                ItemData data = new UndoableObjectListBase<T>.ItemData(this.editLevel,
                   innerList.IndexOf(item), eItemState.Removed);
                remove(item);
                appendState(item, data);

                if (!temp) EndEdit();

                return true;
            }
            if (!temp) EndEdit();
            return false;
        }

        public bool RemoveRange(IEnumerable<T> collection)
        {
            if (innerList.Intersect(collection).Count() == 0) return false;

            bool temp = editStarted;
            if (!temp)
                BeginEdit();

            foreach (T item in collection)
            {
                if (innerList.Contains(item))
                {
                    ItemData data = new UndoableObjectListBase<T>.ItemData(this.editLevel,
                        innerList.IndexOf(item), UndoableObjectListBase<T>.eItemState.Removed);
                    remove(item);
                    appendState(item, data);
                }
            }

            if (!temp) EndEdit();

            return true;
        }

        #endregion

        #region IEnumerable<T> 成员

        public IEnumerator<T> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
        
        #region IList 成员

        int IList.Add(object value)
        {
            Add(value as T);
            return IndexOf(value as T);
        }

        void IList.Clear()
        {
            Clear();
        }

        bool IList.Contains(object value)
        {
            return Contains(value as T);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf(value as T);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, value as T);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get {return false; }
        }

        void IList.Remove(object value)
        {
            Remove(value as T);
        }

        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value as T;
            }
        }

        #endregion

        #region ICollection 成员

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { throw new NotImplementedException(); }
        }

        object ICollection.SyncRoot
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }

    public class ItemUpdatedEventArgs<T> : EventArgs where T:UndoableObjectBase
    {
        public T Item { get;set; }

        public bool Cancel { get; set; }
    }
}