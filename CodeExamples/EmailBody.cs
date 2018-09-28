using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recruit.Models.Emails
{
    public class EmailBody
    {
        private RecruitEntities db = new RecruitEntities();
        private static RecruitEntities re = new RecruitEntities();
        public EmailBody()
        {

        }

        public EmailMessage GetEmailBody(string Template, Dictionary<string, string> replacementValues)
        {
            EmailMessage emessage = new EmailMessage();
            //Get the template and then parse variables into it
            var emailbody = db.email_template.Where(e => e.tag == Template).SingleOrDefault();
            if (emailbody == null)
            {
                return null;
            }

            emessage.subject = emailbody.title;
            string bodyText = emailbody.text;
            if (!string.IsNullOrEmpty(emailbody.possible_variables))
            {
                string[] replaceVals = emailbody.possible_variables.Split(',');
                if (replacementValues != null)
                {
                    foreach (string val in replaceVals)
                    {
                        if (replacementValues.ContainsKey(val))
                        {
                            string value = replacementValues[val];
                            bodyText = bodyText.Replace(val, value);
                        }
                        else
                        {
                            bodyText = bodyText.Replace(val, string.Empty);
                        }
                    }
                }
            }
            string masterText = GetMasterEmailBody(bodyText);
            emessage.body = masterText;

            return emessage;
        }

        public static string GetMasterEmailBody(string body)
        {
            var masterTemplate = re.email_template.Where(e => e.tag == "Master_Template").SingleOrDefault();
            string masterText = masterTemplate.text;
            masterText = masterText.Replace("%MainBody%", body);

            return masterText;
        }
}
}
