using System.Text.Json.Serialization;

namespace AwsLab.Web.DTO
{
    public class ResponseUpload
    {
        [JsonPropertyName("erro")]
        public string Erro { get; set; } = string.Empty;
    }
}