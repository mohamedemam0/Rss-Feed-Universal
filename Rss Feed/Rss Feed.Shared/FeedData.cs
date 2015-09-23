using System;
using System.Collections.Generic;
using System.Text;

namespace Rss_Feed
{
    class FeedData
    {

        //public FeedData(String uniqueId, String title, String imagePath)
        //{
        //    this.UniqueId = uniqueId;
        //    this.Title = title;
        //    this.ImagePath = imagePath;
        //}

        public string UniqueId
        { get; private set; }
        public string Title
        { get;  set; }
        public string Link
        { get; private set; }
        public DateTime PubDate
        { get;  set; }
        public string ImagePath
        { get; private set; }




    }
    }
