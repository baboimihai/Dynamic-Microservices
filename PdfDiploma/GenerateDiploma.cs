using iTextSharp.text;
using iTextSharp.text.pdf;
using Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PdfDiploma
{
    public interface IGenerateDiploma
    {
        byte[] WriteDiploma(string name);
    }
    public class GenerateDiploma : MicroCore<string, byte[]>, IGenerateDiploma
    {
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
        public byte[] WriteDiploma(string name)
        {
            return Run(name);
        }

        protected override byte[] ProcessTask(string name)
        {
            using (MemoryStream myMemoryStream = new MemoryStream())
            {
                PdfReader reader =
                  new PdfReader(new Uri("https://www.pdf-archive.com/2018/05/14/diploma/diploma.pdf"));
                Rectangle size = reader.GetPageSizeWithRotation(1);

                Document myDocument = new Document(size);
                PdfWriter myPDFWriter = PdfWriter.GetInstance(myDocument, myMemoryStream);

                myDocument.Open();
                PdfContentByte cb = myPDFWriter.DirectContent;
                PdfImportedPage page = myPDFWriter.GetImportedPage(reader, 1);
                cb.AddTemplate(page, 0, 0);
                ColumnText.ShowTextAligned(cb, Element.PARAGRAPH,
        new Phrase(name, new Font(Font.FontFamily.HELVETICA, 25, 0, BaseColor.BLACK)), 250, 600, 0);

                myDocument.Close();
                byte[] content = myMemoryStream.ToArray();
                return content;
}

            //WebClient client = new WebClient();
            //var stream = client.DownloadData("http://www.docdroid.net/OhxkA5E/diploma.pdf");

            //PdfReader reader = new PdfReader(stream);
            //Rectangle size = reader.GetPageSizeWithRotation(0);
            //Document document = new Document(size);

            //// open the writer
            //var fs = new MemoryStream();
            //PdfWriter writer = PdfWriter.GetInstance(document, fs);
            //document.Open();

            //// the pdf content
            //PdfContentByte cb = writer.DirectContent;
            //BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            //cb.SetColorFill(BaseColor.DARK_GRAY);
            //cb.SetFontAndSize(bf, 8);

            //cb.BeginText();
            //cb.ShowTextAligned(1, name, 520, 640, 0);
            //cb.EndText();
            //document.Close();
            //writer.Close();
            //reader.Close();
            //return fs.ToArray();
        }
    }
}
