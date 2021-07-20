using FreshMvvm;
using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace InstagramClone.PageModels
{
    public class MainPageModel:FreshBasePageModel
    {
        public ObservableCollection<Story> Stories { get; set; }
        public MainPageModel()
        {
            Stories = new ObservableCollection<Story>(Story.GetAllStories());
        }
    }
}
