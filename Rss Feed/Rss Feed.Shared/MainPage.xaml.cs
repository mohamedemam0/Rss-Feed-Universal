using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using Windows.Web.Syndication;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Rss_Feed
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private async void FeedBtn_Click(object sender, RoutedEventArgs e)
        {
            FeedData feedItem = new FeedData();
                SyndicationClient client = new SyndicationClient();
                Uri RssUri = new Uri("http://dvd.netflix.com/Top100RSS");
                var feeds = await client.RetrieveFeedAsync(RssUri);
            try
            {

                foreach(SyndicationItem feed in feeds.Items)
                {
                    //FeedData feedItem = new FeedData();

                    feedItem.Title = feed.Title.Text;
                    feedItem.PubDate = feed.PublishedDate.DateTime;
                    //feedItem.Image = feed.Summary.Text.Split('<')[5].Replace("img src='", "").Replace("' border='0' />", "");

                    //feedItem.Image = new Regex(@"<img.*?src=""(.*?)""", RegexOptions.IgnoreCase).ToString();

                    //feedItem.Image = Regex.Match(feed.Summary.Text, @"<img\s+src='(.+)'\s+border='0'\s+/>").Groups[1].Value;
                    listTitles.Items.Add(feedItem);

                  

                }
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message).ShowAsync();
            }
            var notifier = ToastNotificationManager.CreateToastNotifier();

            // Make sure notifications are enabled
            if(notifier.Setting != NotificationSetting.Enabled)
            {
                var dialog = new MessageDialog("Notifications are currently disabled");
                await dialog.ShowAsync();
                return;
            }

            // Get a toast template and insert a text node containing a message
            var template = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            var element = template.GetElementsByTagName("text")[1];
            element.AppendChild(template.CreateTextNode("Today's Quote: " + (feedItem.Title.ToString())));
            var eelement = template.GetElementsByTagName("text")[0];
            eelement.AppendChild(template.CreateTextNode("Quote App"));
            XmlNodeList toastImageAttributes = template.GetElementsByTagName("image");

            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "ms-appx:///Assets/StoreLogo.scale-100.png");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "red graphic");


            // Schedule the toast to appear 2 seconds from now
            var date = DateTimeOffset.Now.AddSeconds(2);
            var stn = new ScheduledToastNotification(template, date);
            notifier.AddToSchedule(stn);
            //try
            //{
            //    SyndicationClient client = new SyndicationClient();
            //    Uri feedUri = new Uri("http://dvd.netflix.com/Top100RSS");
            //    var feed = await client.RetrieveFeedAsync(feedUri);

            //    foreach(SyndicationItem item in feed.Items)
            //    {
            //        string data = string.Empty;
            //        if(feed.SourceFormat == SyndicationFormat.Atom10)
            //        {
            //            data = item.Content.Text;
            //        }
            //        else if(feed.SourceFormat == SyndicationFormat.Rss20)
            //        {
            //            data = item.Summary.Text;
            //        }

            //        Regex regx = new Regex("http://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?.(?:jpg|bmp|gif|png)", RegexOptions.IgnoreCase);
            //        string filePath = regx.Match(data).Value;

            //        FeedData group = new FeedData(item.Id,
            //                                                    item.Title.Text,
            //                                                    filePath.Replace("small", "large"));
            //        listTitles.Items.Add(group);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await new MessageDialog(ex.Message).ShowAsync();
            //}




            //HttpClient rssclient = new HttpClient();
            //var RSScontent = await rssclient.GetStringAsync(new Uri("http://teknoseyir.com/feed"));

            //var RssData = from rss in XElement.Parse(RSScontent).Descendants("item")
            //              select new
            //              {
            //                  Title = rss.Element("title").Value,
            //                  pubDate = rss.Element("pubDate").Value,
            //                  Description = rss.Element("description").Value,
            //                  Link = rss.Element("link").Value,
            //                  image = rss.Element("image").Value
            //              };
            //listTitles.Items.Add(RssData);

        }

        private async void listTitles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SyndicationClient client = new SyndicationClient();
            Uri RssUri = new Uri("http://dvd.netflix.com/Top100RSS");
            var feeds = await client.RetrieveFeedAsync(RssUri);

            webView.Navigate(feeds.Items[listTitles.SelectedIndex].Links[0].Uri);
        }


    }
}
