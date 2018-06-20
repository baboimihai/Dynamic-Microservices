using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfJobNSpace
{
    public static class PdfJob
    {
        public static byte[] WriteDiploma(string name)
        {
            string oldFile = "https://drive.google.com/file/d/1u4fJLQ9HHIwnwbdNBA0FMaJBFBBkn4DX/view?usp=sharing";
 
            // open the reader
            PdfReader reader = new PdfReader(new Uri(oldFile));
            Rectangle size = reader.GetPageSizeWithRotation(1);
            Document document = new Document(size);

            // open the writer
            var fs = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            // the pdf content
            PdfContentByte cb = writer.DirectContent;
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetFontAndSize(bf, 8);

            cb.BeginText();
            cb.ShowTextAligned(1, name, 520, 640, 0);
            cb.EndText();
            document.Close();
            writer.Close();
            reader.Close();
            return fs.ToArray();
        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
