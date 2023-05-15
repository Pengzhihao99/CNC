#if SDK
namespace MessageCore.OpenApi.SDK.Definitions
#else
namespace MessageCore.Application.OpenApi.DataTransferModels
#endif
{
    public class AttachmentDto
    {
        /// <summary>
        /// Base64String
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 文件名（包括后缀）
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 建议使用MessageCore.OpenApi.SDK.Definitions.Constant.AttachmentType赋值
        /// </summary>
        public string AttachmentType { get; set; }
    }
}
