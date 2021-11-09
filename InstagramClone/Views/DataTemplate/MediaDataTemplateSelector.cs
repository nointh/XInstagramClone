using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static InstagramClone.Models.PostModel;

namespace InstagramClone.Views.DataTemplate
{
    class MediaDataTemplateSelector : DataTemplateSelector
    {
        public Xamarin.Forms.DataTemplate VideoTemplate { get; set; }
        public Xamarin.Forms.DataTemplate ImageTemplate { get; set; }
        public MediaDataTemplateSelector()
        {
            VideoTemplate = new Xamarin.Forms.DataTemplate(typeof(VideoDataTemplate));
            ImageTemplate = new Xamarin.Forms.DataTemplate(typeof(ImageDataTemplate));
        }
        protected override Xamarin.Forms.DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            Media media = (Media)item;
            return media.Type == "video" ? VideoTemplate : ImageTemplate;
        }
    }
}
