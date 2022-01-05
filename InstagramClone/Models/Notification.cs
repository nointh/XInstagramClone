using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace InstagramClone.Models
{
    public class NotificationModel
    {
        public string Type { get; set; }
        public string Image { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string PostId { get; set; }
        public string PostCaption { get; set; }
        public string CommentId { get; set; }
        public string CommentContent { get; set; }
        public string Time { get; set; }
    }
    public class TimeDifferenceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime postTime;
            if (DateTime.TryParseExact((string)value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out postTime))
            {
                TimeSpan difference = DateTime.Now.Subtract(postTime);
                if (difference.TotalSeconds < 60)
                    return ((int)difference.TotalSeconds).ToString() + " second" + (difference.TotalDays == 1 ? "" : "s") + " ago";
                if (difference.TotalMinutes < 60)
                    return ((int)difference.TotalMinutes).ToString() + " minute" + (difference.TotalDays == 1 ? "" : "s") + " ago";
                if (difference.TotalHours < 60)
                    return ((int)difference.TotalHours).ToString() + " hour" + (difference.TotalDays == 1 ? "" : "s") + " ago";
                return ((int)difference.TotalDays).ToString() + " day" + (difference.TotalDays == 1 ? "" : "s") + " ago";

            }
            else return "";
            //var postTime = DateTime.ParseExact((string)value, "dd/MM/yyyy  h:mm:ss tt", CultureInfo.InvariantCulture);

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class QuoteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string content = (string)value;
            return " \" " + content + " \" ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
