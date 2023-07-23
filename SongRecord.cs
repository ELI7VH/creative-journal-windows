using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace DaemonRecorder {
    public class SongRecord {
        public JsonNode json;
        public string id;
        public string link;
        public string name;
        public string folder;

        public SongRecord(JsonNode _json) {
            this.json = _json;

            this.id = json["id"].ToString();
            var name = json["name"];
            var geneartedName = json["metadata"]["name"];

            this.name = (name ?? geneartedName).ToString();

            this.link = json["metadata"]["link"].ToString();
            this.folder = json["metadata"]["folder"].ToString();
        }

        public string ToRow() {
            return $"{name}";
        }
    }

    public static class SongRecordList {
        public static List<string> ToRows(List<SongRecord> songs) {
            return songs.ConvertAll((song) => song.ToRow());
        }
    }
}
