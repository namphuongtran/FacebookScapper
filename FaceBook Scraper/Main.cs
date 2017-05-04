using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using Facebook;
using System.Reflection;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Net;
using OfficeOpenXml;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput;
using System.Configuration;
using System.Text.RegularExpressions;

namespace FaceBook_Scraper
{
    public partial class Main : Form
    {
        ChromiumWebBrowser webBrw;
        string accessToken;
        string page;
        bool next = false;
        bool stop = true;
        bool started = false;
        bool hasInternetConnection = false;
        int postsLimit = 50;
        int commentsLimit = 100;
        int likesLimit = 1000;
        FacebookClient facebookClient;
        static string folderStored = ConfigurationManager.AppSettings["FolderStored"];
        static string currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static string dataPath = Path.Combine(currentPath, folderStored);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckInternet()
        {
            while (!hasInternetConnection)
            {
                try
                {
                    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://www.google.com");
                    HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                    if (res.StatusCode == HttpStatusCode.OK)
                        hasInternetConnection = true;
                    res.Close();
                }
                catch (Exception ex)
                {
                    hasInternetConnection = false;
                    Logging.WriteLog(ex.Message);
                }
            }
            return hasInternetConnection;
        }

        /// <summary>
        /// 
        /// </summary>
        public Main()
        {
            InitializeComponent();
            //Initialize something
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }
            CefSettings settings = new CefSettings();
            settings.WindowlessRenderingEnabled = true;
            Cef.Initialize(settings);
            Cef.EnableHighDPISupport();
            CheckForIllegalCrossThreadCalls = false;
            //Check Internet connection
            if (CheckInternet())
            {
                string appId = ConfigurationManager.AppSettings["AppId"];
                if (string.IsNullOrEmpty(appId))
                {
                    appId = "395739090795216";
                }
                dynamic parameters = new ExpandoObject();
                parameters.client_id = appId;
                string redirectUri = ConfigurationManager.AppSettings["RedirectUri"];
                if (string.IsNullOrEmpty(redirectUri))
                {
                    redirectUri = "https://www.facebook.com/";
                }
                parameters.redirect_uri = redirectUri;
                parameters.response_type = "token";
                parameters.display = "popup";
                string appSecret = ConfigurationManager.AppSettings["AppSecret"];
                if (string.IsNullOrEmpty(appSecret))
                {
                    appSecret = "ae252004cebce3d04ef5beb78979dfa2";
                }
                parameters.client_secret = appSecret;
                facebookClient = new FacebookClient();
                Uri loginUrl = facebookClient.GetLoginUrl(parameters);
                this.FormClosing += new FormClosingEventHandler(Exit);
                if (!string.IsNullOrEmpty(loginUrl.AbsoluteUri))
                {
                    webBrw = new ChromiumWebBrowser(loginUrl.AbsoluteUri);
                    webBrw.LoadingStateChanged += webBrw_LoadingStateChanged;
                    webBrw.Dock = DockStyle.Fill;
                    Panel.Controls.Add(webBrw);
                }
            }
            else
            {
                string message = "You are offline";
                MessageBox.Show(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        void Exit(object sender, FormClosingEventArgs a)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        /// <summary>
        /// 
        /// </summary>
        void StartStopRecording()
        {
            InputSimulator.SimulateKeyDown(VirtualKeyCode.LWIN);
            InputSimulator.SimulateKeyPress(VirtualKeyCode.VK_G);
            InputSimulator.SimulateKeyUp(VirtualKeyCode.LWIN);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webBrw_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            string url = null;
            if (!e.IsLoading)
            {
                url = webBrw.Address;
            }
            if (!string.IsNullOrEmpty(url))
            {
                if (url.IndexOf("login.php") != -1)
                {
                    string email = ConfigurationManager.AppSettings["Email"];
                    if (string.IsNullOrEmpty(email))
                    {
                        email = "bonii11a@hotmail.com";
                    }
                    string password = ConfigurationManager.AppSettings["Password"];
                    if (string.IsNullOrEmpty(password))
                    {
                        password = "l45134@mvrht.com";
                    }
                    webBrw.EvaluateScriptAsync("document.getElementById('email').value='" + email + "'");
                    webBrw.EvaluateScriptAsync("document.getElementById('pass').value='" + password + "'");
                    webBrw.EvaluateScriptAsync("document.getElementById('login_form').submit();");
                }
                if (url.IndexOf("access_token") != -1)
                {
                    accessToken = url.Substring(url.IndexOf("access_token=") + "access_token=".Length);
                    Thread.Sleep(3000);
                    btnStart.Enabled = true;
                }
                if (url.IndexOf("/story.php") != -1)
                {
                    next = true;
                }
                if (url.IndexOf("reaction/profile/browse") != -1)
                {
                    next = true;
                }
            }
            else { next = true; }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ScrollDown()
        {
            IBrowserHost host = webBrw.GetBrowser().GetHost();
            while (true)
            {
                host.SendKeyEvent(0x0100, 0x28, 0);
                Thread.Sleep(250);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        private dynamic GetPosts(int offset = 0)
        {
            dynamic parameters = new ExpandoObject();
            parameters.limit = 25;
            parameters.offset = offset;
            dynamic posts = facebookClient.Get(page + "/posts", parameters);
            return posts;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private dynamic GetLikes(dynamic post, int offset)
        {
            dynamic parameters = new ExpandoObject();
            parameters.limit = 25;
            parameters.offset = offset;
            dynamic likes = facebookClient.Get(post.id + "/likes", parameters);
            return likes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private dynamic GetShares(dynamic post, int offset)
        {
            dynamic parameters = new ExpandoObject();
            parameters.limit = 25;
            parameters.offset = offset;
            dynamic shares = facebookClient.Get(post.id + "/sharedposts", parameters);
            return shares;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private dynamic GetComments(dynamic post, int offset)
        {
            dynamic parameters = new ExpandoObject();
            parameters.limit = 25;
            parameters.offset = offset;
            dynamic comments = facebookClient.Get(post.id + "/comments", parameters);
            return comments;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private string GetPostTime(dynamic post)
        {
            DateTime dateTime = DateTime.Now;
            if (post != null)
            {
                dateTime = DateTime.Parse(post.created_time);
            }
            string year = dateTime.Year.ToString();
            string month = (dateTime.Month.ToString().Length == 2) ? dateTime.Month.ToString() : "0" + dateTime.Month.ToString();
            string day = (dateTime.Day.ToString().Length == 2) ? dateTime.Day.ToString() : "0" + dateTime.Day.ToString();
            string hour = (dateTime.Hour.ToString().Length == 2) ? dateTime.Hour.ToString() : "0" + dateTime.Hour.ToString();
            string minute = (dateTime.Minute.ToString().Length == 2) ? dateTime.Minute.ToString() : "0" + dateTime.Minute.ToString();
            string second = (dateTime.Second.ToString().Length == 2) ? dateTime.Second.ToString() : "0" + dateTime.Second.ToString();
            return dateTime.ToString(year + month + day + hour + minute + second);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        private string GetPostPrev(dynamic post)
        {
            string newString = string.Empty;
            if (post != null)
            {
                string text = ((post.message == null) ? "" : post.message) + ((post.story == null) ? "" : post.story);
                if (!string.IsNullOrEmpty(text) && text.Length > 20)
                {
                    text = text.Substring(0, 20);
                }
                string oldString = text.Replace("/", "").Replace(@"\", "")
                    .Replace(":", "")
                    .Replace(Convert.ToChar(34).ToString(), "")
                    .Replace("*", "")
                    .Replace("?", "")
                    .Replace("<", "")
                    .Replace(">", "")
                    .Replace("|", "")
                    .Replace("#", "")
                    .Replace("~", "")
                    .Replace("`", "")
                    .Replace("%", "")
                    .Trim();
                newString = string.Join(" ", Regex.Split(oldString, @"(?:\r\n|\n|\r)"));

            }
            return newString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void TakeScreenShoot(string path)
        {
            webBrw.DrawToImage().Save(path, ImageFormat.Png);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="destinationFileName"></param>
        /// <param name="timeoutInSeconds"></param>
        /// <returns></returns>
        public static bool SaveFileFromURL(string url, string destinationFileName, int timeoutInSeconds)
        {
            // Create a web request to the URL
            HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
            MyRequest.UserAgent = "Mozilla/5.0 (Mobile; rv:26.0) Gecko/26.0 Firefox/26.0";
            MyRequest.Timeout = timeoutInSeconds * 1000;
            try
            {
                // Get the web response
                HttpWebResponse MyResponse = (HttpWebResponse)MyRequest.GetResponse();

                // Make sure the response is valid
                if (HttpStatusCode.OK == MyResponse.StatusCode)
                {
                    // Open the response stream
                    using (Stream MyResponseStream = MyResponse.GetResponseStream())
                    {
                        // Open the destination file
                        using (FileStream MyFileStream = new FileStream(destinationFileName, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            // Create a 4K buffer to chunk the file
                            byte[] MyBuffer = new byte[4096];
                            int BytesRead;
                            // Read the chunk of the web response into the buffer
                            while (0 < (BytesRead = MyResponseStream.Read(MyBuffer, 0, MyBuffer.Length))) { MyFileStream.Write(MyBuffer, 0, BytesRead); }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                Logging.WriteLog("METHOD-SaveFileFromURL: Error saving file from URL:" + err.Message);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extractionFolder"></param>
        /// <param name="pre"></param>
        private void ScrollPage(string extractionFolder, string pre)
        {
            int screenId = 1;
            TakeScreenShoot(Path.Combine(extractionFolder, pre + screenId + ".png"));
            screenId++;
            int initial_height = webBrw.Height;
            int speed = int.Parse(ScrollSpeedTB.Text);
            int current_pos = 0;
            int scrolled = 0;
            while (true)
            {
                Task<JavascriptResponse> scrollTask = webBrw.EvaluateScriptAsync("window.scrollBy(0, " + speed + ")");
                scrollTask.Wait();
                Task<JavascriptResponse> getcurrentposTask = webBrw.EvaluateScriptAsync("document.documentElement.scrollTop || document.body.scrollTop");
                getcurrentposTask.Wait();

                scrolled += speed;
                if (scrolled >= initial_height)
                {
                    scrolled = 0;
                    TakeScreenShoot(Path.Combine(extractionFolder, pre + screenId + ".png"));
                    screenId++;
                }
                var result = getcurrentposTask.Result.Result;
                if (result != null)
                {
                    int current = int.Parse(result.ToString());
                    if (current <= current_pos)
                    {
                        break;
                    }
                    current_pos = current;
                    Thread.Sleep(5);
                }
            }

            TakeScreenShoot(Path.Combine(extractionFolder, pre + screenId + ".png"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public bool IsImage(string file)
        {
            try
            {
                Image img;
                using (var bmpTemp = new Bitmap(file))
                {
                    img = new Bitmap(bmpTemp);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static Bitmap CombineBitmap(string[] files)
        {
            //read all images into memory
            List<Bitmap> images = new List<Bitmap>();
            Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (string image in files)
                {
                    //create a Bitmap from the file and add it to the list
                    Bitmap bitmap = new Bitmap(image);

                    //update the size of the final bitmap
                    width += bitmap.Width;
                    height = bitmap.Height > height ? bitmap.Height : height;
                    images.Add(bitmap);
                }

                //create a bitmap to hold the combined image
                finalImage = new Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (Graphics g = Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(Color.Black);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (Bitmap image in images)
                    {
                        g.DrawImage(image,
                          new Rectangle(offset, 0, image.Width, image.Height));
                        offset += image.Width;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();
                Logging.WriteLog("METHOD-CombineBitmap: " + ex.Message);
                return null;
            }
            finally
            {
                //clean up memory
                foreach (Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <returns></returns>
        private string GetFtEntIdentifier()
        {
            string id = null;
            try
            {
                Task<string> html_task = webBrw.GetSourceAsync();
                html_task.Wait();
                string html = html_task.Result.ToString();
                int pos1 = html.IndexOf("ft_ent_identifier");
                pos1 = html.IndexOf("=", pos1) + 1;
                int pos2 = html.IndexOf(Convert.ToChar(34), pos1);
                id = html.Substring(pos1, pos2 - pos1);

            }
            catch (Exception ex)
            {
                Logging.WriteLog("METHOD-GetFtEntIdentifier: " + ex.Message);
            }
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CheckForStop()
        {
            while (stop)
            {
                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            int scraped = 0;
            int foundc = 0;
            string[] keywords = txtKeywords.Text.Split(';');
            if (string.IsNullOrEmpty(txtKeywords.Text))
            {
                lblStatus.Text = "Scraping [" + scraped + "]";
            }
            else
            {
                lblStatus.Text = "Search + Scraping [checked :" + scraped + "] [found : " + foundc + "]";
            }
            int i = 0;
            int postsIndex = 0;
            while (!stop)
            {
                CheckForStop();
                dynamic posts = GetPosts(25 * i);
                int numberOfPosts = posts.data.Count;
                if (posts != null && numberOfPosts > 0)
                {
                    for (int j = 0; j < numberOfPosts; j++)
                    {
                        CheckForStop();
                        if (postsIndex == postsLimit)
                        {
                            btnStart.Enabled = true;
                            return;
                        }

                        dynamic post = posts.data[j];
                        if (post != null)
                        {
                            string postMessage = string.IsNullOrEmpty(post.message) ? "" : post.message;
                            string postStory = string.IsNullOrEmpty(post.story) ? "" : post.story;
                            string body = postMessage + postStory;
                            body = body.ToLower();
                            bool found = false;
                            if (keywords.Length > 0)
                            {
                                foreach (string keyword in keywords)
                                {
                                    if (body.IndexOf(keyword.ToLower()) != -1)
                                    {
                                        found = true;
                                    }
                                }
                            }
                            else
                            {
                                found = true;
                            }
                            if (!found)
                            {
                                scraped++;
                                if (txtKeywords.Text == "")
                                {
                                    lblStatus.Text = "Scraping [" + scraped + "]";
                                }
                                else
                                {
                                    lblStatus.Text = "Search + Scraping [checked :" + scraped + "] [found : " + foundc + "]";
                                }
                                continue;
                            }
                            foundc++;
                            if (string.IsNullOrEmpty(txtKeywords.Text))
                            {
                                lblStatus.Text = "Scraping [" + scraped + "]";
                            }
                            else
                            {
                                lblStatus.Text = "Search + Scraping [checked :" + scraped + "] [found : " + foundc + "]";
                            }
                            string title = GetPostPrev(post);
                            string postsTime = GetPostTime(post);
                            string extractionFolder = Path.Combine(dataPath, page, postsTime + (title.Length > 0 ? "_" + title : ""));
                            if (!Directory.Exists(extractionFolder))
                            {
                                Directory.CreateDirectory(extractionFolder);
                            }

                            string mediaFolder = Path.Combine(extractionFolder, ConfigurationManager.AppSettings["FolderMedia"]);
                            if (!Directory.Exists(mediaFolder))
                            {
                                Directory.CreateDirectory(mediaFolder);
                            }
                            dynamic parameters = new ExpandoObject();
                            parameters.fields = "attachments";
                            dynamic attachments = facebookClient.Get(post.id, parameters);
                            post.media_pictures = new List<string>();
                            //need to improve because out of range
                            if (attachments != null)
                            {
                                if (attachments.attachments != null)
                                {
                                    int numberOfAtts = attachments.attachments.Count;
                                    if (numberOfAtts > 0)
                                    {
                                        for (int mi = 0; mi < numberOfAtts; mi++)
                                        {
                                            dynamic media = attachments.attachments.data[mi].media;
                                            if (media != null)
                                            {
                                                string imageUrl = attachments.attachments.data[mi].media.image.src;
                                                if (!string.IsNullOrEmpty(imageUrl))
                                                {
                                                    SaveFileFromURL(imageUrl, Path.Combine(mediaFolder, (mi + 1) + ".jpg"), 120);
                                                    post.media_pictures.Add(imageUrl);
                                                }
                                                if (attachments.attachments.data[mi].subattachments != null)
                                                {
                                                    int numberOfSubAtts = attachments.attachments.data[mi].subattachments.Count;
                                                    if (numberOfSubAtts > 0)
                                                    {
                                                        for (int smi = 0; smi < numberOfSubAtts; smi++)
                                                        {
                                                            dynamic subMedia = attachments.attachments.data[mi].subattachments.data[smi].media;
                                                            if (subMedia != null)
                                                            {
                                                                string subImageUrl = attachments.attachments.data[mi].subattachments.data[smi].media.image.src;
                                                                if (!string.IsNullOrEmpty(subImageUrl))
                                                                {
                                                                    SaveFileFromURL(subImageUrl, Path.Combine(mediaFolder, (smi + 1) + ".jpg"), 120);
                                                                    post.media_pictures.Add(subImageUrl);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            PostExcel excel = new PostExcel(Path.Combine(extractionFolder, "data.xlsx"));
                            string postArgs = post.id;
                            if (!string.IsNullOrEmpty(postArgs))
                            {
                                string storyUrl = ConfigurationManager.AppSettings["StoryUri"] + postArgs;
                                webBrw.Load(storyUrl);
                                while (!next)
                                {
                                    Thread.Sleep(10);
                                }
                                Thread.Sleep(5000);
                                int lastHeight = 0;
                                while (true)
                                {
                                    CheckForStop();
                                    Task<JavascriptResponse> expandCommentsTask = webBrw.EvaluateScriptAsync("document.getElementById('see_next_" + postArgs.Split('_')[0] + "').getElementsByTagName('a')[0].click();");
                                    expandCommentsTask.Wait();
                                    if (!expandCommentsTask.Result.Success)
                                    {
                                        break;
                                    }
                                    Thread.Sleep(500);

                                    Task<JavascriptResponse> getHightTask = webBrw.EvaluateScriptAsync("document.documentElement.scrollHeight - document.documentElement.clientHeight+document.documentElement.scrollHeight");
                                    getHightTask.Wait();
                                    int height = int.Parse(getHightTask.Result.Result.ToString());
                                    if (height == lastHeight)
                                    {
                                        webBrw.EvaluateScriptAsync("document.getElementById('see_next_" + postArgs.Split('_')[0] + "').remove();");
                                        break;
                                    }
                                    else
                                    {
                                        lastHeight = height;
                                    }
                                }

                                ScrollPage(extractionFolder, "comments_");
                                string ftEntIdentifier = GetFtEntIdentifier();
                                if (string.IsNullOrEmpty(ftEntIdentifier))
                                {
                                    ftEntIdentifier = postArgs.Split('_')[0];
                                }
                                string profileUrl = ConfigurationManager.AppSettings["ProfileUri"] + postArgs + "/?ft_ent_identifier=" + postArgs;
                                webBrw.Load(profileUrl);
                                while (!next)
                                {
                                    Thread.Sleep(10);
                                }
                                Thread.Sleep(5000);
                                lastHeight = 0;
                            }

                            int clickCount = 0;
                            while (!stop)
                            {
                                CheckForStop();
                                Task<JavascriptResponse> expandCommentsTask = webBrw.EvaluateScriptAsync("document.getElementById('reaction_profile_pager').getElementsByTagName('a')[0].click();");
                                expandCommentsTask.Wait();
                                Thread.Sleep(500);
                                clickCount++;
                                int numberExpandComment = int.Parse(ConfigurationManager.AppSettings["NumberExpandComment"]);
                                if (clickCount == numberExpandComment)
                                {
                                    webBrw.EvaluateScriptAsync("document.getElementById('reaction_profile_pager').getElementsByTagName('a')[0].remove();");
                                    break;
                                }
                            }
                            ScrollPage(extractionFolder, "likes_");
                            parameters = new ExpandoObject();
                            parameters.fields = "source";
                            string mediaId = post.id;
                            dynamic video = facebookClient.Get(mediaId, parameters);
                            post.media_video = null;
                            if (video != null && !string.IsNullOrEmpty(video.source))
                            {
                                post.media_video = video.source;
                                if (!string.IsNullOrEmpty(post.media_video))
                                {
                                    SaveFileFromURL(post.media_video, Path.Combine(mediaFolder, "1.mp4"), 120);
                                }                                
                            }
                            excel.InsertPostInfo(post);

                            int likesIndex = 0;
                            int likesOffset = 0;
                            while (true)
                            {
                                CheckForStop();
                                dynamic likes = GetLikes(post, likesOffset * 25);
                                int numberOfLikes = likes.data.Count;
                                if (numberOfLikes > 0)
                                {
                                    likesOffset++;
                                    for (int l = 0; l < numberOfLikes; l++)
                                    {
                                        dynamic like = likes.data[l];
                                        excel.InsertLikes(like, likesIndex);
                                        likesIndex++;
                                        if (likesIndex == likesLimit)
                                            goto EndOfLikesLoop;
                                    }
                                }
                                else
                                {
                                    goto EndOfLikesLoop;
                                }
                            }
                            EndOfLikesLoop:;
                            int commentsIndex = 0;
                            int commentsOffset = 0;
                            while (true)
                            {
                                CheckForStop();
                                dynamic comments = GetComments(post, commentsOffset * 25);
                                if (comments != null)
                                {
                                    int numberOfComments = comments.data.Count;
                                    if (numberOfComments > 0)
                                    {
                                        commentsOffset++;
                                        for (int c = 0; c < numberOfComments; c++)
                                        {
                                            dynamic comment = comments.data[c];
                                            excel.InsertComments(comment, commentsIndex);
                                            commentsIndex++;
                                            if (commentsIndex == commentsLimit)
                                                goto EndOfCommentsLoop;
                                        }
                                    }
                                    else
                                    {
                                        goto EndOfCommentsLoop;
                                    }
                                }
                            }
                            EndOfCommentsLoop:;
                            excel.Close();
                            postsIndex++;
                            scraped++;

                            if (txtKeywords.Text == "")
                            {
                                lblStatus.Text = "Scraping [" + scraped + "]";
                            }
                            else
                            {
                                lblStatus.Text = "Search + Scraping [checked :" + scraped + "] [found : " + foundc + "]";
                            }
                        }
                        else
                        {
                            lblStatus.Text = "Completed";
                            MessageBox.Show(null, "Scraping completed", "facebook scraper", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnStart.Enabled = true;
                            btnStop.Enabled = false;
                            return;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(page))
                    {
                        string extractionFolder = Path.Combine(dataPath, page);
                        if (!Directory.Exists(extractionFolder))
                        {
                            Directory.CreateDirectory(extractionFolder);
                        }
                        webBrw.Load("https://www.facebook.com/" + page);
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        MessageBox.Show("Page is empty");
                        break;
                    }
                }
                i++;
                Thread.Sleep(50);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!started)
            {
                stop = false;
                StartStopRecording();
                postsLimit = int.Parse(txtPostsLimit.Text);
                likesLimit = int.Parse(txtLikesLimit.Text);
                commentsLimit = int.Parse(txtCommentsLimit.Text);
                facebookClient = new FacebookClient(accessToken);
                facebookClient.AppId = ConfigurationManager.AppSettings["AppId"];
                facebookClient.AppSecret = ConfigurationManager.AppSettings["AppSecret"];
                btnStart.Enabled = false;
                page = txtPageUrl.Text;
                btnStop.Enabled = true;
                Thread t = new Thread(Start);
                t.Start();
                started = true;
            }
            else
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                stop = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            //btnStart.Text = "Resume";
            btnStart.Enabled = true;
            stop = true;
            started = false;
            txtPageUrl.Focus();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScrollSpeedTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string RemoveDiacriticals(string fileName)
        {
            string nfd = fileName.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder retval = new System.Text.StringBuilder(nfd.Length);
            foreach (char ch in nfd)
            {
                if ((ch >= '\u0300' && ch <= '\u036f')
                   || (ch >= '\u1dc0' && ch <= '\u1de6')
                   || (ch >= '\ufe20' && ch <= '\ufe26')
                   || (ch >= '\u20d0' && ch <= '\u20f0'))
                    continue;

                retval.Append(ch);
            }
            return retval.ToString();
        }
    }

    public static class User32
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
    }

    public class Gdi32
    {
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public static class Utilities
    {
        public static Image CaptureScreen()
        {
            return CaptureWindow(User32.GetDesktopWindow());
        }

        public static Image DrawToImage(this Control control)
        {
            return CaptureWindow(control.Handle);
        }

        public static Image CaptureWindow(IntPtr handle)
        {

            IntPtr hdcSrc = User32.GetWindowDC(handle);

            RECT windowRect = new RECT();
            User32.GetWindowRect(handle, ref windowRect);

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            IntPtr hdcDest = Gdi32.CreateCompatibleDC(hdcSrc);
            IntPtr hBitmap = Gdi32.CreateCompatibleBitmap(hdcSrc, width, height);

            IntPtr hOld = Gdi32.SelectObject(hdcDest, hBitmap);
            Gdi32.BitBlt(hdcDest, 0, 0, width, height, hdcSrc, 0, 0, 13369376);
            Gdi32.SelectObject(hdcDest, hOld);
            Gdi32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle, hdcSrc);

            Image image = Image.FromHbitmap(hBitmap);
            Gdi32.DeleteObject(hBitmap);

            return image;
        }
    }

    public class PostExcel
    {
        private string FilePath;

        private ExcelPackage Package;

        public PostExcel(string filepath)
        {
            FilePath = filepath;
            using (FileStream fs = File.Open(FilePath, FileMode.Create))
            {
                fs.Write(Properties.Resources.PostCanva, 0, Properties.Resources.PostCanva.Length);
            }
            Package = new ExcelPackage(new FileInfo(FilePath));
        }

        public void InsertPostInfo(dynamic post)
        {
            ExcelWorksheet worksheet = Package.Workbook.Worksheets.First();
            int index = 2;
            worksheet.Cells["A" + index].Value = "https://www.facebook.com/" + post.id;
            worksheet.Cells["G" + index].Value = ((post.message == null) ? "" : post.message) + ((post.story == null) ? "" : post.story);
            string media_str = post.media_video;
            foreach (string pic in post.media_pictures)
            {
                media_str += "\n" + pic;
            }
            worksheet.Cells["M" + index].Value = media_str;
        }

        public void InsertLikes(dynamic like, int index)
        {
            index += 2;
            ExcelWorksheet worksheet = Package.Workbook.Worksheets[2];
            worksheet.Cells["A" + index].Value = like.name;
            worksheet.Cells["B" + index].Value = "https://www.facebook.com/" + like.id;
        }

        public void InsertShares(dynamic share, int index)
        {
            index += 2;
            ExcelWorksheet worksheet = Package.Workbook.Worksheets[3];
            worksheet.Cells["A" + index].Value = share.story;
            worksheet.Cells["B" + index].Value = "https://www.facebook.com/" + share.id;
        }

        public void InsertComments(dynamic comment, int index)
        {
            index += 2;
            ExcelWorksheet worksheet = Package.Workbook.Worksheets[3];
            worksheet.Cells["A" + index].Value = comment.from.name;
            worksheet.Cells["B" + index].Value = "https://www.facebook.com/" + comment.from.id;
            worksheet.Cells["C" + index].Value = comment.message;
        }

        public void Close()
        {
            Package.Save();
            Package.Dispose();
        }
    }
}
