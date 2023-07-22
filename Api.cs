using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace DaemonRecorder {
    internal class Api {
        public static string Url(string resource) {
            return $"https://elijahlucian.ca/api/{resource}";
        }

        public static JsonArray GetMetadata(string scope) {
            var url = $"https://elijahlucian.ca/api/metadata/upload?scope={scope}";

            var response = Get(url);

            if (response != null) {
                return ToJsonArray(response);
            }

            return null;
        }

        public static string Get(string url) {
            var client = new System.Net.Http.HttpClient();

            var response = client.GetAsync(url).Result;
            if (response != null) {
                var content = response.Content.ReadAsStringAsync().Result;

                return content;
            }

            return null;
        }

        public static List<SongRecord> GetSongs() {
            var json = GetMetadata("music");

            return json.Select((item) => new SongRecord(item)).ToList();
        }

        private static JsonArray ToJsonArray(string json) {
            return JsonArray.Parse(json).AsArray();
        }
    }


}
