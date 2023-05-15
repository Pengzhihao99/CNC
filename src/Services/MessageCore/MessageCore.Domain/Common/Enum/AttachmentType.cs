using Framework.Domain.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageCore.Domain.Common.Enum
{
    /// <summary>
    /// https://github.com/jstedfast/MimeKit/blob/master/MimeKit/MimeTypes.cs
    /// </summary>
    public class AttachmentType : Enumeration
    {
        public string MimeType { get; private set; }
        public AttachmentType(int id, string name, string mimeType) : base(id, name)
        {
            MimeType = mimeType;
        }

        [Description("XLSX")]
        public static AttachmentType Xlsx = new AttachmentType(1, "XLSX", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        [Description("XLS")]
        public static AttachmentType Xls = new AttachmentType(2, "XLS", "application/vnd.ms-excel");

        [Description("PDF")]
        public static AttachmentType Pdf = new AttachmentType(3, "PDF", "application/pdf");

        [Description("TXT")]
        public static AttachmentType TXT = new AttachmentType(4, "TXT", "text/plain");

        [Description("DOC")]
        public static AttachmentType DOC = new AttachmentType(5, "DOC", "application/msword");

        [Description("DOCX")]
        public static AttachmentType DOCX = new AttachmentType(6, "DOCX", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");

        [Description("PNG")]
        public static AttachmentType PNG = new AttachmentType(7, "PNG", "image/png");

        [Description("JPG")]
        public static AttachmentType JPG = new AttachmentType(8, "JPG", "image/jpeg");
    }
}
