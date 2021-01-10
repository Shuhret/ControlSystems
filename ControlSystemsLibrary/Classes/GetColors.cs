using System.Windows;
using System.Windows.Media;

namespace ControlSystemsLibrary
{
    class GetColors
    {
        // Метод: Возвращает цвет из ресурса
        public static SolidColorBrush Get(string ColorName)
        {
            return new SolidColorBrush((Color)Application.Current.FindResource(ColorName));
        }
    }
}
