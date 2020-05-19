using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using TwitterCrawlerLib.Core;

namespace TwitterCrawlerLib.Web
{
    public class TwitterSearch
    {
        private Dictionary<string, string> urlEncodeDictionary;

        public string Credentials { get; set; }
        public string AccessToken { get; set; }
        public string ResultType { get; set; } = "recent";
        public int RPP { get; set; } = 15;

        public TwitterSearch()
        {
            InitializeEncodingDictionary();
        }

        public TwitterSearch(string credential)
        {
            Credentials = credential;
            InitializeEncodingDictionary();
            Authorize();
        }

        public TwitterSearch(string newCredential, int newRPP)
        {
            Credentials = newCredential;
            RPP = newRPP;
            InitializeEncodingDictionary();
            Authorize();
        }

        public TwitterSearch(string newCredential, string resultType)
        {
            Credentials = newCredential;
            ResultType = resultType;
            InitializeEncodingDictionary();
            Authorize();
        }

        public TwitterSearch(string newCredential, string resultType, int newRPP)
        {
            Credentials = newCredential;
            ResultType = resultType;
            RPP = newRPP;
            InitializeEncodingDictionary();
            Authorize();
        }

        private void InitializeEncodingDictionary()
        {
            urlEncodeDictionary = new Dictionary<string, string>();

            urlEncodeDictionary[" "] = "%20";
            urlEncodeDictionary["!"] = "%21";
            urlEncodeDictionary["\""] = "%22";
            urlEncodeDictionary["#"] = "%23";
            urlEncodeDictionary["$"] = "%24";
            urlEncodeDictionary["%"] = "%25";
            urlEncodeDictionary["&"] = "%26";
            urlEncodeDictionary["'"] = "%27";
            urlEncodeDictionary["("] = "%28";
            urlEncodeDictionary[")"] = "%29";
            urlEncodeDictionary["@"] = "%40";
            urlEncodeDictionary["`"] = "%60";
            urlEncodeDictionary["*"] = "%2A";
            urlEncodeDictionary["+"] = "%2B";
            urlEncodeDictionary[","] = "%2C";
            urlEncodeDictionary["-"] = "%2D";
            urlEncodeDictionary["."] = "%2E";
            urlEncodeDictionary["/"] = "%2F";
            urlEncodeDictionary[":"] = "%3A";
            urlEncodeDictionary[";"] = "%3B";
            urlEncodeDictionary["<"] = "%3C";
            urlEncodeDictionary["="] = "%3D";
            urlEncodeDictionary[">"] = "%3E";
            urlEncodeDictionary["?"] = "%3F";
            urlEncodeDictionary["["] = "%5B";
            urlEncodeDictionary["\\"] = "%5C";
            urlEncodeDictionary["]"] = "%5D";
            urlEncodeDictionary["^"] = "%5E";
            urlEncodeDictionary["{"] = "%7B";
            urlEncodeDictionary["|"] = "%7C";
            urlEncodeDictionary["}"] = "%7D";
            urlEncodeDictionary["~"] = "%7E";
        }

        public void Authorize()
        {
            var post = WebRequest.Create("https://api.twitter.com/oauth2/token");
            post.Method = "POST";
            post.ContentType = "application/x-www-form-urlencoded";
            post.Headers[HttpRequestHeader.Authorization] = "Basic " + Credentials;
            var reqBody = Encoding.UTF8.GetBytes("grant_type=client_credentials");
            post.ContentLength = reqBody.Length;

            using (var req = post.GetRequestStream())
            {
                req.Write(reqBody, 0, reqBody.Length);
            }

            try
            {
                string responseBody = null;
                using (var resp = post.GetResponse().GetResponseStream())
                {
                    var respR = new StreamReader(resp);
                    responseBody = respR.ReadToEnd();
                }

                AccessToken = responseBody.Substring(
                    responseBody.IndexOf("access_token\":\"") + "access_token\":\"".Length,
                    responseBody.IndexOf("\"}") - (responseBody.IndexOf("access_token\":\"") + "access_token\":\"".Length));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string EncodeSearch(string[] newSearchTerms)
        {
            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < newSearchTerms.Length; i++)
            {
                char[] termParts = newSearchTerms[i].ToCharArray();
                for (int o = 0; o < termParts.Length; o++)
                {
                    if (urlEncodeDictionary.ContainsKey(termParts[o].ToString()))
                    {
                        newSearchTerms[i] = newSearchTerms[i].Replace(
                            termParts[o].ToString(),
                            urlEncodeDictionary[termParts[o].ToString()]);
                    }
                }
                stringBuilder.Append(newSearchTerms[i]);
            }

            return stringBuilder.ToString();
        }

        public APISearchResult APIWebRequest_W_Metadata(string url, string[] baseQuery)
        {
            if (AccessToken == string.Empty) { Authorize(); }
            string encodedQuery = EncodeSearch(baseQuery);
            Console.WriteLine("Attempting Query: " + url + encodedQuery + "&result_type=" + ResultType + "&count=" + RPP.ToString() + "&include_entities=1");
            HttpWebRequest request = WebRequest.Create(url + encodedQuery + "&result_type=" + ResultType + "&count=" + RPP.ToString() + "&include_entities=1") as HttpWebRequest;
            APISearchResult searchResult = new APISearchResult();

            request.Method = "GET";
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + AccessToken;

            try
            {
                string responseBody = null;
                using (var resp = request.GetResponse().GetResponseStream())
                {
                    var responseReader = new StreamReader(resp);
                    responseBody = responseReader.ReadToEnd();

                    searchResult = JsonConvert.DeserializeObject<APISearchResult>(responseBody);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught Exception in CallUrl: " + ex.Message);
            }

            return searchResult;
        }

        public Tweet[] APIWebRequest_N_Metadata(string url, string[] baseQuery)
        {
            if (AccessToken == string.Empty) { Authorize(); }
            string encodedQuery = EncodeSearch(baseQuery);
            Console.WriteLine("Attempting Query: " + url + encodedQuery + "&result_type=" + ResultType + "&count=" + RPP.ToString() + "&include_entities=1");
            HttpWebRequest request = WebRequest.Create(url + encodedQuery + "&result_type=" + ResultType + "&count=" + RPP.ToString() + "&include_entities=1") as HttpWebRequest;

            request.Method = "GET";
            request.Headers[HttpRequestHeader.Authorization] = "Bearer " + AccessToken;

            try
            {
                string responseBody = null;
                using (var resp = request.GetResponse().GetResponseStream())
                {
                    var responseReader = new StreamReader(resp);
                    responseBody = responseReader.ReadToEnd();

                    responseBody = responseBody.Substring(0, responseBody.IndexOf("\"search_metadata\"") - 1) + "}";
                    int stringOffset = responseBody.IndexOf("\"statuses\":") + "\"statuses\":".Length;
                    responseBody = responseBody.Substring(stringOffset, responseBody.Length - (stringOffset + 1));
                    Tweet[] tweets = JsonConvert.DeserializeObject<Tweet[]>(responseBody);
                    if (tweets.Length > 0) { return tweets; }
                    else { return null; }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Caught Exception in CallUrl: " + ex.Message);
            }

            return null;
        }
    }
}
