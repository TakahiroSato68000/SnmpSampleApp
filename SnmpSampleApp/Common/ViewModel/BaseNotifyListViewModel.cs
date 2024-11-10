using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Common.ViewModel
{
    public class BaseNotifyListViewModel : BaseViewModel
    {
        private Dictionary<string, object> currentPropertyValues = new Dictionary<string, object>();

        /// <summary>
        /// 現在のプロパティ値を取得
        /// </summary>
        protected TResult GetPropertyValue<TResult>([CallerMemberName]string propertyName = null)
        {
            Debug.Assert(propertyName != null, "propertyName should not be null");

            //キーに値が無かったら初期値を現在値に入力
            if (!currentPropertyValues.ContainsKey(propertyName))
                currentPropertyValues[propertyName] = default(TResult);

            //Dictionaryから現在値を取得してプロパティの型に変換する
            return (TResult)currentPropertyValues[propertyName];
        }

        /// <summary>
        /// 現在のプロパティ値を取得
        /// </summary>
        /// <param name="initialValue">初期値</param>
        /// <remarks>プロパティ名を指定しない場合は、呼び出し元のメソッド名が使用されます。</remarks>
        protected TResult GetProperty<TResult>(TResult initialValue, [CallerMemberName] string propertyName = null)
        {
            Debug.Assert(propertyName != null, "propertyName should not be null");

            //キーに値が無かったら初期値を現在値に入力
            if (!currentPropertyValues.ContainsKey(propertyName))
                currentPropertyValues[propertyName] = initialValue;

            //Dictionaryから現在値を取得してプロパティの型に変換する
            return (TResult)currentPropertyValues[propertyName];
        }

        /// <summary>
        /// 前と値が違うなら変更してイベントを発行する
        /// </summary>
        /// <param name="value">新しい値</param>
        /// <returns>値の変更有無</returns>
        /// <remarks>プロパティ名を指定しない場合は、呼び出し元のメソッド名が使用されます。</remarks>
        protected bool RaisePropertyChangedIfSet<TResult>(TResult value, [CallerMemberName] string propertyName = null)
        {
            Debug.Assert(propertyName != null, "propertyName should not be null");

            //値が同じだったら何もしない
            if (EqualityComparer<TResult>.Default.Equals(GetPropertyValue<TResult>(propertyName), value))
                return false;

            //プロパティの現在値に入力
            currentPropertyValues[propertyName] = value;
            //イベント発行
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
