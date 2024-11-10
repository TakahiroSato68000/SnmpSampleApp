using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Common.ViewModel
{
    public class PropertyNotify : INotifyPropertyChanged
    {
        // メインスレッドのSynchronizationContextを保持するためのフィールド
        private static readonly Lazy<SynchronizationContext> synchronizationContext = new(() => SynchronizationContext.Current);

        // プロパティ変更イベント
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// メインスレッドの SynchronizationContext を初期化します。
        /// </summary>
        /// <remarks>メインスレッドからこの関数を呼び出してください。</remarks>
        public static void InitializeSynchronizationContext()
        {
            var _ = synchronizationContext.Value;
        }

        /// <summary>
        /// 指定された値をソースに設定し、値が変更されたかどうかを返します。
        /// </summary>
        /// <typeparam name="T">値の型</typeparam>
        /// <param name="source">元の値</param>
        /// <param name="value">新しい値</param>
        /// <returns>値が変更された場合は true、それ以外の場合は false</returns>
        protected bool SetValue<T>(ref T source, T value)
        {
            if (EqualityComparer<T>.Default.Equals(source, value))
            {
                return false;
            }
            source = value;
            return true;
        }

        /// <summary>
        /// 指定された値をソースに設定し、値が変更された場合は PropertyChanged イベントを発行します。
        /// </summary>
        /// <typeparam name="T">値の型</typeparam>
        /// <param name="source">元の値</param>
        /// <param name="value">新しい値</param>
        /// <param name="propertyName">プロパティ名</param>
        /// <returns>値が変更された場合は true、それ以外の場合は false</returns>
        /// <remarks>プロパティ名を指定しない場合は、呼び出し元のメソッド名が使用されます。</remarks>
        protected bool RaiseIfSet<T>(ref T source, T value, [CallerMemberName] string propertyName = null)
        {
            Debug.Assert(propertyName != null, "propertyName should not be null");

            if (SetValue(ref source, value))
            {
                RaisePropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 指定された値をソースに設定し、値が変更された場合は PropertyChanged イベントを発行します。
        /// </summary>
        /// <typeparam name="T">値の型</typeparam>
        /// <param name="source">元の値</param>
        /// <param name="value">新しい値</param>
        /// <param name="arg">PropertyChangedEventArgs インスタンス</param>
        /// <returns>値が変更された場合は true、それ以外の場合は false</returns>
        protected bool RaiseIfSet<T>(ref T source, T value, PropertyChangedEventArgs arg)
        {
            if (SetValue(ref source, value))
            {
                RaisePropertyChanged(arg);
                return true;
            }
            return false;
        }

        /// <summary>
        /// PropertyChanged イベントを発行します。
        /// </summary>
        /// <param name="propertyName">プロパティ名</param>
        /// <remarks>プロパティ名を指定しない場合は、呼び出し元のメソッド名が使用されます。</remarks>
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.Assert(propertyName != null, "propertyName should not be null");
            RaisePropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// PropertyChanged イベントを発行します。
        /// </summary>
        /// <param name="args">PropertyChangedEventArgs インスタンス</param>
        protected void RaisePropertyChanged(PropertyChangedEventArgs args)
        {
            Debug.Assert(synchronizationContext.Value != null);
            synchronizationContext.Value.Post(e =>
            {
                // イベント発行
                PropertyChanged?.Invoke(this, (PropertyChangedEventArgs)e);
            }, args);
        }
    }
}