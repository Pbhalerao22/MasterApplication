using Newtonsoft.Json;

namespace MasterApplication.Services
{
    class MailBody
    {

        public From from { get; set; }
        public string subject { get; set; }
        public Content[] content { get; set; }
        public Attachments[] attachments { get; set; }
        public Personalizations[] personalizations { get; set; }
        public MailBody()
        {

        }
        public MailBody(From _from, string _subject, Content[] _content, Attachments[] _Attachments, Personalizations[] _Personalizations)
        {
            from = _from;
            subject = _subject;
            content = _content;
            attachments = _Attachments;
            personalizations = _Personalizations;
        }
    }
    public class From
    {
        public string email { get; set; }
        public string name { get; set; }
    }
    public class Content
    {
        public string type { get; set; }
        public string value { get; set; }
        public Content(string _type, string _value)
        {
            type = _type;
            value = _value;
        }
    }

    public class Attachments
    {
        public string name { get; set; }
        public string content { get; set; }
        public Attachments(string _name, string _content)
        {
            name = _name;
            content = _content;
        }
    }
    public class Personalizations
    {
        public To[] to { get; set; }
        public Personalizations(To[] _to)
        {
            to = _to;
        }
    }

    public class To
    {
        public string email { get; set; }
        public To(string _email)
        {
            email = _email;
        }
    }
    public class CC
    {
        public string email { get; set; }
        public CC(string _email)
        {
            email = _email;
        }
    }
    public class BCC
    {
        public string email { get; set; }
        public BCC(string _email)
        {
            email = _email;
        }
    }


    public class Email_Json
    {
        public string userid { get; set; }
        public string pwd { get; set; }
        public string subuid { get; set; }
        public string dcode { get; set; }
        public string[] to { get; set; }
        public string[] cc { get; set; }
        public string[] bcc { get; set; }
        public string msgtype { get; set; }
        public string from { get; set; }
        public string subjectline { get; set; }
        public string ctype { get; set; }
        public string msgtxt { get; set; }
        public string tempname { get; set; }
        public ACL_Attachments[] attachments { get; set; }
        public string[] variables { get; set; }
    }
    public class ACL_Attachments
    {
        public string content { get; set; }
        public string filename { get; set; }
        public string disposition { get; set; }

        [JsonProperty(PropertyName = "content-id")]
        public string content_id { get; set; }
        public string type { get; set; }
        public ACL_Attachments(string _content, string _filename, string _disposition, string _content_id, string _type)
        {
            content = _content;
            filename = _filename;
            disposition = _disposition;
            content_id = _content_id;
            type = _type;
        }
    }
}
